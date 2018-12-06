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
	public partial class AttendancePage : ContentPage
	{
		public AttendancePage ()
		{
			InitializeComponent ();
            var restClient = new RestClient();
            Device.BeginInvokeOnMainThread(async() =>
            {
                try
                {
                    var resp = await restClient.GetAsync<ServiceResponse<List<AttendenceData>>>(AppConstants.BaseUrl + "Attendance");
                    lstAttendance.ItemsSource = resp.Data;
                }
                catch
                {
                    await DisplayAlert("", "Failed to reach server", "Ok");
                }
            });
            lstAttendance.RefreshCommand = new Command(async() => {
                //Do your stuff.
                try
                {
                    lstAttendance.ItemsSource = null;
                    var resp = await restClient.GetAsync<ServiceResponse<List<AttendenceData>>>(AppConstants.BaseUrl + "Attendance");
                    lstAttendance.ItemsSource = resp.Data;
                    lstAttendance.IsRefreshing = false;
                }
                catch
                {
                    await DisplayAlert("", "Failed to reach server", "Ok");
                }
            });
        }
	}
}