using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using nWorksLeaveApp.Common;

namespace nWorksLeaveApp.Admin
{
    public class detail_infoDatewisePage : CarouselPage
    {
        public detail_infoDatewisePage(List<DateWiseData> Results)
        {
            for (int i = 0; i < Results.Count; i++)
            {   //Task.Delay (1000).Wait();
                Debug.WriteLine("loading " + i.ToString());
                this.Children.Add(new detailsPage(Results[i]._Date, Results[i].dateData));
            }
        }

    }

    public class detailsPage : ContentPage
    {
        public detailsPage(string date, List<DateData> listofUser)
        {
            BackgroundColor = ColorResources.PageBackgroundColor;
            Title = Convert.ToDateTime(date).ToString("dd-MMM-yyyy");

            var empName_header = new Label()
            {
                FontFamily = "HelveticaNeue-Medium",
                FontSize = 16,
                VerticalOptions = LayoutOptions.Center,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Red,
                Text = Convert.ToDateTime(date).ToString("dd-MMM-yyyy"),
            };
            ListView listof_datewiseRecord = new ListView
            {
                BackgroundColor = ColorResources.PageBackgroundColor,
                ItemTemplate = new DataTemplate(typeof(data_cell)),
                ItemsSource = listofUser,
                RowHeight = 70,
            };

            listof_datewiseRecord.ItemTapped += async (object sender, ItemTappedEventArgs e) =>
            {
                await this.Navigation.PushModalAsync(new Loading());

                try
                {
                    if (e == null) return; // has been set to null, do not 'process' tapped event
                    ((ListView)sender).SelectedItem = null; // de-select the row

                    var selection = e.Item as DateData;
                    Debug.WriteLine("DDDDDDDDDDDDDDDD>>>" + selection.Is_Loc_Device_Changed.ToString());

                    Debug.WriteLine(selection.uid.ToString() + " " + string.Format("{0:yyyy-MM-dd HH:mm:ss}", selection._date));
                    get_inoutDetails obj = new get_inoutDetails();
                    obj.choosedDate = string.Format("{0:yyyy-MM-dd HH:mm:ss}", selection._date);
                    obj.userid = selection.uid.ToString();

                    var json = JsonConvert.SerializeObject(obj);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpClient Client = new HttpClient();
                    HttpResponseMessage response = null;
                    response = await Client.PostAsync(ColorResources.baseUrl + "get_inoutdetailsperdate", content);
                    if (response.IsSuccessStatusCode)
                    {
                        Debug.WriteLine(response.StatusCode.ToString());
                        var content1 = await response.Content.ReadAsStringAsync();
                        var res = JsonConvert.DeserializeObject<inoutDetailsPerDate>(content1);
                        if (res.inTimes.Count != 0 && res.outTimes.Count != 0)
                        {
                            await this.Navigation.PushAsync(new info_ofdayPage(res, selection.totalInTime.ToString(), selection.employeeName, selection._date, selection.weekday));
                        }
                        else
                        {
                            await DisplayAlert(" nWorksLeaveApp", "Not Appropriate Data!", "OK");
                        }
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert(" nWorksLeaveApp", "Unable to connect server, Try again!", "OK");
                    Debug.WriteLine(ex.ToString());
                }
                await this.Navigation.PopModalAsync();
            };
            var listHeader = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children ={
                    new Label{
                        Text="Date",FontFamily = "HelveticaNeue-Medium",
                        FontSize = 14,
                        WidthRequest=90,
                        TextColor = Color.Aqua,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions=LayoutOptions.Center
                    },

                    new Label{
                        Text="In Time",FontFamily = "HelveticaNeue-Medium",
                        FontSize = 14,
                        WidthRequest=60,
                        TextColor = Color.Aqua,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions=LayoutOptions.Center
                    },

                    new Label{
                        Text="Out Time",FontFamily = "HelveticaNeue-Medium",
                        FontSize = 14,
                        WidthRequest=60,
                        TextColor = Color.Aqua,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions=LayoutOptions.Center
                    },

                    new Label{
                        Text="Total",FontFamily = "HelveticaNeue-Medium",
                        FontSize = 14,
                        WidthRequest=60,
                        TextColor = Color.Aqua,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions=LayoutOptions.Center
                    },
                }
            };

            Content = new StackLayout
            {
                Padding = new Thickness(10, 5, 10, 5),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Children ={
                    empName_header,
                    listHeader,
                    listof_datewiseRecord,
                }
            };
        }
    }




    public class data_cell : ViewCell
    {
        public data_cell()
        {
            var date = new Label()
            {
                FontFamily = "HelveticaNeue-Medium",
                FontSize = 13,
                //				TextColor = ColorResources.TextColor,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Start
            };
            date.SetBinding(Label.TextProperty, "employeeName");
            date.SetBinding(Label.TextColorProperty, "Is_Loc_Device_Changed");

            var day = new Label()
            {
                FontFamily = "HelveticaNeue-Medium",
                FontSize = 13,
                //				TextColor = ColorResources.TextColor,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Start
            };
            day.SetBinding(Label.TextProperty, "uid");
            day.SetBinding(Label.TextColorProperty, "Is_Loc_Device_Changed");

            var date_and_day = new StackLayout
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Start,
                WidthRequest = 90,
                Children ={
                    date,
                    day
                }
            };

            var inTime = new Label()
            {
                FontFamily = "HelveticaNeue-Medium",
                FontSize = 13,
                WidthRequest = 60,
                //				TextColor = ColorResources.TextColor,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Start
            };
            inTime.SetBinding(Label.TextProperty, "fin");
            inTime.SetBinding(Label.TextColorProperty, "Is_Loc_Device_Changed");


            var outTime = new Label()
            {
                FontFamily = "HelveticaNeue-Medium",
                WidthRequest = 60,
                FontSize = 13,
                //				TextColor = ColorResources.TextColor,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Start
            };
            outTime.SetBinding(Label.TextProperty, "fout");
            outTime.SetBinding(Label.TextColorProperty, "Is_Loc_Device_Changed");

            var totalTime = new Label()
            {
                FontFamily = "HelveticaNeue-Medium",
                WidthRequest = 60,
                FontSize = 13,
                //				TextColor = ColorResources.TextColor,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Start
            };
            totalTime.SetBinding(Label.TextProperty, "totalInTime");
            totalTime.SetBinding(Label.TextColorProperty, "Is_Loc_Device_Changed");


            var btnDetails = new Image()
            {
                WidthRequest = 40,
                HeightRequest = 40,
                BackgroundColor = Color.Black,
                Source = "info.png",
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };

            var cellLayout = new StackLayout
            {
                Spacing = 5,
                Padding = new Thickness(5, 5, 5, 5),
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children = {
                    date_and_day,
                    inTime,
                    outTime,
                    totalTime,
                    btnDetails
                }
            };
            this.View = cellLayout;
        }
    }
}