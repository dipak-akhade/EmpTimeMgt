using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Net.Http;
using Newtonsoft.Json;
using Xamarin.Forms.Xaml;
using nWorksLeaveApp.Common;

namespace nWorksLeaveApp.Employee
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EventCalendar : ContentPage
    {
        public EventCalendar()
        {
            InitializeComponent();
            Title = "Event Calendar";
            BackgroundColor = ColorResources.PageBackgroundColor;
            getEventCalendar();

        }
        async public void getEventCalendar()
        {
            await this.Navigation.PushModalAsync(new Loading());
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
                await this.Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert", "Unable to connect server, Try again!", "OK");
            }
        }
    }
}