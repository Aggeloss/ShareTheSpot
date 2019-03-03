using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ShareTheSpot.Views;
using ShareTheSpot.Models;

namespace ShareTheSpot
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BasicPage : ContentPage
	{
		public BasicPage ()
		{
			InitializeComponent ();

            Init();
		}

        void Init()
        {
            //BackgroundColor = Constants.BackgroundColor;
            ActivitySpinner.IsVisible = false;
            App.StartCheckIfInternet(lbl_NoInternet, this);
            //Entry_Password.Completed += (s, e) => SignInProcedure(s, e);
        }

        async void GoToLogin(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());
        }

        async void GoToSignup(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }

        async void GoToLoginThroughGoogle(object sender, EventArgs e)
        {
            await DisplayAlert("Info", "Coming soon...", "ok");
        }

        async void GoToLoginThroughFacebook(object sender, EventArgs e)
        {
            await DisplayAlert("Info", "Coming soon...", "ok");
        }
    }
}