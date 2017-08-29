using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Plugin.Connectivity;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Xamarin.Forms.Xaml;
using nWorksLeaveApp.Common;

namespace nWorksLeaveApp.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OnBoardEmp : ContentPage
    {
        int maleCheck = 1, femaleCheck = 0;
        string isConnected;

        public OnBoardEmp()
        {
            InitializeComponent();
            BackgroundColor = ColorResources.PageBackgroundColor;
            isConnected = CrossConnectivity.Current.IsConnected ? "Connected" : "No Connection";
            dateOfAnniversary.Date = DateTime.MinValue;
            dateOfHire.Date = DateTime.MinValue;
            dateOfBirth.Date = DateTime.MinValue;
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

        async public void Register_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(empNo.Text) || string.IsNullOrEmpty(empFname.Text) || string.IsNullOrEmpty(empMname.Text) || string.IsNullOrEmpty(empLname.Text) || string.IsNullOrEmpty(username.Text) || string.IsNullOrEmpty(desg.Text) || string.IsNullOrEmpty(address1.Text) || string.IsNullOrEmpty(city.Text) || string.IsNullOrEmpty(state.Text) || string.IsNullOrEmpty(country.Text) || string.IsNullOrEmpty(password.Text) || string.IsNullOrEmpty(confirmPassword.Text) || string.IsNullOrEmpty(mobileNumber.Text) || string.IsNullOrEmpty(emailAddress.Text))
            {
                await DisplayAlert(" nWorksLeaveApp", "Are you missing some fields?", "OK");
            }
            else
            {
                register();
            }
        }

        async public void register()
        {
            await this.Navigation.PushModalAsync(new Loading());

            try
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
                                    obj._type = "Employee";
                                    obj.userActive = "true";
                                    obj.leaveDate = "1900-01-01";
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
                                        response = await Client.PostAsync(ColorResources.baseUrl + "userRegistration", content);
                                        if (response.IsSuccessStatusCode)
                                        {
                                            var content1 = await response.Content.ReadAsStringAsync();
                                            await DisplayAlert("Message", content1.ToString(), "OK");
                                            clearAllFields();
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
                                    await DisplayAlert(" nWorksLeaveApp", "Password and Confirm Password does not matched!", "OK");
                                }
                            }
                            else
                            {
                                await DisplayAlert(" nWorksLeaveApp", "Email is invalid!", "OK");
                                emailAddress.Focus();
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
            catch (Exception ex)
            {
                await DisplayAlert(" nWorksLeaveApp", "Unable to connect server, Try again!", "OK");
                Debug.WriteLine(ex.ToString());
            }
            await this.Navigation.PopModalAsync();
        }

        public bool validateMobileNumber(string mobNumber)
        {
            if (mobNumber == "")
            {
                return false;
            }
            else
            {
                Regex rx = new Regex(@"^\d{10}$");
                if (rx.IsMatch(mobNumber))
                {
                    return true;
                }
                else
                    return false;
            }
        }
        public bool validatePincode(string pincode)
        {
            if (pincode == "")
            {
                return false;
            }
            else
            {
                Regex rx = new Regex(@"^\d{6,}$");
                if (rx.IsMatch(pincode))
                    return true;
                else
                    return false;
            }
        }
        public bool validateEmail(string email)
        {
            if (email == "")
            {
                return false;
            }
            else
            {
                Regex rx = new Regex(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");
                if (rx.IsMatch(email))
                {
                    return true;
                }
                else
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
        public void Reset_Click(object sender, EventArgs e)
        {
            clearAllFields();
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

