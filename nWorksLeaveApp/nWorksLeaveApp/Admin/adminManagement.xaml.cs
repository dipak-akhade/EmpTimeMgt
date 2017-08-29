using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Plugin.Connectivity;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Text;

using Xamarin.Forms.Xaml;
using nWorksLeaveApp.Models;
using nWorksLeaveApp.Common;

namespace nWorksLeaveApp.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class adminManagement : ContentPage
    {
        int maleCheck = 1, femaleCheck = 0;
        string uid = "", isConnected = "", Userid = "";

        public adminManagement()
        {
            InitializeComponent();
            BackgroundColor = ColorResources.PageBackgroundColor;
            dateOfAnniversary.Date = DateTime.MinValue;
            dateOfHire.Date = DateTime.MinValue;
            dateOfBirth.Date = DateTime.MinValue;
            getAdmins();
            isConnected = CrossConnectivity.Current.IsConnected ? "Connected" : "No Connection";
        }
        async public void Update_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(empNo.Text) || string.IsNullOrEmpty(empFname.Text) || string.IsNullOrEmpty(empMname.Text) || string.IsNullOrEmpty(empLname.Text) || string.IsNullOrEmpty(username.Text) || string.IsNullOrEmpty(desg.Text) || string.IsNullOrEmpty(address1.Text) || string.IsNullOrEmpty(city.Text) || string.IsNullOrEmpty(state.Text) || string.IsNullOrEmpty(country.Text) || string.IsNullOrEmpty(mobileNumber.Text) || string.IsNullOrEmpty(emailAddress.Text))
            {
                await DisplayAlert(" nWorksLeaveApp", "Are you missing some fields?", "OK");
            }
            else
            {
                if (pickerAdmin.SelectedIndex == -1)
                {
                    await DisplayAlert(" nWorksLeaveApp", "Select Emplyee Please!", "OK");
                }
                else
                {
                    UpdateAdmin();
                }
            }
        }
        async public void Remove_Click(object sender, EventArgs e)
        {
            passUid userid = new passUid();
            userid.uid = uid;
            if (userid.uid != "")
            {
                var result = await DisplayAlert(" nWorksLeaveApp", "Do you really wants to remove " + userid.uid.ToString(), "Yes", "No");
                if (result == true)
                {
                    try
                    {
                        var json = JsonConvert.SerializeObject(userid);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");
                        HttpClient Client = new HttpClient();
                        HttpResponseMessage response = null;
                        response = await Client.PostAsync(ColorResources.baseUrl + "deleteAdmin", content);
                        if (response.IsSuccessStatusCode)
                        {
                            var content1 = await response.Content.ReadAsStringAsync();
                            await DisplayAlert(" nWorksLeaveApp", content1.ToString(), "OK");
                            await this.Navigation.PopAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert(" nWorksLeaveApp", ex.ToString(), "OK");
                    }
                }
            }
            else
            {
                await DisplayAlert(" nWorksLeaveApp", "Select user please!", "OK");
            }
        }
        public void PickerEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pickerAdmin.SelectedIndex == -1)
            {
                DisplayAlert(" nWorksLeaveApp", "Invalid Selection!", "OK");
            }
            else
            {
                string selection = (pickerAdmin.Items[pickerAdmin.SelectedIndex]);
                int len = selection.IndexOf(":");

                if (len > 0)
                {
                    uid = selection.Substring(0, len);
                }
                if (selection.ToString() == "Select")
                {
                    clearAllFields();
                }
                else
                {
                    gettingDetailsOfSelectedEmployee();
                }
            }
        }
        async public void getAdmins()
        {
            await this.Navigation.PushModalAsync(new Loading());

            try
            {
                HttpClient client = new HttpClient();
                string RestUrl = ColorResources.baseUrl + "getadminlist";
                var uri = new Uri(string.Format(RestUrl, string.Empty));
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var Items = JsonConvert.DeserializeObject<ModelGetEmployeeList>(content);
                    pickerAdmin.Items.Add("Select");
                    foreach (EmployeeList emp in Items.EmpList)
                    {
                        pickerAdmin.Items.Add(emp.uid.ToString() + ":" + emp.fname.ToString() + " " + emp.lname.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert(" nWorksLeaveApp", ex.ToString(), "OK");
            }
            await this.Navigation.PopModalAsync();
        }
        async public void UpdateAdmin()
        {
            if (isConnected == "Connected")
            {
                if (validateMobileNumber(mobileNumber.Text) == true)
                {
                    if (validatePincode(pinCode.Text) == true)
                    {
                        if (validateEmail(emailAddress.Text) == true)
                        {
                            userRegistration obj = new userRegistration();
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
                            obj._type = "Employee";
                            obj.userActive = "true";
                            obj.leaveDate = "0000-00-00";
                            obj.anniversary = dateOfAnniversary.Date.ToString("yyyy-MM-dd");
                            obj.desg = desg.Text;
                            obj.inout_status = "Outside";
                            obj.email = emailAddress.Text;
                            if (switchActiveInactive.IsToggled == true)
                            {
                                obj.userActive = "true";
                            }
                            else
                            {
                                obj.userActive = "false";
                            }
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
                                response = await Client.PostAsync(ColorResources.baseUrl + "manageemployee", content);
                                if (response.IsSuccessStatusCode)
                                {
                                    var content1 = await response.Content.ReadAsStringAsync();
                                    await DisplayAlert("Message", content1.ToString(), "OK");
                                    await this.Navigation.PopAsync();
                                }
                            }
                            catch (Exception ex)
                            {
                                await DisplayAlert(" nWorksLeaveApp", ex.ToString(), "OK");
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
        public void uidTextChanged(object sender, EventArgs e)
        {
            empNo.Text = Userid;
        }
        public void clearAllFields()
        {
            switchActiveInactive.IsToggled = false;
            labelActive.Text = "";
            empNo.Text = "";
            empFname.Text = "";
            empMname.Text = "";
            empLname.Text = "";
            username.Text = "";
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
                    //assign response to variables/fields
                    empNo.Text = Items.Emp.uid;
                    empFname.Text = Items.Emp.fname;
                    empMname.Text = Items.Emp.mname;
                    empLname.Text = Items.Emp.lname;

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
                    if (Items.Emp.userActive == "true")
                    {
                        switchActiveInactive.IsToggled = true;
                    }
                    else
                    {
                        switchActiveInactive.IsToggled = false;
                    }
                    desg.Text = Items.Emp.desg;
                    username.Text = Items.Emp.username;
                    address1.Text = Items.Emp.addressLine1;
                    address2.Text = Items.Emp.addressLine2;
                    state.Text = Items.Emp.state;
                    city.Text = Items.Emp.city;
                    pinCode.Text = Items.Emp.pincode;
                    country.Text = Items.Emp.country;
                    mobileNumber.Text = Items.Emp.mobileNo;
                    emailAddress.Text = Items.Emp.email;
                    //dateOfHire.Date = emp.hire_date;
                    dateOfAnniversary.Date = Items.Emp.anniversary;
                    dateOfBirth.Date = Items.Emp.dob;


                }
            }
            catch (Exception ex)
            {
                await DisplayAlert(" nWorksLeaveApp", ex.ToString(), "OK");
            }
            await this.Navigation.PopModalAsync();

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
        public void onLastNameChanged(object sender, EventArgs e)
        {
            username.Text = empFname.Text + "." + empLname.Text;
        }

        public void Reset_Click(object sender, EventArgs e)
        {
            clearAllFields();
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
        public void switchTaggled(object sender, EventArgs e)
        {
            if (switchActiveInactive.IsToggled == true)
            {
                labelActive.Text = "Active";
            }
            else
            {
                labelActive.Text = "Inactive";
            }
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

    }
}
