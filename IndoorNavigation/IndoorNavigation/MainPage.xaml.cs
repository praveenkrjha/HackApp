using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IndoorNavigation
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            stkPwd.ScaleTo(0);
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

        }

        private void Username_Focussed(object sender, FocusEventArgs e)
        {

        }

        private void Password_Focussed(object sender, FocusEventArgs e)
        {

        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (!stkPwd.IsVisible)
            {
                stkUName.ScaleTo(0, 1000, Easing.BounceIn);
                stkUName.IsVisible = false;
                stkPwd.IsVisible = true;
                stkPwd.ScaleTo(1, 1000, Easing.BounceOut);
            }
            else
                App.Current.MainPage = new IndoorLayout();
        }
    }
}
