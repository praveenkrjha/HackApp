using JDA.Entities.Request;
using JDA.Entities.Response;
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
            MessagingCenter.Subscribe<FModel>(this, "Hello", async(s) =>
             {
                 MessagingCenter.Unsubscribe<FModel>(this, "Hello");
                 try
                 {
                     bool resp = await MarkAttendance();
                     if (resp)
                     {
                         await DisplayAlert("Info", "Attendance marked", "Ok");
                     }
                     else
                     {
                         await DisplayAlert("Alert", "Attendance not marked", "OK");
                     }
                     await Navigation.PopAsync();
                 }
                 catch (Exception ex)
                 {
                     Console.WriteLine(ex);
                 }
             });
        }

        private async Task<bool> MarkAttendance()
        {
            var restClient = new RestClient();
            var resp = await restClient.PostAsync<BlankRequest, ServiceResponse<string>>(AppConstants.BaseUrl + "Attendance", new BlankRequest { Id = 1 });
            return resp.IsSuccess;
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