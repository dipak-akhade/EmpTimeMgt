using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms.Xaml;
using nWorksLeaveApp.Common;

namespace nWorksLeaveApp.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class detail_infoPage : CarouselPage
    {
        public detail_infoPage(List<EmployeeWiseData> Results)
        {
            for (int i = 0; i < Results.Count; i++)
            {
                Debug.WriteLine(i.ToString());
                //				Task.Delay (1000).Wait();
                this.Children.Add(new redpage(Results[i].EmployeeName, Results[i].employeeWiseData));
            }
        }
    }
    public class redpage : ContentPage
    {
        public redpage(string empName, List<EmployeeData> listofdate)
        {
            BackgroundColor = ColorResources.PageBackgroundColor;

            var empName_header = new Label()
            {
                FontFamily = "HelveticaNeue-Medium",
                FontSize = 16,
                VerticalOptions = LayoutOptions.Center,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Red,
                Text = empName.ToString(),
            };
            ListView listof_datewiseRecord = new ListView
            {
                BackgroundColor = ColorResources.PageBackgroundColor,
                ItemTemplate = new DataTemplate(typeof(dataCell)),
                ItemsSource = listofdate,
                RowHeight = 70,
            };

            listof_datewiseRecord.ItemTapped += async (object sender, ItemTappedEventArgs e) =>
            {
                await this.Navigation.PushModalAsync(new Loading());

                try
                {
                    if (e == null) return; // has been set to null, do not 'process' tapped event
                    ((ListView)sender).SelectedItem = null; // de-select the row

                    var selection = e.Item as EmployeeData;
                    //				Debug.WriteLine(selection.uid.ToString()+" "+ string.Format("{0:yyyy-MM-dd HH:mm:ss}",selection.date));
                    get_inoutDetails obj = new get_inoutDetails();
                    obj.choosedDate = string.Format("{0:yyyy-MM-dd HH:mm:ss}", selection.date);
                    obj.userid = selection.uid.ToString();
                    //					Debug.WriteLine("Is device or location changed :"+selection.Is_Loc_Device_Changed.ToString());
                    var json = JsonConvert.SerializeObject(obj);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpClient Client = new HttpClient();
                    HttpResponseMessage response = null;
                    response = await Client.PostAsync(ColorResources.baseUrl + "get_inoutdetailsperdate", content);
                    if (response.IsSuccessStatusCode)
                    {
                        //						Debug.WriteLine(response.StatusCode.ToString());
                        var content1 = await response.Content.ReadAsStringAsync();
                        var res = JsonConvert.DeserializeObject<inoutDetailsPerDate>(content1);
                        if (res.inTimes != null || res.outTimes != null)
                            await this.Navigation.PushAsync(new info_ofdayPage(res, selection.totalInTime.ToString(), empName, selection.date, selection.weekday));
                        else
                            await DisplayAlert(" nWorksLeaveApp", "Invalid Selection!", "OK");
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




    public class dataCell : ViewCell
    {
        public dataCell()
        {
            var date = new Label()
            {
                FontFamily = "HelveticaNeue-Medium",
                FontSize = 13,
                //				TextColor = ColorResources.TextColor,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Start
            };
            date.SetBinding(Label.TextProperty, "date");
            date.SetBinding(Label.TextColorProperty, "Is_Loc_Device_Changed");

            var day = new Label()
            {
                FontFamily = "HelveticaNeue-Medium",
                FontSize = 13,
                //				TextColor = ColorResources.TextColor,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Start
            };
            day.SetBinding(Label.TextProperty, "weekday");
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
            inTime.SetBinding(Label.TextProperty, "In");
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
            outTime.SetBinding(Label.TextProperty, "Out");
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
    //	public class userDataModel
    //	{
    //		public string empName { get; set; }
    //
    //		public string id{ get; set;}
    //
    //		public string date{ get; set;}
    //
    //		public string day{ get; set;}
    //
    //		public string inTime{ get; set;}
    //
    //		public string outTime{ get; set;}
    //
    //		public string totalTime{ get; set;}
    //
    //	}
    //	public static class userData
    //	{
    //		public static List<userDataModel> GetData ()
    //		{
    //			return new List<userDataModel> {
    //
    //				new userDataModel () {
    //
    //					empName="Dipak Akhade",id="1",date="13/07/2016",day="Wednesday",inTime="10.30",outTime="13.56",totalTime="8.30"
    //				},
    //
    //				new userDataModel () {
    //					empName="Rajeev Prakash",id="2",date="13/07/2016",day="Thursday",inTime="10.30",outTime="13.56",totalTime="7.45"
    //				},
    //
    //				new userDataModel () {
    //					empName="Parag Bhosale",id="3",date="13/07/2016",day="Wednesday",inTime="10.30",outTime="13.56",totalTime="8"						
    //				},
    //
    //				new userDataModel () {
    //					empName="Rupesh Chavan",id="4",date="15/07/2016",day="Friday",inTime="10.30",outTime="13.56",totalTime="8.13"
    //				},
    //
    //				new userDataModel () {
    //					empName="Saurabh Pawar",id="5",date="16/07/2016",day="Saturday",inTime="10.30",outTime="13.56",totalTime="5.22"
    //				},
    //
    //				new userDataModel () {
    //					empName="Heena Mulla",id="6",date="17/07/2016",day="Monday",inTime="10.30",outTime="13.56",totalTime="8.22"
    //				},
    //
    //				new userDataModel () {
    //					empName="Prawal Gupta",id="7",date="13/07/2016",day="Wednesday",inTime="10.30",outTime="13.56",totalTime="9.49",
    //				},
    //
    //				new userDataModel () {
    //					empName="Sunil Targe",id="8",date="19/07/2016",day="Tuesday",inTime="10.30",outTime="13.56",totalTime="10.00"
    //				}
    //			};
    //		}
    //	}
}
