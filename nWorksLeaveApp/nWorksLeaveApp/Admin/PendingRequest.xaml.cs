using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Forms.Xaml;
using nWorksLeaveApp.Common;

namespace nWorksLeaveApp.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PendingRequest : ContentPage
    {
        ObservableCollection<LeaveRequester> model = new ObservableCollection<LeaveRequester>();

        public PendingRequest()
        {
            InitializeComponent();
            BackgroundColor = ColorResources.PageBackgroundColor;
        }

        async public void listview_Notification_Tapped(object sender, ItemTappedEventArgs e)
        {
            if (e == null) return; // has been set to null, do not 'process' tapped event
            var selectedItem = e.Item as LeaveRequester;
            await this.Navigation.PushAsync(new RequestedLeavesOfUser(selectedItem.userId.ToString(), selectedItem.userFullName.ToString()));
            ((ListView)sender).SelectedItem = null; // de-select the row
        }

        async public void getLeaveRequesters()
        {
            MyActivityIndicator.IsVisible = true;
            MyActivityIndicator.IsRunning = true;
            try
            {
                HttpClient client = new HttpClient();
                string RestUrl = ColorResources.baseUrl + "getleaverequesters";
                var uri = new Uri(string.Format(RestUrl, string.Empty));
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var Items = JsonConvert.DeserializeObject<List<LeaveRequester>>(content);
                    for (int i = 0; i < Items.Count; i++)
                    {
                        model.Add(Items[i]);
                    }
                    listview_Notification.ItemsSource = model;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert(" nWorksLeaveApp", "Unable to connect server, Try again!", "OK");
                Debug.WriteLine(ex.ToString());
            }
            MyActivityIndicator.IsVisible = false;
            MyActivityIndicator.IsRunning = false;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            model.Clear();
            getLeaveRequesters();
        }
    }
}

