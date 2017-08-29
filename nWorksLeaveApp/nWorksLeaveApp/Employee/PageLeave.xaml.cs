using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Diagnostics;
using nWorksLeaveApp.Helpers;
using Xamarin.Forms.Xaml;
using nWorksLeaveApp.Common;

namespace nWorksLeaveApp.Employee
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageLeave : ContentPage
    {

        public PageLeave()
        {
            InitializeComponent();
            getData();

            var LabelTotalUsedLeaves_Tapped = new TapGestureRecognizer();
            LabelTotalUsedLeaves_Tapped.Tapped += (s, e) =>
            {
                this.Navigation.PushAsync(new LeavesTaken());
            };
            Label_Total_used_in_the_current_year.GestureRecognizers.Add(LabelTotalUsedLeaves_Tapped);

            var LabelTotalNotUsedLeaves_Tapped = new TapGestureRecognizer();
            LabelTotalNotUsedLeaves_Tapped.Tapped += (s, e) =>
            {
                this.Navigation.PushAsync(new LeavesUnUsed());
            };
            Label_Total_applied_for_but_not_used.GestureRecognizers.Add(LabelTotalNotUsedLeaves_Tapped);

            Label_Total_used_in_the_current_year.Effects.Add(new ShadowEffect
            {
                Radius = 15,
                Color = Color.Red,
                DistanceX = 15,
                DistanceY = 15
            });
        }

        public void btnRequestClicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new leaveRequestPage());
        }
        //		public void StackUsedLeavesClicked(object sender,EventArgs e)
        //		{
        //			this.Navigation.PushAsync (new LeavesTaken());
        //			DisplayAlert(" nWorksLeaveApp","Will show list of holidays taken","OK");
        //		}
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
                response = await Client.PostAsync(ColorResources.baseUrl + "usersLeaveStatus", content);
                Debug.WriteLine(response.StatusCode.ToString());
                if (response.IsSuccessStatusCode)
                {
                    var content1 = await response.Content.ReadAsStringAsync();
                    var Items = JsonConvert.DeserializeObject<userLeaveStatus>(content1);
                    Debug.WriteLine("Inside if");
                    Debug.WriteLine("A_Rolled_over_from_the_previous_year :" + Math.Round((float)Items.a_rolled_over_from_previous_year, 5).ToString());

                    A_Rolled_over_from_the_previous_year.Text = Math.Round((float)Items.a_rolled_over_from_previous_year, 5).ToString();
                    B_Earned_in_the_current_year.Text = Math.Round((float)Items.b_earned_in_current_year, 5).ToString();
                    C_Additional_earned_in_the_current_year.Text = Math.Round((float)Items.c_additional_earned_in_current_year, 5).ToString();
                    D_Total_in_the_current_year.Text = Math.Round((float)Items.d_total_in_current_year, 5).ToString();
                    E_Total_used_in_the_current_year.Text = Math.Round((float)Items.e_total_used_in_current_year, 5).ToString();
                    F_Total_applied_for_but_not_used.Text = Math.Round((float)Items.f_total_applied_but_notused_in_current_year, 5).ToString();
                    G_Total_leaves_without_pay_in_the_current_year.Text = Math.Round((float)Items.g_total_without_pay_in_current_year, 5).ToString();
                    H_Total_left_in_the_current_year.Text = Math.Round((float)Items.h_total_left_in_current_year, 5).ToString();
                    I_Total_left_in_the_current_year_to_month.Text = Math.Round((float)Items.j_total_left_in_current_year2Month, 5).ToString();
                    J_Total_left_in_the_year_to_date.Text = Math.Round((float)Items.i_total_left_in_current_year2date, 5).ToString();

                    //					await this.Navigation.PopModalAsync ();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert(" nWorksLeaveApp", "Internal Server Error! Please Contact Admin!", "OK");
                Debug.WriteLine(ex.ToString());
            }
        }
    }
}