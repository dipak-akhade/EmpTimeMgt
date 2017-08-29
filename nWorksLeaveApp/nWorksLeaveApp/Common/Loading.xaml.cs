using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace nWorksLeaveApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Loading : ContentPage
    {
        public Loading()
        {
            InitializeComponent();
            if (Device.OS.ToString() == "iOS")
            {
                this.BackgroundColor = Color.Black;
            }
            else
            {
                this.BackgroundColor = Color.Transparent;

            }
            //			this.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
        }
    }
}