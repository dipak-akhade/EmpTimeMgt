using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Collections.ObjectModel;

using Xamarin.Forms.Xaml;
using nWorksLeaveApp.Common;

namespace nWorksLeaveApp.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class info_ofdayPage : ContentPage
    {
        ObservableCollection<inTimes> ListOf_IN_times = new ObservableCollection<inTimes>();
        ObservableCollection<outTimes> ListOf_OUT_times = new ObservableCollection<outTimes>();

        public info_ofdayPage(inoutDetailsPerDate Results, string totalInTime, string Ename, string date, string weekday)
        {
            InitializeComponent();

            this.BackgroundColor = ColorResources.PageBackgroundColor;

            getData(Results, totalInTime, Ename, date, weekday);
        }
        public void getData(inoutDetailsPerDate Results, string totalInTime, string ename, string date, string weekday)
        {

            foreach (inTimes time in Results.inTimes)
            {
                ListOf_IN_times.Add(time);
            }
            foreach (outTimes time in Results.outTimes)
            {
                ListOf_OUT_times.Add(time);
            }
            ListView_inTime.ItemsSource = ListOf_IN_times;
            ListView_outTime.ItemsSource = ListOf_OUT_times;

            Label_totalInTime.Text = "Total In Time " + totalInTime;
            Label_Details.Text = ename + ": " + date + " " + weekday;
            Label_Details.FontAttributes = FontAttributes.Bold;
            Label_totalInTime.FontAttributes = FontAttributes.Italic;
        }
        public void ListView_inTimeTapped(object sender, ItemTappedEventArgs e)
        {
            if (e == null) return; // has been set to null, do not 'process' tapped event
            ((ListView)sender).SelectedItem = null; // de-select the row
            var selection = e.Item as inTimes;
            DisplayAlert(" nWorksLeaveApp", "Device Id :" + selection.deviceid + " and corresponding Location is : " + selection.location, "OK");

        }
        public void ListView_outTimeTapped(object sender, ItemTappedEventArgs e)
        {
            if (e == null) return; // has been set to null, do not 'process' tapped event
            ((ListView)sender).SelectedItem = null; // de-select the row
            var selection = e.Item as outTimes;
            DisplayAlert(" nWorksLeaveApp", "Device Id :" + selection.deviceid + " and corresponding Location is : " + selection.location, "OK");

        }

    }
}

