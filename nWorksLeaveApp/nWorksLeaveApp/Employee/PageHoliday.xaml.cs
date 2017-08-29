using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using System.Net.Http;
using nWorksLeaveApp.Helpers;

using Xamarin.Forms.Xaml;
using nWorksLeaveApp.Common;

namespace nWorksLeaveApp.Employee
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageHoliday : ContentPage
    {

        public PageHoliday()
        {
            InitializeComponent();

            var LabelTotalUsedHolidays_Tapped = new TapGestureRecognizer();
            LabelTotalUsedHolidays_Tapped.Tapped += (s, e) =>
            {
                this.Navigation.PushAsync(new HolidaysTaken());
            };
            Total_used_in_the_current_year.GestureRecognizers.Add(LabelTotalUsedHolidays_Tapped);

            var LabelTotal_Not_UsedHolidays_Tapped = new TapGestureRecognizer();
            LabelTotal_Not_UsedHolidays_Tapped.Tapped += (s, e) =>
            {
                this.Navigation.PushAsync(new HolidaysUnUsed());
            };
            Total_applied_but_notused_in_the_current_year.GestureRecognizers.Add(LabelTotal_Not_UsedHolidays_Tapped);
            getData();
        }
        public void btnRequestClicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new leaveRequestPage());
        }
        //		public void StackUsedLeavesClicked(object sender,EventArgs e)
        //		{
        //			this.Navigation.PushAsync (new HolidaysTaken());
        ////			DisplayAlert(" nWorksLeaveApp","Will show list of holidays taken","OK");
        //		}
        public void btnCalendarClicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new EventCalendar());
            //			DisplayAlert(" nWorksLeaveApp","Will show list of holidays taken","OK");
        }
        async public void getData()
        {
            passUid user = new passUid();
            user.uid = ColorResources.LIVE_USER_ID;
            //			await this.Navigation.PushModalAsync (new Loading());
            try
            {
                var json = JsonConvert.SerializeObject(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpClient Client = new HttpClient();
                HttpResponseMessage response = null;
                response = await Client.PostAsync(ColorResources.baseUrl + "usersHolidayStatus", content);
                Debug.WriteLine(response.StatusCode.ToString());
                if (response.IsSuccessStatusCode)
                {
                    var content1 = await response.Content.ReadAsStringAsync();
                    var Items = JsonConvert.DeserializeObject<userHolidayStatus>(content1);
                    Debug.WriteLine("Inside if");

                    A.Text = Math.Round((float)Items.a_earned_in_current_year, 5).ToString();
                    B.Text = Math.Round((float)Items.b_total_used_in_current_year, 5).ToString();
                    C.Text = Math.Round((float)Items.c_total_applied_but_notused_in_current_year, 5).ToString();
                    D.Text = Math.Round((float)Items.d_total_left_in_current_year, 5).ToString();
                    E.Text = Math.Round((float)Items.e_total_left_in_current_year2date, 5).ToString();
                    F.Text = Math.Round((float)Items.f_total_left_in_current_year2Month, 5).ToString();

                    //					await this.Navigation.PopModalAsync ();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert(" nWorksLeaveApp", ex.ToString(), "OK");
            }
        }
    }
}

