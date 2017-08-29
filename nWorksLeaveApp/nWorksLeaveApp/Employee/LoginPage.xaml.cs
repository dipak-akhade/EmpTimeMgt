using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;
using Xamarin.Forms;
using System.Text;
using Plugin.Connectivity;
using nWorksLeaveApp;
using nWorksLeaveApp.Helpers;
using System.Diagnostics;
using GeoCoordinatePortable;
using System.Threading.Tasks;
using Xamarin.Forms.Xaml;
using nWorksLeaveApp.Admin;
using nWorksLeaveApp.Common;

namespace nWorksLeaveApp.Employee
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        string isConnected;

        public LoginPage()
        {
            InitializeComponent();
            isConnected = CrossConnectivity.Current.IsConnected ? "Connected" : "No Connection";
            BackgroundColor = ColorResources.PageBackgroundColor;
            var linkForgotPassword_tapped = new TapGestureRecognizer();
            linkForgotPassword_tapped.Tapped += (s, e) =>
            {
                this.Navigation.PushAsync(new ForgotPasswordPage());
            };
            linkForgotPassword.GestureRecognizers.Add(linkForgotPassword_tapped);
            CheckForAutoLogin();
        }
        private void CheckForAutoLogin()
        {
            entryUsername.Text = Settings.Username;
            entryPassword.Text = Settings.password;
        }

        async public void btnLoginClicked(object sender, EventArgs e)
        {
          nWorksLeaveApp.Helpers.Settings.Username = entryUsername.Text;
          nWorksLeaveApp.Helpers.Settings.password = entryPassword.Text;

            btnLogin.IsEnabled = false;
            activityIndicator.IsRunning = true;
            activityIndicator.IsVisible = true;

            if (string.IsNullOrEmpty(entryPassword.Text) || string.IsNullOrEmpty(entryUsername.Text))
            {
                activityIndicator.IsRunning = false;
                activityIndicator.IsVisible = false;
                btnLogin.IsEnabled = true;
                await DisplayAlert(" nWorksLeaveApp", "Enter Username and Password!", "OK");
                entryUsername.Focus();
            }
            else
            {
                if (isConnected == "Connected")
                {
                    login obj = new login();
                    obj.username = entryUsername.Text;
                    obj.password = entryPassword.Text;
                    try
                    {
                        var json = JsonConvert.SerializeObject(obj);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");
                        HttpClient Client = new HttpClient();
                        HttpResponseMessage response = null;
                        response = await Client.PostAsync(ColorResources.baseUrl + "userlogin", content);
                        if (response.IsSuccessStatusCode)
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                Task.Delay(1000).Wait();

                                var content1 = await response.Content.ReadAsStringAsync();
                                var res = JsonConvert.DeserializeObject<userLogin>(content1);
                                if (res.serviceStatus == "Success" && res.userActive == "true")
                                {
                                    //Login Success
                                    if (res.type == "Master" || res.type == "Admin")
                                    {
                                        //Admin Account
                                        //await this.Navigation.PushAsync (new AdminHome ());
                                        Navigation.InsertPageBefore(new AdminHome(), this);
                                        await Navigation.PopAsync().ConfigureAwait(false);
                                        ColorResources.LIVE_USER_ID = res.userId;
                                        ColorResources.LIVE_USER_TYPE = res.type;
                                    }
                                    else if (res.type == "Employee")
                                    {
                                        //Employee Account
                                        //await this.Navigation.PushAsync (new EmpHome ());
                                        Navigation.InsertPageBefore(new EmpHome(), this);
                                        await Navigation.PopAsync().ConfigureAwait(false);
                                        ColorResources.LIVE_USER_ID = res.userId;
                                        ColorResources.LIVE_USER_TYPE = res.type;
                                    }
                                    else if (res.type == "Admin_Employee")
                                    {
                                        var Answer = await DisplayAlert(" nWorksLeaveApp", "Which Account You Want To Use?", "Admin", "Employee");
                                        if (Answer == true)
                                        {
                                            Navigation.InsertPageBefore(new AdminHome(), this);
                                            await Navigation.PopAsync().ConfigureAwait(false);
                                            ColorResources.LIVE_USER_ID = res.userId;
                                            ColorResources.LIVE_USER_TYPE = res.type;
                                        }
                                        else
                                        {
                                            Navigation.InsertPageBefore(new EmpHome(), this);
                                            await Navigation.PopAsync().ConfigureAwait(false);
                                            ColorResources.LIVE_USER_ID = res.userId;
                                            ColorResources.LIVE_USER_TYPE = res.type;
                                        }
                                    }
                                }
                                else
                                {
                                    btnLogin.IsEnabled = true;
                                    activityIndicator.IsRunning = false;
                                    activityIndicator.IsVisible = false;
                                    await DisplayAlert(" nWorksLeaveApp", "Incorrect Username/Password!", "OK");
                                }
                            });
                        }
                        btnLogin.IsEnabled = true;
                    }
                    catch (Exception ex)
                    {
                        btnLogin.IsEnabled = true;
                        activityIndicator.IsRunning = false;
                        activityIndicator.IsVisible = false;
                        await DisplayAlert(" nWorksLeaveApp", "Unable to connect Server, Try again!", "OK");
                        Debug.WriteLine(ex.ToString());
                    }
                }
                else
                {
                    btnLogin.IsEnabled = true;
                    activityIndicator.IsRunning = false;
                    activityIndicator.IsVisible = false;
                    await DisplayAlert(" nWorksLeaveApp", "No Internet Connection, Try Again!", "OK");
                }
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //			var networkConnection = DependencyService.Get<INetworkConnection>();
            //			networkConnection.CheckNetworkConnection();
            //			var networkStatus = networkConnection.IsConnected ? "Connected" : "Not Connected";

            //			DisplayAlert (" nWorksLeaveApp",networkStatus.ToString(),"OK");
            CrossConnectivity.Current.ConnectivityChanged += (sender, args) =>
            {
                //your implementation
                this.DisplayAlert("Connectivity Changed", "IsConnected: " + args.IsConnected.ToString(), "OK");
            };
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
    }
    public class login
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}

