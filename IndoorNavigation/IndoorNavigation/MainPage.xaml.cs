using JDA.Entities.Request;
using JDA.Entities.Response;
using System;
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

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                var restClient = new RestClient();
                if (!stkPwd.IsVisible)
                {
                    if (string.IsNullOrEmpty(UsernameEntry.Text))
                    {
                        await DisplayAlert("", "Enter User name", "Ok");
                        return;
                    }

                    var resp = await restClient.PostAsync<ValidateUserRequest, ServiceResponse<ValidateUserResponse>>(AppConstants.BaseUrl + "ValidateUser", new ValidateUserRequest { EmailId = UsernameEntry.Text });

                    if (resp != null)
                    {
                        if (resp.IsSuccess)
                        {
                            await stkUName.ScaleTo(0, 1000, Easing.BounceIn);
                            stkUName.IsVisible = false;
                            stkPwd.IsVisible = true;
                            await stkPwd.ScaleTo(1, 1000, Easing.BounceOut);
                            if (App.Current.Properties.ContainsKey("SecurityToken"))
                            {
                                App.Current.Properties["SecurityToken"] = resp.Data.SecurityToken;
                            }
                            else
                            {
                                App.Current.Properties.Add("SecurityToken", resp.Data.SecurityToken);
                            }
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(PasswordEntry.Text))
                    {
                        await DisplayAlert("", "Enter Password", "Ok");
                    }
                    var resp = await restClient.PostAsync<RegisterUserRequest, ServiceResponse<RegisterUserResponse>>(AppConstants.BaseUrl + "RegisterUser", new RegisterUserRequest { EmailId = UsernameEntry.Text, Password = Utilities.GetSha256Hash(PasswordEntry.Text) });

                    if (resp != null)
                    {
                        if (resp.IsSuccess)
                        {
                            App.Current.MainPage = new NavigationPage(new MenuPage());
                        }
                        else
                        {
                            await DisplayAlert("", "Invalid password", "Ok");
                        }
                    }

                }
            }
            catch
            {
                await DisplayAlert("", "Failed to reach server", "Ok");
            }
        }
    }
}
