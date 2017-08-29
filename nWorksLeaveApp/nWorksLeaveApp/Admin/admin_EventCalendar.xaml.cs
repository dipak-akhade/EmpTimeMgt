using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Linq;
using System.Diagnostics;


using Xamarin.Forms.Xaml;
using nWorksLeaveApp.Common;

namespace nWorksLeaveApp.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class admin_EventCalendar : ContentPage
    {
        ObservableCollection<ModelEventCalendar> model = new ObservableCollection<ModelEventCalendar>();

        public admin_EventCalendar()
        {
            InitializeComponent();

            BackgroundColor = ColorResources.PageBackgroundColor;

            getEvents();

        }
        async public void Submit_Click(object sender, EventArgs e)
        {
            try
            {
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpClient Client = new HttpClient();
                HttpResponseMessage response = null;
                response = await Client.PostAsync(ColorResources.baseUrl + "uploadeventcalendar", content);
                if (response.IsSuccessStatusCode)
                {
                    var content1 = await response.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<string>(content1);
                    await DisplayAlert(" nWorksLeaveApp", res.ToString(), "OK");
                    await this.Navigation.PopAsync();

                }
            }
            catch (Exception ex)
            {
                await DisplayAlert(" nWorksLeaveApp", "Unable to connect server, Try again!", "OK");
                Debug.WriteLine(ex.ToString());
            }
        }
        public void Add_Click(object sender, EventArgs e)
        {
            try
            {
                if (picker_occasion.SelectedIndex == -1)
                {
                    DisplayAlert(" nWorksLeaveApp", "Select Occasion please!", "OK");
                }
                else if (model.Any(p => p.Occasion == picker_occasion.Items[picker_occasion.SelectedIndex].ToString()) == false)
                {
                    model.Add(new ModelEventCalendar
                    {
                        OccasionDate = string.Format("{0:yyyy-MM-dd HH:mm:ss}", choosedDate.Date),
                        Occasion = picker_occasion.Items[picker_occasion.SelectedIndex].ToString(),
                        OccasionWeekDay = choosedDate.Date.DayOfWeek.ToString()
                    });

                    listview_Events.ItemsSource = model;

                    DisplayAlert(picker_occasion.Items[picker_occasion.SelectedIndex].ToString() + " Added!", "Alert", "OK");

                }
                else
                    DisplayAlert(" nWorksLeaveApp", "Already Exist!", "OK");
            }
            catch (Exception ex)
            {
                DisplayAlert(" nWorksLeaveApp", ex.ToString(), "OK");
            }
        }

        public void onDeleteClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var itemForRemoving = button.BindingContext as ModelEventCalendar;
            model.Remove(itemForRemoving);
            //			DisplayAlert (itemForRemoving.Occasion.ToString () + " Deleted!", "Alert", "OK");
        }
        public void getEvents()
        {
            picker_occasion.Items.Add("New Year Day");
            picker_occasion.Items.Add("Lohri");
            picker_occasion.Items.Add("Pongal");
            picker_occasion.Items.Add("Makar Sakranti");
            picker_occasion.Items.Add("Guru Gobind Singh Jayanti");
            picker_occasion.Items.Add("Republic Day");
            picker_occasion.Items.Add("Shiv Ratri");
            picker_occasion.Items.Add("Holi");
            picker_occasion.Items.Add("Good Friday");
            picker_occasion.Items.Add("Gudi Padwa");
            picker_occasion.Items.Add("Baisakhi");
            picker_occasion.Items.Add("Dr. Ambedkar Jayanti");
            picker_occasion.Items.Add("Mahavir Jayanti");
            picker_occasion.Items.Add("May Day");
            picker_occasion.Items.Add("Akshay Tritiya");
            picker_occasion.Items.Add("Buddha Jayanti");
            picker_occasion.Items.Add("Jagannath Yatra");
            picker_occasion.Items.Add("Id-ul-Fitr");
            picker_occasion.Items.Add("Independence Day");
            picker_occasion.Items.Add("Parsi New Year");
            picker_occasion.Items.Add("Rakshabandhan");
            picker_occasion.Items.Add("Janmashtami");
            picker_occasion.Items.Add("Ganesh Jayanti");
            picker_occasion.Items.Add("Eid-Ul-Juha");
            picker_occasion.Items.Add("Onam");
            picker_occasion.Items.Add("Gandhi Jayanti");
            picker_occasion.Items.Add("Muharram");
            picker_occasion.Items.Add("Dashehara");
            picker_occasion.Items.Add("Diwali");
            picker_occasion.Items.Add("Guru Nanak Jayanti");
            picker_occasion.Items.Add("Eid-e-Milad");
            picker_occasion.Items.Add("Christmas");
        }
    }
}