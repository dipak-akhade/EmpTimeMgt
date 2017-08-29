using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plugin.Connectivity;

using Xamarin.Forms;
using nWorksLeaveApp.Employee;

namespace nWorksLeaveApp
{
    public partial class App : Application
    {
        static NavigationPage _NavPage;
        string isConnected;
        public App()
        {
            //InitializeComponent();

            //MainPage = new nWorksLeaveApp.MainPage();
            isConnected = CrossConnectivity.Current.IsConnected ? "Connected" : "No Connection";
            var loginpage = new LoginPage();
            _NavPage = new NavigationPage(loginpage);
            _NavPage.BarTextColor = Color.FromHex("#ffffff");
            _NavPage.BarBackgroundColor = Color.FromHex("#000000");
            MainPage = _NavPage;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
