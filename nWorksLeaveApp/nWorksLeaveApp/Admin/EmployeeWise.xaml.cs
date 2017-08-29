using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Xamarin.Forms.Xaml;
using nWorksLeaveApp.Common;

namespace nWorksLeaveApp.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmployeeWise : ContentPage
    {
        ObservableCollection<EmployeeWiseData> MyData = new ObservableCollection<EmployeeWiseData>();
        int next = 0;
        public EmployeeWise(List<EmployeeWiseData> Results)
        {
            InitializeComponent();
            BackgroundColor = ColorResources.PageBackgroundColor;
            foreach (EmployeeWiseData obj in Results)
            {
                MyData.Add(obj);
            }
            getData();
        }
        public void btnPrev_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (next != 0)
                {
                    next--;
                    Debug.WriteLine("Btn Prev Clicked :" + next.ToString());
                    listof_employeewiseRecord.ItemsSource = MyData[next].employeeWiseData;
                    HeaderDate.Text = MyData[next].EmployeeName;
                    btnNext.IsEnabled = true;
                    btnNext.BackgroundColor = Color.Fuchsia;
                }
                else
                {
                    btnPrev.IsEnabled = false;
                    btnPrev.BackgroundColor = Color.Silver;
                }
            }
            catch (Exception ex)
            {
                DisplayAlert(" nWorksLeaveApp", ex.ToString(), "OK");
            }
        }
        public void btnNext_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (next != MyData.Count - 1)
                {
                    next++;
                    Debug.WriteLine("Btn Next Clicked :" + next.ToString());
                    HeaderDate.Text = MyData[next].EmployeeName;
                    listof_employeewiseRecord.ItemsSource = MyData[next].employeeWiseData;
                    btnPrev.IsEnabled = true;
                    btnPrev.BackgroundColor = Color.Fuchsia;
                }
                else
                {
                    btnNext.IsEnabled = false;
                    btnNext.BackgroundColor = Color.Silver;
                }
            }
            catch (Exception ex)
            {
                DisplayAlert(" nWorksLeaveApp", ex.ToString(), "OK");
            }
        }
        public void getData()
        {
            if (MyData.Count == 1)
            {
                btnPrev.IsEnabled = false;
                btnNext.IsEnabled = false;
                btnNext.BackgroundColor = Color.Silver;
                btnPrev.BackgroundColor = Color.Silver;
            }
            else
                btnPrev.IsEnabled = false;
            btnPrev.BackgroundColor = Color.Silver;
            listof_employeewiseRecord.ItemsSource = MyData[next].employeeWiseData;
            HeaderDate.Text = MyData[next].EmployeeName;
            Debug.WriteLine("On First Load next :" + next.ToString());
            Debug.WriteLine("Data Count" + MyData.Count.ToString());
        }
        async public void listof_employeewiseRecord_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            await this.Navigation.PushModalAsync(new Loading());

            try
            {
                if (e == null) return; // has been set to null, do not 'process' tapped event
                ((ListView)sender).SelectedItem = null; // de-select the row

                var selection = e.Item as EmployeeData;
                // Debug.WriteLine(selection.uid.ToString()+" "+ string.Format("{0:yyyy-MM-dd HH:mm:ss}",selection.date));
                get_inoutDetails obj = new get_inoutDetails();
                obj.choosedDate = string.Format("{0:yyyy-MM-dd HH:mm:ss}", selection.date);
                obj.userid = selection.uid.ToString();
                // Debug.WriteLine("Is device or location changed :"+selection.Is_Loc_Device_Changed.ToString());
                var json = JsonConvert.SerializeObject(obj);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpClient Client = new HttpClient();
                HttpResponseMessage response = null;
                response = await Client.PostAsync(ColorResources.baseUrl + "get_inoutdetailsperdate", content);
                if (response.IsSuccessStatusCode)
                {
                    // Debug.WriteLine(response.StatusCode.ToString());
                    var content1 = await response.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<inoutDetailsPerDate>(content1);
                    if (res.inTimes != null || res.outTimes != null)
                    {
                        await this.Navigation.PopModalAsync();

                        await this.Navigation.PushAsync(new info_ofdayPage(res, selection.totalInTime.ToString(), selection.uid, selection.date, selection.weekday));
                    }
                    else
                    {
                        await this.Navigation.PopModalAsync();

                        await DisplayAlert(" nWorksLeaveApp", "Invalid Selection!", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await this.Navigation.PopModalAsync();

                await DisplayAlert(" nWorksLeaveApp", "Unable to connect server, Try again!", "OK");
                Debug.WriteLine(ex.ToString());
            }
        }

    }
}
