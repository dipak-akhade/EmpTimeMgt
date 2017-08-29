using System;
using System.Collections.Generic;
using Xamarin.Forms;
using nWorksLeaveApp.Helpers;
using Plugin.Geolocator;
using Newtonsoft.Json;
using GeoCoordinatePortable;
using System.Net.Http;
using System.Diagnostics;
using Xamarin.Forms.Xaml;
using nWorksLeaveApp.Common;

namespace nWorksLeaveApp.Employee
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmpHome : ContentPage
    {
        double h, w;

        public EmpHome()
        {
            InitializeComponent();
            Title = "Welcome Mr./Mrs." + Settings.Username.ToUpper() + "!";

            BackgroundColor = ColorResources.PageBackgroundColor;
        }
        protected override void OnAppearing()
        {
            h = this.Height;
            w = (this.Width / 3) - 25;
            r1.Height = w;
            r2.Height = w;
            r3.Height = w;
            r4.Height = w;
            c1.Width = w;
            c2.Width = w;
            c3.Width = w;
        }
        async void comingInClicked(object sender, EventArgs e)
        {
            try
            {
                await this.Navigation.PushModalAsync(new Loading());
                //location attribute lat. long.
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 100;
                var position = await locator.GetPositionAsync(timeoutMilliseconds: 5000);

                //device id
                string UniqueDeviceID = DependencyService.Get<IGetSimNumber>().GetPlatformSimNumber();

                //Location Address
                string LocAddress = "";
                HttpClient client = new HttpClient();
                string RestUrl = "http://maps.googleapis.com/maps/api/geocode/json?latlng=" + position.Latitude + "," + position.Longitude + "&sensor=true[^]";
                var uri = new Uri(string.Format(RestUrl, string.Empty));
                var responses = await client.GetAsync(uri);
                if (responses.IsSuccessStatusCode)
                {
                    var contnt = await responses.Content.ReadAsStringAsync();
                    var Items = JsonConvert.DeserializeObject<RootObject>(contnt);
                    LocAddress = Items.results[0].formatted_address.ToString();
                }
                //calculating distance from Origin
                double originLatitude = 18.5956358, originLongitude = 73.7842351;
                var sCoord = new GeoCoordinate(originLatitude, originLongitude);
                var eCoord = new GeoCoordinate(position.Latitude, position.Longitude);

                //	navigate for scaning

                await this.Navigation.PushAsync(new ComingIn(position.Latitude.ToString(), position.Longitude.ToString(), UniqueDeviceID, LocAddress, sCoord.GetDistanceTo(eCoord).ToString()));
                await this.Navigation.PopModalAsync();

            }
            catch (Exception ex)
            {
                await DisplayAlert(" nWorksLeaveApp", "Unable to get Location or Device Id. Please Try again!", "OK");
                Debug.WriteLine(ex.ToString());
                await this.Navigation.PopModalAsync();
            }

        }
        async void goingOutClicked(object sender, EventArgs e)
        {
            try
            {
                await this.Navigation.PushModalAsync(new Loading());

                //location attribute lat. long.
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 100;
                var position = await locator.GetPositionAsync(timeoutMilliseconds: 5000);
                //device id
                string UniqueDeviceID = DependencyService.Get<IGetSimNumber>().GetPlatformSimNumber();
                //Location Address
                string LocAddress = "";
                HttpClient client = new HttpClient();
                string RestUrl = "http://maps.googleapis.com/maps/api/geocode/json?latlng=" + position.Latitude + "," + position.Longitude + "&sensor=true[^]";
                var uri = new Uri(string.Format(RestUrl, string.Empty));
                var responses = await client.GetAsync(uri);
                if (responses.IsSuccessStatusCode)
                {
                    var contnt = await responses.Content.ReadAsStringAsync();
                    var Items = JsonConvert.DeserializeObject<RootObject>(contnt);
                    LocAddress = Items.results[0].formatted_address.ToString();
                }

                //calculating distance from Origin
                double originLatitude = 18.5956358, originLongitude = 73.7842351;
                var sCoord = new GeoCoordinate(originLatitude, originLongitude);
                var eCoord = new GeoCoordinate(position.Latitude, position.Longitude);

                //	navigate for scaning
                await this.Navigation.PushAsync(new goingOut(position.Latitude.ToString(), position.Longitude.ToString(), UniqueDeviceID, LocAddress, sCoord.GetDistanceTo(eCoord).ToString()));

                await this.Navigation.PopModalAsync();

            }
            catch (Exception ex)
            {
                await DisplayAlert(" nWorksLeaveApp", "Unable to get Location or Device Id. Please Try again!", "OK");
                await this.Navigation.PopModalAsync();
                Debug.WriteLine(ex.ToString());

            }
        }
        //		void eventCalendarClicked(object sender, EventArgs e)
        //		{
        //			this.Navigation.PushAsync (new EventCalendar());
        //		}



        void attendanceClicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new LeavesAndHolidays());
            //			this.Navigation.PushAsync(new leaveRequestPage());
            //			DisplayAlert("Coming Soon, Thanks! ","Alert","OK");
        }
        //		void statusClicked(object sender, EventArgs e)
        //		{
        //			this.Navigation.PushAsync (new emp_Attendance());
        //		}
        async void btnLogout_Click(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Question", "Are you sure want to Logout?", "Yes", "No");
            if (answer == true)
            {
                Navigation.InsertPageBefore(new LoginPage(), this);
                await this.Navigation.PopToRootAsync();
                ColorResources.LIVE_USER_ID = "";
                ColorResources.LIVE_USER_TYPE = "";
                ColorResources.Already_In = false;
                ColorResources.Already_Out = false;
            }
        }
    }
}
