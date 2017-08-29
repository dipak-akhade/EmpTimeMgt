using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Net.Http;
using Newtonsoft.Json;
using nWorksLeaveApp.Helpers;
using System.Diagnostics;

using Xamarin.Forms.Xaml;
using nWorksLeaveApp.Employee;
using nWorksLeaveApp.Common;

namespace nWorksLeaveApp.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminHome : ContentPage
    {
        double h, w;

        public AdminHome()
        {
            InitializeComponent();
            BackgroundColor = ColorResources.PageBackgroundColor;
            Title = "Welcome Mr./Mrs." + Settings.Username.ToUpper() + "!";

        }
        Int32 Notification = 0;
        protected override void OnAppearing()
        {
            base.OnAppearing();
            getNotificationNumber();

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
        async public void getNotificationNumber()
        {
            try
            {
                HttpClient client = new HttpClient();
                string RestUrl = ColorResources.baseUrl + "getnotification";
                var uri = new Uri(string.Format(RestUrl, string.Empty));
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var Items = JsonConvert.DeserializeObject<Int32>(content);
                    Notification = Items;
                }
                btnNotif.Text = Notification.ToString();
            }
            catch (Exception ex)
            {
                await DisplayAlert(" nWorksLeaveApp", "Unable to connect server, Try again!", "OK");
                Debug.WriteLine(ex.ToString());
            }
        }
        void onBoardClicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new OnBoardEmp());
        }

        void PendingRequestClicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new PendingRequest());
        }
        void empMgtClicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new empManagement());
            //			DisplayAlert("Coming Soon, Thanks! ","Alert","OK");
        }


        void eventCalendarClicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new emp_EventCalendar());
            //			DisplayAlert("Coming Soon, Thanks! ","Alert","OK");
        }


        void attendanceClicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new admin_Attendance());
            //			DisplayAlert("Coming Soon, Thanks! ","Alert","OK");
        }


        void admMgtClicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new adminManagement());
            //			DisplayAlert("Coming Soon, Thanks! ","Alert","OK");

        }


        void admRegClicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new adminRegistration());
            //			DisplayAlert("Coming Soon, Thanks! ","Alert","OK");
        }


        void timeManagementClicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new Time_Management());
            //			DisplayAlert("Coming Soon, Thanks! ","Alert","OK");
        }
        async void btnLogout_Click(object sender, EventArgs e)
        {
            var answer = await DisplayAlert(" nWorksLeaveApp", "Are you sure want to Logout?", "Yes", "No");
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
    public class getNumberOfNotifications
    {
        public int Notifications { get; set; }
    }
}