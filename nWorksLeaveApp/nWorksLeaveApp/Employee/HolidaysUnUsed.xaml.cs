using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Newtonsoft.Json;
using System.Net.Http;
using System.Diagnostics;
using Xamarin.Forms.Xaml;
using nWorksLeaveApp.Common;

namespace nWorksLeaveApp.Employee
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HolidaysUnUsed : ContentPage
    {
        public HolidaysUnUsed()
        {
            InitializeComponent();
            Title = "Unused Holidays";
            getStatus();
            if (Device.OS.ToString() == "iOS")
            {
                this.BackgroundColor = Color.Black;
            }
            else
            {
                this.BackgroundColor = Color.Transparent;
            }
        }
        async public void getStatus()
        {
            await this.Navigation.PushModalAsync(new Loading());
            try
            {
                HttpClient client = new HttpClient();
                string RestUrl = ColorResources.baseUrl + "UnUsedLeaves?uid=" + ColorResources.LIVE_USER_ID;
                var uri = new Uri(string.Format(RestUrl, string.Empty));
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var Items = JsonConvert.DeserializeObject<AppliedButNotUsed>(content);
                    if (Items.holidays.Count == 0)
                        await DisplayAlert(" nWorksLeaveApp", "No Unused Leaves!", "OK");
                    Listview_holidaysTakenbutNotUsed.ItemsSource = Items.holidays;

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