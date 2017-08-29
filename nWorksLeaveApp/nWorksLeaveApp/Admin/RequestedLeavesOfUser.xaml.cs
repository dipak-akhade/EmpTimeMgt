using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Xamarin.Forms.Xaml;
using nWorksLeaveApp.Common;

namespace nWorksLeaveApp.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RequestedLeavesOfUser : ContentPage
    {
        ObservableCollection<UserRequest> modelRequestedDates = new ObservableCollection<UserRequest>();
        ObservableCollection<UserRequest> modelSelectedDates = new ObservableCollection<UserRequest>();
        ObservableCollection<UserRequest> modelDeselectedDates = new ObservableCollection<UserRequest>();

        string UserId = "", UserFullName = "";
        public RequestedLeavesOfUser(string userId, string userFullName)
        {
            InitializeComponent();
            BackgroundColor = ColorResources.PageBackgroundColor;
            this.UserId = userId;
            this.UserFullName = userFullName;
            Title = UserFullName.ToString();
        }
        public void btnSelectDeselect_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            var selection = button.BindingContext as UserRequest;
            Button btnSelectDeselect = button.Parent.FindByName<Button>("btnSelectDeselect");

            if (modelSelectedDates.Any(p => p.leavedate == selection.leavedate) == true)
            {
                btnSelectDeselect.Text = "Select";
                btnSelectDeselect.BackgroundColor = Color.Red;
                modelSelectedDates.Remove(selection);
                modelDeselectedDates.Add(selection);

                //				DisplayAlert ("Alert",selection.leavedate+" "+selection.requestedAs+" is Deseleced","OK");
            }
            else
            {
                btnSelectDeselect.Text = "Selected";
                btnSelectDeselect.BackgroundColor = Color.Green;
                modelSelectedDates.Add(selection);
                modelDeselectedDates.Remove(selection);

                //				DisplayAlert ("Alert",selection.leavedate+" "+selection.requestedAs+" is Seleced","OK");
            }
        }
        public void onWithPayClicked(object sender, EventArgs e)
        {
            var btn = sender as Image;
            var status = btn.BindingContext as UserRequest;

            if (status.isWithPay == "true")
            {
                //				tempEmpList.Remove (employeeCheck);
                btn.Source = "unChecked.png";
                status.isWithPay = "false";
            }
            else
            {
                //				tempEmpList.Add (employeeCheck);
                btn.Source = "checked.png";
                status.isWithPay = "true";
            }
        }
        public void onAcceptClicked(object sender, EventArgs e)
        {
            var btn = sender as Image;
            var status = btn.BindingContext as UserRequest;
            if (status.isAccepted == "true")
            {
                btn.Source = "unChecked.png";
                status.isAccepted = "false";
                modelSelectedDates.Remove(status);
                modelDeselectedDates.Add(status);
            }
            else
            {
                btn.Source = "checked.png";
                status.isAccepted = "true";
                modelSelectedDates.Add(status);
                modelDeselectedDates.Remove(status);
            }
            //			DisplayAlert ("LA",status.isWithPay.ToString()+" "+status.isAccepted.ToString()+" "+status.leavedate.ToString(),"OK");

        }

        async public void btnConfirm_Click(object sender, EventArgs e)
        {
            //			for (int i = 0; i < modelSelectedDates.Count; i++) {
            //				await DisplayAlert ("la",modelSelectedDates[i].leavedate.ToString()+" Accepted with "+modelSelectedDates[i].isWithPay.ToString(),"k");
            //			}
            //			for (int i = 0; i < modelDeselectedDates.Count; i++) {
            //				await DisplayAlert ("la",modelDeselectedDates[i].leavedate.ToString()+" Rejected with "+modelDeselectedDates[i].isWithPay.ToString(),"k");
            //			}
            var Answer = await DisplayAlert(" nWorksLeaveApp", "Are you sure to confirm leaves?", "Yes", "No");
            if (Answer == true)
            {

                btnConfirm.IsEnabled = false;
                MyActivityIndicator.IsVisible = true;
                MyActivityIndicator.IsRunning = true;

                model_ConfirmLeaves confirmLeaves = new model_ConfirmLeaves();   //Get Main object  
                confirmLeaves.userId = UserId;                                  //Set userid property

                Debug.WriteLine(modelSelectedDates.Count.ToString());
                List<model_Dates> List_Leaves = new List<model_Dates>();
                for (int i = 0; i < modelSelectedDates.Count; i++)
                {
                    model_Dates leaves = new model_Dates();
                    leaves.requestedAs = modelSelectedDates[i].requestedAs;
                    leaves.selectedDate = modelSelectedDates[i].leavedate;
                    leaves.isWithPay = modelSelectedDates[i].isWithPay;
                    List_Leaves.Add(leaves);
                }                                                               //set selected dates
                confirmLeaves.Dates = List_Leaves;

                List<model_DeselectedDates> List_DeselectedLeaves = new List<model_DeselectedDates>();
                for (int i = 0; i < modelDeselectedDates.Count; i++)
                {
                    model_DeselectedDates DeselectedLeaves = new model_DeselectedDates();
                    DeselectedLeaves.requestedAs = modelDeselectedDates[i].requestedAs;
                    DeselectedLeaves.selectedDate = modelDeselectedDates[i].leavedate;
                    List_DeselectedLeaves.Add(DeselectedLeaves);
                }
                confirmLeaves.deselectedDates = List_DeselectedLeaves;


                try
                {
                    var json = JsonConvert.SerializeObject(confirmLeaves);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpClient Client = new HttpClient();
                    HttpResponseMessage response = null;
                    response = await Client.PostAsync(ColorResources.baseUrl + "ConfirmLeaves", content);
                    if (response.IsSuccessStatusCode)
                    {
                        var content1 = await response.Content.ReadAsStringAsync();
                        ColorResources.isLeaveConfirm = true;
                        await DisplayAlert("Message", content1.ToString(), "OK");
                        await this.Navigation.PopAsync();
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert(" nWorksLeaveApp", "Unable to connect server, Try again!", "OK");
                    Debug.WriteLine(ex.ToString());
                }
                btnConfirm.IsEnabled = true;
                MyActivityIndicator.IsVisible = true;
                MyActivityIndicator.IsRunning = true;
            }
        }
        async public void getData()
        {
            MyActivityIndicator.IsVisible = true;
            MyActivityIndicator.IsRunning = true;
            try
            {
                HttpClient client = new HttpClient();
                string RestUrl = ColorResources.baseUrl + "GetRequestedLeavesForUser?user=" + UserId;
                var uri = new Uri(string.Format(RestUrl, string.Empty));
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var Items = JsonConvert.DeserializeObject<List<UserRequest>>(content);
                    for (int i = 0; i < Items.Count; i++)
                    {
                        modelRequestedDates.Add(Items[i]);
                        modelSelectedDates.Add(Items[i]);
                    }
                    listview_Leaves.ItemsSource = modelRequestedDates;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert(" nWorksLeaveApp", "Unable to connect server, Try again!", "OK");
                Debug.WriteLine(ex.ToString());
            }
            MyActivityIndicator.IsVisible = false;
            MyActivityIndicator.IsRunning = false;
        }

        async public void getStatus()
        {
            MyActivityIndicator.IsVisible = true;
            MyActivityIndicator.IsRunning = true;
            passUid User = new passUid();
            User.uid = UserId;
            try
            {
                var json = JsonConvert.SerializeObject(User);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpClient Client = new HttpClient();
                HttpResponseMessage response = null;
                response = await Client.PostAsync(ColorResources.baseUrl + "usersHolidayStatus", content);
                if (response.IsSuccessStatusCode)
                {
                    var Content = await response.Content.ReadAsStringAsync();
                    var Items = JsonConvert.DeserializeObject<userHolidayStatus>(Content);

                    y2d.Text = Items.e_total_left_in_current_year2date.ToString();
                    y2m.Text = Items.f_total_left_in_current_year2Month.ToString();
                    y2y.Text = Items.d_total_left_in_current_year.ToString();
                }



                var json1 = JsonConvert.SerializeObject(User);
                var content1 = new StringContent(json1, Encoding.UTF8, "application/json");
                HttpClient Client1 = new HttpClient();
                HttpResponseMessage response1 = null;
                response1 = await Client1.PostAsync(ColorResources.baseUrl + "usersLeaveStatus", content1);
                if (response1.IsSuccessStatusCode)
                {
                    var Content1 = await response1.Content.ReadAsStringAsync();
                    var Items = JsonConvert.DeserializeObject<userLeaveStatus>(Content1);

                    _y2d.Text = Items.i_total_left_in_current_year2date.ToString();
                    _y2m.Text = Items.j_total_left_in_current_year2Month.ToString();
                    _y2y.Text = Items.h_total_left_in_current_year.ToString();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert(" nWorksLeaveApp", "Unable to connect server, Try again!", "OK");
                Debug.WriteLine(ex.ToString());
            }
            MyActivityIndicator.IsVisible = false;
            MyActivityIndicator.IsRunning = false;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            getData();
            getStatus();
        }

    }
}
