using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace nWorksLeaveApp.Employee
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LeavesAndHolidays : TabbedPage
    {
        public LeavesAndHolidays()
        {
            InitializeComponent();
        }
    }
}

