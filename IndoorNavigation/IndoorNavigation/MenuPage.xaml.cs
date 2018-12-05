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
    public partial class MenuPage : ContentPage
    {
        public MenuPage()
        {
            InitializeComponent();
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            try
            {
                const int _animationTime = 20;
                await OuterStk.ScaleTo(1.2, _animationTime);
                await OuterStk.ScaleTo(1, _animationTime);
                await Navigation.PushAsync(new LocatedItemsPage());
            }
            catch (Exception ez)
            {
                ez.Message.ToString();
            }
           
        }

        private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            const int _animationTime = 20;
            await WhereamiStk.ScaleTo(1.2, _animationTime);
            await WhereamiStk.ScaleTo(1, _animationTime);
            await Navigation.PushAsync(new IndoorLayout());
            
        }

        private async void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
        {
            const int _animationTime = 20;
            await MarkAttendance.ScaleTo(1.2, _animationTime);
            await MarkAttendance.ScaleTo(1, _animationTime);
            await Navigation.PushAsync(new MarkAttendancePage());
        }

        private async void TapGestureRecognizer_Tapped_3(object sender, EventArgs e)
        {
            const int _animationTime = 20;
            await ViewAttendance.ScaleTo(1.2, _animationTime);
            await ViewAttendance.ScaleTo(1, _animationTime);
            
        }

        private async void TapGestureRecognizer_Tapped_4(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }
    }
}