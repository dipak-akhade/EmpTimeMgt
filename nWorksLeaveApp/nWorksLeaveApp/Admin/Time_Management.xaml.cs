using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Diagnostics;
using Xamarin.Forms.Xaml;
using nWorksLeaveApp.Common;

namespace nWorksLeaveApp.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Time_Management : ContentPage
    {
        ObservableCollection<empList_Model> generateEmpList = new ObservableCollection<empList_Model>();

        ObservableCollection<empList_Model> EmpList = new ObservableCollection<empList_Model>();

        ObservableCollection<empList_Model> tempEmpList = new ObservableCollection<empList_Model>();

        ObservableCollection<empList_Model> tempGenerateEmpList = new ObservableCollection<empList_Model>();


        getEmployeewiseRecord obj;

        public Time_Management()
        {
            InitializeComponent();
            greaterThan1.Text = ">";
            greaterThan2.Text = ">>";
            lessThan1.Text = "<";
            lessThan2.Text = "<<";

            BackgroundColor = ColorResources.PageBackgroundColor;

            Title = "Time Management";

            refresh_EmpList();

            listof_Employees.ItemsSource = EmpList;
        }


        async public void DatewiseClicked(object sender, EventArgs e)
        {
            await this.Navigation.PushModalAsync(new Loading());
            obj = new getEmployeewiseRecord();
            obj.uid = new List<string>();
            string fd = fromDatePicker.Date.ToString("yyyy-MM-dd HH:mm:ss");
            string td = toDatePicker.Date.ToString("yyyy-MM-dd HH:mm:ss");

            Debug.WriteLine("from date fd :" + fd);
            Debug.WriteLine("to date td :" + td);

            obj.fromdate = Convert.ToDateTime(fd);
            obj.todate = Convert.ToDateTime(td);

            foreach (empList_Model user in generateEmpList)
            {
                obj.uid.Add(user.id.ToString());
            }

            try
            {
                if (obj.uid.Count != 0)
                {
                    Debug.WriteLine(obj.fromdate.ToString());

                    var json = JsonConvert.SerializeObject(obj);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpClient Client = new HttpClient();
                    HttpResponseMessage response = null;
                    response = await Client.PostAsync(ColorResources.baseUrl + "DatewiseRecord", content);
                    if (response.IsSuccessStatusCode)
                    {
                        var content1 = await response.Content.ReadAsStringAsync();
                        var res = JsonConvert.DeserializeObject<List<DateWiseData>>(content1);
                        await this.Navigation.PopModalAsync();

                        //						await this.Navigation.PushAsync(new detail_infoDatewisePage(res));
                        await this.Navigation.PushAsync(new Datewise(res));
                        res.Clear();
                    }
                }
                else
                {
                    await DisplayAlert(" nWorksLeaveApp", "Select employee/s to generate report!", "OK");
                    await this.Navigation.PopModalAsync();

                }
            }
            catch (Exception ex)
            {
                await DisplayAlert(" nWorksLeaveApp", "Unable to connect server, Try again!", "OK");
                Debug.WriteLine(ex.ToString());
                await this.Navigation.PopModalAsync();

            }
            //clear everything
            Debug.WriteLine("pop called");
            refresh_EmpList();
            generateEmpList.Clear();
            tempEmpList.Clear();
        }
        async public void EmployeewiseClicked(object sender, EventArgs e)
        {
            await this.Navigation.PushModalAsync(new Loading());

            obj = new getEmployeewiseRecord();
            obj.uid = new List<string>();

            string fd = fromDatePicker.Date.ToString("yyyy-MM-dd");
            string td = toDatePicker.Date.ToString("yyyy-MM-dd");

            Debug.WriteLine("from date fd :" + fd);
            Debug.WriteLine("to date td :" + td);

            obj.fromdate = Convert.ToDateTime(fd);
            obj.todate = Convert.ToDateTime(td);
            foreach (empList_Model user in generateEmpList)
            {
                obj.uid.Add(user.id.ToString());
            }

            try
            {
                if (obj.uid.Count != 0)
                {
                    var json = JsonConvert.SerializeObject(obj);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpClient Client = new HttpClient();
                    HttpResponseMessage response = null;
                    response = await Client.PostAsync(ColorResources.baseUrl + "employeewiserecord", content);
                    if (response.IsSuccessStatusCode)
                    {
                        var content1 = await response.Content.ReadAsStringAsync();
                        var res = JsonConvert.DeserializeObject<List<EmployeeWiseData>>(content1);
                        await this.Navigation.PopModalAsync();
                        //						await this.Navigation.PushAsync(new detail_infoPage(res));
                        await this.Navigation.PushAsync(new EmployeeWise(res));
                        res.Clear();
                    }
                }
                else
                {
                    await DisplayAlert(" nWorksLeaveApp", "Select employee/s to generate report!", "OK");
                    await this.Navigation.PopModalAsync();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert(" nWorksLeaveApp", "Unable to connect server, Try again!", "OK");
                Debug.WriteLine(ex.ToString());
                await this.Navigation.PopModalAsync();

            }

            //clear everything
            refresh_EmpList();
            generateEmpList.Clear();
            tempEmpList.Clear();
        }
        public void DateSelected_fromDatePicker(object sender, EventArgs e)
        {
            if (fromDatePicker.Date > DateTime.Today || fromDatePicker.Date > toDatePicker.Date)
            {
                DisplayAlert(" nWorksLeaveApp", "Invalid Date Selection!", "OK");
                fromDatePicker.Date = DateTime.Today;
            }
        }
        public void DateSelected_toDatePicker(object sender, EventArgs e)
        {
            if (toDatePicker.Date > DateTime.Today || toDatePicker.Date < fromDatePicker.Date)
            {
                DisplayAlert(" nWorksLeaveApp", "Invalid Date Selection!", "OK");
                toDatePicker.Date = DateTime.Today;
            }
        }

        async public void refresh_EmpList()
        {
            EmpList.Clear();
            try
            {
                HttpClient client = new HttpClient();
                string RestUrl = ColorResources.baseUrl + "getemployeelist";
                var uri = new Uri(string.Format(RestUrl, string.Empty));
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var Items = JsonConvert.DeserializeObject<List<empList_Model>>(content);
                    foreach (empList_Model emp in Items)
                    {
                        EmpList.Add(emp);
                    }
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert(" nWorksLeaveApp", "Unable to connect server, Try again!", "OK");
                Debug.WriteLine(ex.ToString());
                await this.Navigation.PopModalAsync();

            }
        }
        public void greaterThan1Clicked(object sender, EventArgs e)
        {
            //selected Employees should added in Gererate report list
            foreach (empList_Model emp in tempEmpList)
            {
                generateEmpList.Add(emp);
                tempGenerateEmpList.Add(emp);
                EmpList.Remove(emp);
            }
            listof_generateReportForEmployees.ItemsSource = generateEmpList;
            listof_Employees.ItemsSource = EmpList;
            tempEmpList.Clear();
        }
        public void greaterThan2Clicked(object sender, EventArgs e)
        {
            //All Employees should be added in generate report list
            foreach (empList_Model emp in EmpList)
            {
                emp.check = true;
                generateEmpList.Add(emp);
                tempGenerateEmpList.Add(emp);
            }
            EmpList.Clear();
            listof_generateReportForEmployees.ItemsSource = generateEmpList;
            listof_Employees.ItemsSource = EmpList;
        }

        public void lessThan1Clicked(object sender, EventArgs e)
        {
            //Selected Employees should be added in Employee list

            foreach (empList_Model emp in tempGenerateEmpList)
            {
                emp.check = false;
                generateEmpList.Remove(emp);
                EmpList.Add(emp);
            }
            listof_generateReportForEmployees.ItemsSource = generateEmpList;
            listof_Employees.ItemsSource = EmpList;
            //			tempEmpList.Clear ();
            tempGenerateEmpList.Clear();
        }
        public void lessThan2Clicked(object sender, EventArgs e)
        {
            //All Employees should be added back to Employee list 
            foreach (empList_Model emp in generateEmpList)
            {
                emp.check = false;
                EmpList.Add(emp);
            }
            generateEmpList.Clear();
            listof_generateReportForEmployees.ItemsSource = generateEmpList;
            listof_Employees.ItemsSource = EmpList;
            tempEmpList.Clear();
            tempGenerateEmpList.Clear();

        }
        public void onSelectClicked(object sender, EventArgs e)
        {
            var btn = sender as Image;
            var employeeCheck = btn.BindingContext as empList_Model;

            if (employeeCheck.check == true)
            {
                tempEmpList.Remove(employeeCheck);
                btn.Source = "unChecked.png";
                employeeCheck.check = false;
            }
            else
            {
                tempEmpList.Add(employeeCheck);
                btn.Source = "checked.png";
                employeeCheck.check = true;
            }
        }

        //
        public void onRemoveClicked(object sender, EventArgs e)
        {
            var btn = sender as Image;
            var employeeCheck = btn.BindingContext as empList_Model;

            if (employeeCheck.check == true)
            {
                tempEmpList.Remove(employeeCheck);
                btn.Source = "unChecked.png";
                employeeCheck.check = false;
            }
            else
            {
                tempEmpList.Add(employeeCheck);
                btn.Source = "checked.png";
                employeeCheck.check = true;
            }
        }
    }

    public class empList_Model
    {
        public string empName { get; set; }
        public Boolean check { get; set; }
        public string id { get; set; }
    }
    public class getEmployeewiseRecord
    {
        public List<string> uid { get; set; }
        public DateTime fromdate { get; set; }
        public DateTime todate { get; set; }
    }

}