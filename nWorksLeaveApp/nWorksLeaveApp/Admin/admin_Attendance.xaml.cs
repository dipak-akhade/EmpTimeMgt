using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Diagnostics;
using Xamarin.Forms.Xaml;
using nWorksLeaveApp.Common;

namespace nWorksLeaveApp.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class admin_Attendance : ContentPage
    {
        string uid = "";
        public admin_Attendance()
        {
            InitializeComponent();
            BackgroundColor = ColorResources.PageBackgroundColor;
            getEmployees();
        }
        async public void holidaysTaken_Tapped(object sender, ItemTappedEventArgs e)
        {
            if (e == null)
                return;
            var selectedItem = e.Item as Holidays;
            if (Convert.ToDateTime(selectedItem.date) >= DateTime.Today)
            {
                var Ans = await DisplayAlert(" nWorksLeaveApp", "Are you sure want to cancel holiday applied on date " + selectedItem.date.ToString(), "Yes", "No");
                if (Ans == true)
                {
                    await this.Navigation.PushModalAsync(new Loading());

                    CancelReqLeaves obj = new CancelReqLeaves();
                    obj.uid = this.uid;
                    obj.dateToBeCancel = Convert.ToDateTime(selectedItem.date);

                    try
                    {
                        var json = JsonConvert.SerializeObject(obj);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");
                        HttpClient Client = new HttpClient();
                        HttpResponseMessage response = null;
                        response = await Client.PostAsync(ColorResources.baseUrl + "CancelReqLeave", content);
                        if (response.IsSuccessStatusCode)
                        {
                            var content1 = await response.Content.ReadAsStringAsync();
                            var res = JsonConvert.DeserializeObject<string>(content1);
                            //refresh list
                            getStatus();
                            await this.Navigation.PopModalAsync();
                            await DisplayAlert(" nWorksLeaveApp", res.ToString(), "OK");

                        }
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert(" nWorksLeaveApp", "Unable to connect server, Try again!", "OK");
                        Debug.WriteLine(ex.ToString());

                    }

                }
            }
            ((ListView)sender).SelectedItem = null;
        }
        async public void leavesTaken_Tapped(object sender, ItemTappedEventArgs e)
        {
            if (e == null)
                return;
            var selectedItem = e.Item as Leaves;
            if (Convert.ToDateTime(selectedItem.date) >= DateTime.Today)
            {
                var Ans = await DisplayAlert(" nWorksLeaveApp", "Are you sure want to cancel leave applied on date " + selectedItem.date.ToString(), "Yes", "No");
                if (Ans == true)
                {
                    await this.Navigation.PushModalAsync(new Loading());

                    CancelReqLeaves obj = new CancelReqLeaves();
                    obj.uid = this.uid;
                    obj.dateToBeCancel = Convert.ToDateTime(selectedItem.date);

                    try
                    {
                        var json = JsonConvert.SerializeObject(obj);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");
                        HttpClient Client = new HttpClient();
                        HttpResponseMessage response = null;
                        response = await Client.PostAsync(ColorResources.baseUrl + "CancelReqLeave", content);
                        if (response.IsSuccessStatusCode)
                        {
                            var content1 = await response.Content.ReadAsStringAsync();
                            var res = JsonConvert.DeserializeObject<string>(content1);
                            //refresh list
                            getStatus();
                            await this.Navigation.PopModalAsync();
                            await DisplayAlert(" nWorksLeaveApp", res.ToString(), "OK");

                        }
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert(" nWorksLeaveApp", "Unable to connect server, Try again!", "OK");
                        Debug.WriteLine(ex.ToString());

                    }

                }
            }
            ((ListView)sender).SelectedItem = null;
        }
        async public void getEmployees()
        {
            await this.Navigation.PushModalAsync(new Loading());

            try
            {
                HttpClient client = new HttpClient();
                string RestUrl = ColorResources.baseUrl + "getlistofemployees";
                var uri = new Uri(string.Format(RestUrl, string.Empty));
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var Items = JsonConvert.DeserializeObject<ModelGetEmployeeList>(content);
                    pickerEmployee.Items.Add("Select");
                    foreach (EmployeeList emp in Items.EmpList)
                    {
                        pickerEmployee.Items.Add(emp.uid.ToString() + ":" + emp.fname.ToString() + " " + emp.lname.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert(" nWorksLeaveApp", "Unable to connect server, Try again!", "OK");
                Debug.WriteLine(ex.ToString());
            }
            await this.Navigation.PopModalAsync();
        }
        async public void getStatus()
        {
            try
            {
                await this.Navigation.PushModalAsync(new Loading());

                HttpClient client = new HttpClient();
                string RestUrl = ColorResources.baseUrl + "getleavesstatus?uid=" + uid;
                var uri = new Uri(string.Format(RestUrl, string.Empty));
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var Items = JsonConvert.DeserializeObject<LeavesStatus>(content);
                    leavesTaken.ItemsSource = Items.leaves;
                    holidaysTaken.ItemsSource = Items.holidays;

                    int TLeaves = Items.totalLeaves;
                    int THolidays = Items.totalHolidays;
                    float BLeaves = Items.balanceLeaves;
                    float BHolidays = Items.balanceHolidays;

                    totalLeaves.Text = TLeaves.ToString();
                    totalHolidays.Text = THolidays.ToString();
                    balanceLeaves.Text = BLeaves.ToString();
                    balanceHolidays.Text = BHolidays.ToString();
                }
                await this.Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert(" nWorksLeaveApp", "Unable to connect server, Try again!", "OK");
                Debug.WriteLine(ex.ToString());

            }
        }
        public void PickerEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pickerEmployee.SelectedIndex == -1)
            {
                DisplayAlert(" nWorksLeaveApp", "Invalid Selection!", "OK");
            }
            else
            {
                string selection = (pickerEmployee.Items[pickerEmployee.SelectedIndex]);
                int len = selection.IndexOf(":");//getting length of string appears before "-" 

                if (len > 0)
                {
                    uid = selection.Substring(0, len);//get 0 to len string
                }
                if (selection.ToString() == "Select")
                {
                    clearAllFields();
                }
                else
                {
                    getStatus();
                }
            }
        }
        public void clearAllFields()
        {
            totalLeaves.Text = "";
            totalHolidays.Text = "";
            balanceLeaves.Text = "";
            balanceHolidays.Text = "";
        }
    }
}