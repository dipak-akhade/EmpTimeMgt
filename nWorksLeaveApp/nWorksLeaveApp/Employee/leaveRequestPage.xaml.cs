using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms.Xaml;
using nWorksLeaveApp.Common;

namespace nWorksLeaveApp.Employee
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class leaveRequestPage : ContentPage
    {
        //List<dateListModel> selectedDates = new List<dateListModel>{ };

        ObservableCollection<dateListModel> model = new ObservableCollection<dateListModel>();
        //		List<ModelEventCalendar>  allEventDates;
        List<ModelEventCalendarForUser> Items;
        public leaveRequestPage()
        {
            InitializeComponent();
            BackgroundColor = ColorResources.PageBackgroundColor;
            InitializePicker();
            getAllEventDates();
            choosedDate.MinimumDate = DateTime.Today.Date;
        }
        public void OnDateSelected(object sender, EventArgs e)
        {
            DateTime selectedDate = choosedDate.Date;
            if (selectedDate.DayOfWeek.ToString() == "Sunday")
            {
                DisplayAlert("Alert", "Ohh, You choose Sunday!", "OK");
                choosedDate.Date = DateTime.Today.Date;
            }
            else if (selectedDate.DayOfWeek.ToString() == "Saturday")
            {
                DisplayAlert("Alert", "Ohh, You choose Saturday!", "OK");
                choosedDate.Date = DateTime.Today.Date;
            }
        }
        public void InitializePicker()
        {
            requestFor.Items.Add("Leave ");
            requestFor.Items.Add("Holiday ");
            requestFor.Items.Add("Half Day ");
            requestFor.SelectedIndex = 0;
        }
        public void onSelectedIndexChanged_picker(object sender, EventArgs e)
        {
            if (requestFor.SelectedIndex == 1)
            {
                listview_Holiday.IsVisible = true;
            }
            else
            {
                listview_Holiday.IsVisible = false;
            }
        }
        public void Holiday_Tapped(object sender, ItemTappedEventArgs e)
        {
            var selectedItem = e.Item as ModelEventCalendarForUser;
            try
            {
                if (selectedItem.OccasionWeekDay.ToString() != "Sunday" && selectedItem.OccasionWeekDay.ToString() != "Saturday")
                {
                    if (!model.Any(p => Convert.ToDateTime(p.dateSelected) == Convert.ToDateTime(selectedItem.OccasionDate)))
                    {
                        if (selectedItem.isAllow == true)
                        {
                            listview_Holiday.IsVisible = false;
                            requestFor.SelectedIndex = 0;
                            model.Add(new dateListModel
                            {
                                dateSelected = string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(selectedItem.OccasionDate)),
                                requestFor = "Holiday",
                                id = 1,
                                weekDay = selectedItem.OccasionWeekDay.ToString()
                            });
                            listview_MenuItem.ItemsSource = model;
                        }
                        else
                        {
                            DisplayAlert("Alert", selectedItem.Occasion.ToString() + " is already taken!", "OK");
                        }
                    }
                    else
                    {
                        DisplayAlert("Alert", "Already Exist!", "OK");
                    }
                }
                else
                {
                    DisplayAlert("Alert", "It looks Saturday/Sunday!", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

            if (e == null) return; // has been set to null, do not 'process' tapped event
            ((ListView)sender).SelectedItem = null; // de-select the row
        }


        public void onDeleteClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var itemForRemoving = button.BindingContext as dateListModel;
            model.Remove(itemForRemoving);
        }

        public void listview_MenuItem_Tapped(object sender, ItemTappedEventArgs e)
        {
            if (e == null) return; // has been set to null, do not 'process' tapped event
            Debug.WriteLine("Tapped: " + e.Item);
            ((ListView)sender).SelectedItem = null; // de-select the row
        }

        public void onAddClicked(object sender, EventArgs e)
        {
            try
            {
                if (choosedDate.Date.DayOfWeek.ToString() != "Saturday" && choosedDate.Date.DayOfWeek.ToString() != "Sunday")
                {
                    if (!model.Any(p => Convert.ToDateTime(p.dateSelected) == choosedDate.Date))
                    {
                        model.Add(new dateListModel
                        {
                            dateSelected = string.Format("{0:yyyy-MM-dd}", choosedDate.Date),
                            requestFor = requestFor.Items[requestFor.SelectedIndex],
                            id = 1,
                            weekDay = choosedDate.Date.DayOfWeek.ToString()
                        });

                        listview_MenuItem.ItemsSource = model;
                    }
                    else
                        DisplayAlert("Alert", "Already Exist!", "OK");
                }
                else
                {
                    DisplayAlert("Alert", "It looks Saturday/Sunday!", "OK");
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", ex.ToString(), "OK");
            }
        }

        async public void RequestTimeOff_Click(object sender, EventArgs e)
        {

            var Answer = await DisplayAlert("Alert", "Are you sure to make a request?", "Yes", "No");
            if (Answer == true)
            {
                await this.Navigation.PushModalAsync(new Loading());

                btnRequestTimeoff.IsEnabled = false;
                List<LeaveRequestData> data = new List<LeaveRequestData>();
                foreach (dateListModel obj in model)
                {
                    LeaveRequestData requestData = new LeaveRequestData();
                    requestData.uid = ColorResources.LIVE_USER_ID;
                    requestData.requestedAs = obj.requestFor.ToString();
                    requestData.requestedDate = obj.dateSelected.ToString();
                    data.Add(requestData);
                }

                if (data.Count != 0)
                {
                    try
                    {
                        var json = JsonConvert.SerializeObject(data);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");
                        HttpClient Client = new HttpClient();
                        HttpResponseMessage response = null;
                        response = await Client.PostAsync(ColorResources.baseUrl + "sendleaverequestmail", content);
                        if (response.IsSuccessStatusCode)
                        {
                            var responseContent = await response.Content.ReadAsStringAsync();
                            await DisplayAlert("Message", responseContent.ToString(), "OK");
                        }
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Alert", "Unable to connect server! Try again!", "OK");
                        Debug.WriteLine(ex.ToString());
                    }
                }
                else
                {
                    await DisplayAlert("Alert", "Select date please!", "OK");
                }

                btnRequestTimeoff.IsEnabled = true;
                await this.Navigation.PopModalAsync();
            }
        }

        async public void getAllEventDates()
        {
            try
            {
                passUid userid = new passUid();
                userid.uid = ColorResources.LIVE_USER_ID.ToString();
                var json = JsonConvert.SerializeObject(userid);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpClient Client = new HttpClient();
                HttpResponseMessage response = null;
                response = await Client.PostAsync(ColorResources.baseUrl + "getEventCalendarForUser", content);
                if (response.IsSuccessStatusCode)
                {
                    var content1 = await response.Content.ReadAsStringAsync();
                    Items = JsonConvert.DeserializeObject<List<ModelEventCalendarForUser>>(content1);
                    listview_Holiday.ItemsSource = Items;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert", "Unable to connect server! Try again!", "OK");
            }
        }
        public void onHolidaySelected(object sender, SelectedItemChangedEventArgs e)
        {
        }
    }

    public class AllEventDates
    {
        public string eventDate { get; set; }
    }
}
