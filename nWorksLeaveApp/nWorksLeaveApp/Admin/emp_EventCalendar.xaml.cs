using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Diagnostics;
using Xamarin.Forms.Xaml;
using nWorksLeaveApp.Common;

namespace nWorksLeaveApp.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class emp_EventCalendar : ContentPage
    {
        public emp_EventCalendar()
        {
            InitializeComponent();
            Title = "Event Calendar";
            BackgroundColor = ColorResources.PageBackgroundColor;
            if (ColorResources.LIVE_USER_TYPE == "Employee")
            {
                btnChangeCalendar.IsVisible = false;
            }
        }
        async public void getEventCalendar()
        {
            MyActivityIndicator.IsVisible = true;
            MyActivityIndicator.IsRunning = true;
            try
            {

                HttpClient client = new HttpClient();
                string RestUrl = ColorResources.baseUrl + "geteventcalendar";
                var uri = new Uri(string.Format(RestUrl, string.Empty));
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var Items = JsonConvert.DeserializeObject<List<ModelEventCalendar>>(content);
                    listview_Events.ItemsSource = Items;
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
            getEventCalendar();
        }
        public void ChangeCalendar_Click(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new admin_EventCalendar());
        }
        async public void OnDeleteTapGestureRecognizerTapped(object sender, ItemTappedEventArgs e)
        {
            if (e == null) return; // has been set to null, do not 'process' tapped event
            ((ListView)sender).SelectedItem = null; // de-select the row

            var selection = e.Item as ModelEventCalendar;

            var Answer = await DisplayAlert(" nWorksLeaveApp", "Delete " + selection.Occasion + " event?", "Yes", "No");

            DeleteEvent obj = new DeleteEvent();
            obj.event2delete = selection.Occasion;
            if (Answer == true)
            {
                try
                {
                    var json = JsonConvert.SerializeObject(obj);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpClient Client = new HttpClient();
                    HttpResponseMessage response = null;
                    response = await Client.PostAsync(ColorResources.baseUrl + "deleteEvents", content);
                    if (response.IsSuccessStatusCode)
                    {
                        var content1 = await response.Content.ReadAsStringAsync();
                        var res = JsonConvert.DeserializeObject<string>(content1);
                        await DisplayAlert("Message", res.ToString(), "OK");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());

                    await DisplayAlert(" nWorksLeaveApp", "Unable to connect server, Try again!", "OK");
                }
                var Answer1 = await DisplayAlert(" nWorksLeaveApp", "Do you want to refresh page?", "Yes", "No");
                if (Answer1 == true)
                {
                    await this.Navigation.PopAsync();
                    await this.Navigation.PushAsync(new emp_EventCalendar());
                }
            }
        }
    }
}