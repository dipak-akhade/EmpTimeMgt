﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;
using Plugin.Geolocator;
using Newtonsoft.Json;
using System.Net.Http;
using System.Diagnostics;
using Xamarin.Forms.Xaml;
using nWorksLeaveApp.Common;

namespace nWorksLeaveApp.Employee
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class goingOut : ContentPage
    {
        ZXingScannerView zxing;
        ZXingDefaultOverlay overlay;
        string Latitude = "", Longitude = "", UniqueDeviceID = "", LocationAddress = "", distanceFromOrigin = "";

        public goingOut(string Lat, string Long, string DeviceId, string LocAddress, string distanceFromOrigin) : base()
        {
            BackgroundColor = ColorResources.PageBackgroundColor;

            this.distanceFromOrigin = distanceFromOrigin;
            this.Latitude = Lat;
            this.Longitude = Long;
            this.UniqueDeviceID = DeviceId;
            this.LocationAddress = LocAddress;

            zxing = new ZXingScannerView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            zxing.OnScanResult += (result) => {
                if (result.Text == "nWorks Technologies (India) Pvt. Ltd., 206 Garden Plaza, Rahatani, Pune, Maharashtra, 411 017, INDIA")
                {
                    zxing.IsAnalyzing = false;

                    Device.BeginInvokeOnMainThread(async () =>
                    {

                        if (result.Text == "nWorks Technologies (India) Pvt. Ltd., 206 Garden Plaza, Rahatani, Pune, Maharashtra, 411 017, INDIA")
                        {
                            comingIn_goingOut obj = new comingIn_goingOut();
                            try
                            {

                                obj.empId = ColorResources.LIVE_USER_ID;
                                obj.qrValue = result.Text;
                                obj.DeviceID = this.UniqueDeviceID;
                                obj.latitude = this.Latitude;
                                obj.longitude = this.Longitude;
                                obj.Location = this.LocationAddress;
                                obj.distanceFromOrigin = this.distanceFromOrigin;

                                await this.Navigation.PopAsync();
                                zxing.IsScanning = false;


                                var json = JsonConvert.SerializeObject(obj);
                                var content = new StringContent(json, Encoding.UTF8, "application/json");
                                HttpClient Client = new HttpClient();
                                HttpResponseMessage response = null;
                                response = await Client.PostAsync(ColorResources.baseUrl + "goingout", content);
                                if (response.IsSuccessStatusCode)
                                {
                                    var responseContent = await response.Content.ReadAsStringAsync();
                                    var res = JsonConvert.DeserializeObject<string>(responseContent);
                                    await DisplayAlert(" nWorksLeaveApp", res.ToString(), "OK");
                                }
                            }
                            catch (Exception ex)
                            {
                                //						await DisplayAlert(" nWorksLeaveApp","Working on it...","OK");
                                Debug.WriteLine(ex.ToString());

                            }
                        }
                        else
                        {
                            await DisplayAlert(" nWorksLeaveApp", "Invalid QR Code!", "OK");
                        }
                    });
                }
                else
                {
                    DisplayAlert(" nWorksLeaveApp", "Invalid QR Code!", "OK");
                }
            };

            overlay = new ZXingDefaultOverlay
            {
                TopText = "Hold your phone up to the barcode",
                BottomText = "Scanning will happen automatically",
                ShowFlashButton = zxing.HasTorch,
            };
            overlay.FlashButtonClicked += (sender, e) => {
                zxing.IsTorchOn = !zxing.IsTorchOn;
            };
            var grid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 500,
            };
            grid.Children.Add(zxing);
            grid.Children.Add(overlay);

            Content = new StackLayout
            {
                Padding = new Thickness(0, 15, 0, 10),
                VerticalOptions = LayoutOptions.Center,
                Children = {
                    new Label {
                        Text = "I am going...",
                        TextColor=Color.Yellow,
                        FontSize = 25,
                        VerticalOptions = LayoutOptions.StartAndExpand,
                        HorizontalOptions = LayoutOptions.Center,
                    },
                    grid

                }
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            zxing.IsScanning = true;
        }

        protected override void OnDisappearing()
        {
            zxing.IsScanning = false;

            base.OnDisappearing();
        }
    }
}