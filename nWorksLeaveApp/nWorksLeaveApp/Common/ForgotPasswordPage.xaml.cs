using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Diagnostics;
using Xamarin.Forms.Xaml;

namespace nWorksLeaveApp.Common
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgotPasswordPage : ContentPage
    {
        string userid = "", emailaddress = "", OTP = "";
        public ForgotPasswordPage()
        {
            InitializeComponent();
            this.BackgroundColor = ColorResources.PageBackgroundColor;
            getUsers();
        }
        public void PickerUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            label_isOTPGenerate.Text = "";
            if (pickerUser.SelectedIndex == -1)
            {
                DisplayAlert(" nWorksLeaveApp", "Invalid Selection!", "OK");
            }
            else
            {
                string selection = (pickerUser.Items[pickerUser.SelectedIndex]);
                int len = selection.IndexOf(":");

                if (len > 0)
                {
                    userid = selection.Substring(0, len);
                }
                if (selection.ToString() == "Select")
                {
                    //					clearAllFields ();
                }
                else
                {
                    gettingEmailOfSelectedEmployee(userid);
                    Debug.WriteLine(userid.ToString());
                }
            }
        }

        public void emailTextChanged(object sender, EventArgs e)
        {
            entryEmail.Text = emailaddress;
        }

        async public void gettingEmailOfSelectedEmployee(string uid)
        {
            await this.Navigation.PushModalAsync(new Loading());
            try
            {
                Debug.WriteLine("Service calling!");

                passUid userid = new passUid();
                userid.uid = uid;
                var json = JsonConvert.SerializeObject(userid);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpClient Client = new HttpClient();
                HttpResponseMessage response = null;
                response = await Client.PostAsync(ColorResources.baseUrl + "getUserMailAddress", content);
                if (response.IsSuccessStatusCode)
                {
                    var content1 = await response.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<string>(content1);
                    emailaddress = res.ToString();
                    entryEmail.Text = emailaddress;
                    Debug.WriteLine("Service called!" + res.ToString());
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert(" nWorksLeaveApp", "Unable to connect server, Try again!", "OK");
                Debug.WriteLine(ex.ToString());
            }
            await this.Navigation.PopModalAsync();

        }
        async public void GenerateOTP_Click(object sender, EventArgs e)
        {
            btnGenerateOTP.IsEnabled = false;
            await this.Navigation.PushModalAsync(new Loading());

            try
            {
                getOTP getOtp = new getOTP();

                if (string.IsNullOrEmpty(entryEmail.Text) || string.IsNullOrWhiteSpace(entryEmail.Text))
                {
                    await DisplayAlert(" nWorksLeaveApp", "Invalid Email Address!", "OK");
                }
                else
                {
                    getOtp.uid = userid;
                    getOtp.email = entryEmail.Text;
                    var json = JsonConvert.SerializeObject(getOtp);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpClient Client = new HttpClient();
                    HttpResponseMessage response = null;
                    response = await Client.PostAsync(ColorResources.baseUrl + "sendOTP", content);
                    if (response.IsSuccessStatusCode)
                    {
                        var content1 = await response.Content.ReadAsStringAsync();
                        var res = JsonConvert.DeserializeObject<string>(content1);
                        Debug.WriteLine(res.ToString());
                        if (res.Length == 7)
                        {
                            label_isOTPGenerate.Text = "OTP is successfully sent on your email address!";
                            OTP = res.ToString();
                        }
                        else
                            label_isOTPGenerate.Text = "Email Sending Failure! Try Again!";
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert(" nWorksLeaveApp", "Unable to connect server, Try again!", "OK");
                Debug.WriteLine(ex.ToString());
            }
            btnGenerateOTP.IsEnabled = true;
            await this.Navigation.PopModalAsync();

        }
        async public void Submit_Click(object sender, EventArgs e)
        {
            label_isOTPGenerate.Text = "";

            try
            {
                if (string.IsNullOrEmpty(entryOTP.Text))
                {
                    await DisplayAlert(" nWorksLeaveApp", "Please enter OTP!", "ok");
                }
                else if (string.IsNullOrEmpty(entryNewPassword.Text))
                {
                    await DisplayAlert(" nWorksLeaveApp", "Please enter New Password!", "ok");
                }
                else if (string.IsNullOrEmpty(entryConfirmPassword.Text))
                {
                    await DisplayAlert(" nWorksLeaveApp", "Please enter Confirm Password!", "ok");
                }
                else if (!((entryNewPassword.Text).Equals(entryConfirmPassword.Text)))
                {
                    await DisplayAlert(" nWorksLeaveApp", "New Password and Confirm Password is not matched!", "OK");
                }
                else if ((entryOTP.Text).Equals(OTP))
                {
                    btnSubmit.IsEnabled = false;
                    await this.Navigation.PushModalAsync(new Loading());
                    //reset password actually
                    resetPassword resetPass = new resetPassword();
                    resetPass.password = entryNewPassword.Text;
                    resetPass.uid = userid;

                    var json = JsonConvert.SerializeObject(resetPass);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpClient Client = new HttpClient();
                    HttpResponseMessage response = null;
                    response = await Client.PostAsync(ColorResources.baseUrl + "resetPasscode", content);
                    if (response.IsSuccessStatusCode)
                    {
                        var content1 = await response.Content.ReadAsStringAsync();
                        var res = JsonConvert.DeserializeObject<string>(content1);
                        await this.Navigation.PopModalAsync();
                        await this.Navigation.PopAsync();
                        await DisplayAlert(" nWorksLeaveApp", res.ToString(), "OK");

                    }
                }
                else
                {
                    await DisplayAlert(" nWorksLeaveApp", "You enter wrong OTP!", "OK");
                }
                btnSubmit.IsEnabled = true;
                await this.Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                //				await DisplayAlert (" nWorksLeaveApp","Unable to connect server, Try again!","OK");
                Debug.WriteLine(ex.ToString());
            }
        }
        async public void getUsers()
        {
            await this.Navigation.PushModalAsync(new Loading());

            try
            {
                HttpClient client = new HttpClient();
                string RestUrl = ColorResources.baseUrl + "getlistofalluser";
                var uri = new Uri(string.Format(RestUrl, string.Empty));
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var Items = JsonConvert.DeserializeObject<List<modelUserID_Username>>(content);

                    pickerUser.Items.Add("Select");

                    for (int i = 0; i < Items.Count; i++)
                    {
                        pickerUser.Items.Add(Items[i].uid.ToString() + ":" + Items[i].username.ToString());
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
    }
}