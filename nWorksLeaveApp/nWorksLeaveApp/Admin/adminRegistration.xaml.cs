using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Net.Http;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System.Text.RegularExpressions;
using System.Text;
using System.Diagnostics;
using Xamarin.Forms.Xaml;
using nWorksLeaveApp.Models;
using nWorksLeaveApp.Common;

namespace nWorksLeaveApp.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class adminRegistration : ContentPage
    {

        int maleCheck = 1, femaleCheck = 0;
        string uid = "";
        string isConnected;

        public adminRegistration()
        {
            InitializeComponent();
            BackgroundColor = ColorResources.PageBackgroundColor;
            isConnected = CrossConnectivity.Current.IsConnected ? "Connected" : "No Connection";
            getEmployees();
            dateOfAnniversary.Date = DateTime.MinValue;
            dateOfHire.Date = DateTime.MinValue;
            dateOfBirth.Date = DateTime.MinValue;
        }
        void Register_Click(object sender, EventArgs e)
        {
            if (pickerEmployee.SelectedIndex == -1)
            {
                if (string.IsNullOrEmpty(empNo.Text) || string.IsNullOrEmpty(empFname.Text) || string.IsNullOrEmpty(empMname.Text) || string.IsNullOrEmpty(empLname.Text) || string.IsNullOrEmpty(username.Text) || string.IsNullOrEmpty(password.Text) || string.IsNullOrEmpty(confirmPassword.Text) || string.IsNullOrEmpty(desg.Text) || string.IsNullOrEmpty(address1.Text) || string.IsNullOrEmpty(city.Text) || string.IsNullOrEmpty(state.Text) || string.IsNullOrEmpty(country.Text))
                {
                    DisplayAlert(" nWorksLeaveApp", "Are you missing some fields?", "OK");
                }
                //Admin Registration
                RegisterNewAdmin();
            }
            else
            {
                if (string.IsNullOrEmpty(empNo.Text) || string.IsNullOrEmpty(empFname.Text) || string.IsNullOrEmpty(empMname.Text) || string.IsNullOrEmpty(empLname.Text) || string.IsNullOrEmpty(username.Text) || string.IsNullOrEmpty(desg.Text) || string.IsNullOrEmpty(address1.Text) || string.IsNullOrEmpty(city.Text) || string.IsNullOrEmpty(state.Text) || string.IsNullOrEmpty(country.Text))
                {
                    DisplayAlert(" nWorksLeaveApp", "Are you missing some fields?", "OK");
                }
                //Employee to Admin_Employee
                MakeEmplpoyeeToEmp_Admin();
            }
        }

        public void RegisterNewAdmin()
        {
            if (string.IsNullOrEmpty(empNo.Text) || string.IsNullOrEmpty(empFname.Text) || string.IsNullOrEmpty(empMname.Text) || string.IsNullOrEmpty(empLname.Text) || string.IsNullOrEmpty(username.Text) || string.IsNullOrEmpty(desg.Text) || string.IsNullOrEmpty(address1.Text) || string.IsNullOrEmpty(city.Text) || string.IsNullOrEmpty(state.Text) || string.IsNullOrEmpty(country.Text) || string.IsNullOrEmpty(password.Text) || string.IsNullOrEmpty(confirmPassword.Text) || string.IsNullOrEmpty(mobileNumber.Text) || string.IsNullOrEmpty(emailAddress.Text))
            {
                DisplayAlert(" nWorksLeaveApp", "Are you missing some fields?", "OK");
            }
            else
            {
                register();
            }

        }
        public void MakeEmplpoyeeToEmp_Admin()
        {
            if (string.IsNullOrEmpty(empNo.Text) || string.IsNullOrEmpty(empFname.Text) || string.IsNullOrEmpty(empMname.Text) || string.IsNullOrEmpty(empLname.Text) || string.IsNullOrEmpty(username.Text) || string.IsNullOrEmpty(desg.Text) || string.IsNullOrEmpty(address1.Text) || string.IsNullOrEmpty(city.Text) || string.IsNullOrEmpty(state.Text) || string.IsNullOrEmpty(country.Text) || string.IsNullOrEmpty(mobileNumber.Text) || string.IsNullOrEmpty(emailAddress.Text))
            {
                DisplayAlert(" nWorksLeaveApp", "Are you missing some fields?", "OK");
            }
            else
            {
                promot();
            }
        }
        async public void register()
        {
            if (isConnected == "Connected")
            {
                if (validateMobileNumber(mobileNumber.Text) == true)
                {
                    if (validatePincode(pinCode.Text) == true)
                    {
                        if (validateEmail(emailAddress.Text) == true)
                        {
                            if (password.Text == confirmPassword.Text)
                            {
                                userRegistration obj = new userRegistration();
                                obj.uid = empNo.Text;
                                obj.fname = empFname.Text;
                                obj.mname = empMname.Text;
                                obj.lname = empLname.Text;
                                obj.dob = dateOfBirth.Date.ToString("yyyy-MM-dd");
                                obj.hire_date = dateOfHire.Date.ToString("yyyy-MM-dd");
                                obj.username = username.Text;
                                obj.password = password.Text;
                                obj.addressLine1 = address1.Text;
                                obj.addressLine2 = address2.Text;
                                obj.state = state.Text;
                                obj.city = city.Text;
                                obj.pincode = int.Parse(pinCode.Text);
                                obj.country = country.Text;
                                obj.mobileNo = mobileNumber.Text;
                                obj._type = "Admin";
                                obj.userActive = "true";
                                obj.leaveDate = "0000-00-00";
                                obj.anniversary = dateOfAnniversary.Date.ToString("yyyy-MM-dd");
                                obj.desg = desg.Text;
                                obj.inout_status = "Outside";
                                obj.email = emailAddress.Text;
                                if (maleCheck == 1 && femaleCheck == 0)
                                    obj.gender = "Male";
                                else
                                    obj.gender = "Female";
                                try
                                {
                                    var json = JsonConvert.SerializeObject(obj);
                                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                                    HttpClient Client = new HttpClient();
                                    HttpResponseMessage response = null;
                                    response = await Client.PostAsync(ColorResources.baseUrl + "adminRegistration", content);
                                    if (response.IsSuccessStatusCode)
                                    {
                                        var content1 = await response.Content.ReadAsStringAsync();
                                        await DisplayAlert(" nWorksLeaveApp", content1.ToString(), "OK");
                                        await this.Navigation.PopAsync();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    await DisplayAlert(" nWorksLeaveApp", "Unable to connect server, Try again!", "OK");
                                    Debug.WriteLine(ex.ToString());
                                }
                            }
                            else
                            {
                                await DisplayAlert(" nWorksLeaveApp", "Password and Confirm Password does not matched!", "OK");
                            }
                        }
                        else
                        {
                            await DisplayAlert(" nWorksLeaveApp", "Email is invalid!", "OK");
                        }
                    }
                    else
                    {
                        await DisplayAlert(" nWorksLeaveApp", "Pincode is invalid!", "OK");
                        pinCode.Focus();
                    }
                }
                else
                {
                    await DisplayAlert(" nWorksLeaveApp", "Mobile Number is invalid!", "OK");
                    mobileNumber.Focus();
                }
            }
            else
            {
                await DisplayAlert(" nWorksLeaveApp", "No Internet Connection, Try Again!", "OK");
            }

        }
        async public void promot()
        {
            if (isConnected == "Connected")
            {
                if (validateMobileNumber(mobileNumber.Text) == true)
                {
                    if (validatePincode(pinCode.Text) == true)
                    {
                        if (validateEmail(emailAddress.Text) == true)
                        {
                            if (password.Text == confirmPassword.Text)
                            {
                                userUpdation obj = new userUpdation();
                                obj.uid = empNo.Text;
                                obj.fname = empFname.Text;
                                obj.mname = empMname.Text;
                                obj.lname = empLname.Text;
                                obj.dob = dateOfBirth.Date.ToString("yyyy-MM-dd");
                                obj.hire_date = dateOfHire.Date.ToString("yyyy-MM-dd");
                                obj.username = username.Text;
                                obj.addressLine1 = address1.Text;
                                obj.addressLine2 = address2.Text;
                                obj.state = state.Text;
                                obj.city = city.Text;
                                obj.pincode = int.Parse(pinCode.Text);
                                obj.country = country.Text;
                                obj.mobileNo = mobileNumber.Text;
                                obj._type = "Admin_Employee";
                                obj.userActive = "true";
                                obj.leaveDate = "1900-01-01";
                                obj.anniversary = dateOfAnniversary.Date.ToString("yyyy-MM-dd");
                                obj.desg = desg.Text;
                                obj.email = emailAddress.Text;
                                if (maleCheck == 1 && femaleCheck == 0)
                                    obj.gender = "Male";
                                else
                                    obj.gender = "Female";
                                try
                                {
                                    var json = JsonConvert.SerializeObject(obj);
                                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                                    HttpClient Client = new HttpClient();
                                    HttpResponseMessage response = null;
                                    response = await Client.PostAsync(ColorResources.baseUrl + "EmployeeToAdminRegistration", content);
                                    if (response.IsSuccessStatusCode)
                                    {
                                        var content1 = await response.Content.ReadAsStringAsync();
                                        await DisplayAlert(" nWorksLeaveApp", content1.ToString(), "OK");
                                        await this.Navigation.PopAsync();
                                    }
                                    else
                                        await DisplayAlert(" nWorksLeaveApp", "Response is not Received!", "OK");
                                }
                                catch (Exception ex)
                                {
                                    await DisplayAlert(" nWorksLeaveApp", "Unable to connect server, Try again!", "OK");
                                    Debug.WriteLine(ex.ToString());
                                }
                            }
                            else
                            {
                                await DisplayAlert(" nWorksLeaveApp", "Password and Confirm Password does not matched!", "OK");
                            }
                        }
                        else
                        {
                            await DisplayAlert(" nWorksLeaveApp", "Email is invalid!", "OK");
                        }
                    }
                    else
                    {
                        await DisplayAlert(" nWorksLeaveApp", "Pincode is invalid!", "OK");
                        pinCode.Focus();
                    }
                }
                else
                {
                    await DisplayAlert(" nWorksLeaveApp", "Mobile Number is invalid!", "OK");
                    mobileNumber.Focus();
                }
            }
            else
            {
                await DisplayAlert(" nWorksLeaveApp", "No Internet Connection, Try Again!", "OK");
            }

        }

        void onMaleSelected(object sender, EventArgs e)
        {
            if (maleCheck == 1)
            {
                maleCheck = 0;
                femaleCheck = 1;
                imageFemale.Source = "radio_Check.png";
                imageMale.Source = "radio_uncheck.png";
            }
            else
            {
                maleCheck = 1;
                femaleCheck = 0;
                imageFemale.Source = "radio_uncheck.png";
                imageMale.Source = "radio_Check.png";
            }
        }
        void onFemaleSelected(object sender, EventArgs e)
        {
            if (maleCheck == 1)
            {
                maleCheck = 0;
                femaleCheck = 1;
                imageFemale.Source = "radio_Check.png";
                imageMale.Source = "radio_uncheck.png";
            }
            else
            {
                maleCheck = 1;
                femaleCheck = 0;
                imageFemale.Source = "radio_uncheck.png";
                imageMale.Source = "radio_Check.png";
            }
        }

        async public void getEmployees()
        {
            await this.Navigation.PushModalAsync(new Loading());

            try
            {
                HttpClient client = new HttpClient();
                string RestUrl = ColorResources.baseUrl + "emplist";
                var uri = new Uri(string.Format(RestUrl, string.Empty));
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var Items = JsonConvert.DeserializeObject<ModelGetEmployeeList>(content);
                    pickerEmployee.Items.Add("**New Registration**");
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

        public void Reset_Click(object sender, EventArgs e)
        {
            clearAllFields();
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
                int len = selection.IndexOf(":");

                if (len > 0)
                {
                    uid = selection.Substring(0, len);
                }
                if (selection.ToString() == "**New Registration**")
                {
                    clearAllFields();
                    password.IsEnabled = true;
                    confirmPassword.IsEnabled = true;
                }
                else
                {
                    gettingDetailsOfSelectedEmployee();
                    password.IsEnabled = false;
                    confirmPassword.IsEnabled = false;
                }

            }
        }

        async public void gettingDetailsOfSelectedEmployee()
        {
            await this.Navigation.PushModalAsync(new Loading());

            try
            {
                HttpClient client = new HttpClient();
                string RestUrl = ColorResources.baseUrl + "getemployeedetails?uid=" + uid;
                var uri = new Uri(string.Format(RestUrl, string.Empty));
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var Items = JsonConvert.DeserializeObject<EmployeeDetails>(content);

                    empNo.Text = Items.Emp.uid;
                    empFname.Text = Items.Emp.fname;
                    empMname.Text = Items.Emp.mname;
                    empLname.Text = Items.Emp.lname;

                    dateOfBirth.Date = Items.Emp.dob;
                    //							gender
                    if (Items.Emp.gender == "Male")
                    {
                        maleCheck = 1;
                        femaleCheck = 0;
                        imageFemale.Source = "radio_uncheck.png";
                        imageMale.Source = "radio_Check.png";
                    }
                    else
                    {
                        maleCheck = 0;
                        femaleCheck = 1;
                        imageFemale.Source = "radio_Check.png";
                        imageMale.Source = "radio_uncheck.png";
                    }

                    desg.Text = Items.Emp.desg;
                    dateOfHire.Date = Items.Emp.hire_date;
                    username.Text = Items.Emp.username;
                    address1.Text = Items.Emp.addressLine1;
                    address2.Text = Items.Emp.addressLine2;
                    state.Text = Items.Emp.state;
                    city.Text = Items.Emp.city;
                    pinCode.Text = Items.Emp.pincode;
                    country.Text = Items.Emp.country;
                    mobileNumber.Text = Items.Emp.mobileNo;
                    dateOfAnniversary.Date = Items.Emp.anniversary;
                    emailAddress.Text = Items.Emp.email;

                }
            }
            catch (Exception ex)
            {
                await DisplayAlert(" nWorksLeaveApp", "Unable to connect server, Try again!", "OK");
                Debug.WriteLine(ex.ToString());
            }
            await this.Navigation.PopModalAsync();

        }

        public bool validateMobileNumber(string mobNumber)
        {
            if (mobNumber != "")
            {
                Regex rx = new Regex(@"^\d{10}$");
                if (rx.IsMatch(mobNumber))
                {
                    return true;
                }
                else
                    return false;
            }
            else
            {

                return false;
            }
        }
        public bool validatePincode(string pincode)
        {
            if (pincode != "")
            {
                Regex rx = new Regex(@"^\d{6,}$");
                if (rx.IsMatch(pincode))
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }
        public bool validateEmail(string email)
        {
            if (email != "")
            {
                Regex rx = new Regex(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");
                if (rx.IsMatch(email))
                {
                    return true;
                }
                else
                    return false;
            }
            else
            {
                return false;
            }
        }


        public void clearAllFields()
        {
            empNo.Text = "";
            empFname.Text = "";
            empMname.Text = "";
            empLname.Text = "";
            username.Text = "";
            password.Text = "";
            confirmPassword.Text = "";
            desg.Text = "";
            address1.Text = "";
            address2.Text = "";
            city.Text = "";
            state.Text = "";
            country.Text = "";
            pinCode.Text = "";
            mobileNumber.Text = "";
            emailAddress.Text = "";
            dateOfAnniversary.Date = dateOfAnniversary.MinimumDate;
            dateOfHire.Date = dateOfHire.MinimumDate;
            dateOfBirth.Date = dateOfBirth.MinimumDate;
        }
        public void onLastNameChanged(object sender, EventArgs e)
        {
            username.Text = empFname.Text + "." + empLname.Text;
        }
        public void onPincodeChanged(object sender, EventArgs e)
        {
            Entry pinEntry = sender as Entry;
            String val = pinEntry.Text; //Get Current Text

            if (val.Length > 6)//If it is more than your character restriction
            {
                val = val.Remove(val.Length - 1);// Remove Last character 
                pinEntry.Text = val; //Set the Old value
            }
        }
        public void onMobileNumberChanged(object sender, EventArgs e)
        {
            Entry mobEntry = sender as Entry;
            String val = mobEntry.Text; //Get Current Text

            if (val.Length > 10)//If it is more than your character restriction
            {
                val = val.Remove(val.Length - 1);// Remove Last character 
                mobEntry.Text = val; //Set the Old value
            }
        }
    }
}
