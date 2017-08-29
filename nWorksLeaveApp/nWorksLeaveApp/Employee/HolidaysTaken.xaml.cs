using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Net.Http;
using Newtonsoft.Json;
using System.Diagnostics;
using nWorksLeaveApp.Helpers;

using Xamarin.Forms.Xaml;
using nWorksLeaveApp.Common;

namespace nWorksLeaveApp.Employee
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HolidaysTaken : ContentPage
    {

        public HolidaysTaken()
        {
            InitializeComponent();

            Title = "Mr./Mrs." + Settings.Username.ToUpper() + "!";


            if (Device.OS.ToString() == "iOS")
            {
                this.BackgroundColor = Color.Black;
            }
            else
            {
                this.BackgroundColor = Color.Transparent;
            }
            getStatus();
        }
        async public void getStatus()
        {
            await this.Navigation.PushModalAsync(new Loading());
            try
            {
                HttpClient client = new HttpClient();
                string RestUrl = ColorResources.baseUrl + "getleavesstatus?uid=" + ColorResources.LIVE_USER_ID;
                var uri = new Uri(string.Format(RestUrl, string.Empty));
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var Items = JsonConvert.DeserializeObject<LeavesStatus>(content);

                    holidaysTaken.ItemsSource = Items.holidays;

                    await this.Navigation.PopModalAsync();

                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert", "Unable to connect server, Try again!", "OK");
                Debug.WriteLine(ex.ToString());
            }

        }
    }
}