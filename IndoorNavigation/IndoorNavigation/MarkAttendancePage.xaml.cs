using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IndoorNavigation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MarkAttendancePage : ContentPage
    {

        public MarkAttendancePage()
        {
            InitializeComponent();
            DependencyService.Get<IFingerprint>().Authenticate();
            MessagingCenter.Subscribe<FModel>(this, "Hello", (s) =>
             {
                 Navigation.PopAsync();
                 MessagingCenter.Unsubscribe<FModel>(this, "Hello");
             });
        }

        public static void FPNavigate()
        {
            //await Navigation.PopAsync();

        }
    }
    public class FModel
    {

        public bool flag
        {
            get { return true; }
            set
            {
                MessagingCenter.Send(this, "Hello");
            }
        }

    }
}