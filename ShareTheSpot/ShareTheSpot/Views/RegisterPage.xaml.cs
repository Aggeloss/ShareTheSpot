using ShareTheSpot.Helpers;
using ShareTheSpot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShareTheSpot.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RegisterPage : ContentPage
	{
		public RegisterPage ()
		{
			InitializeComponent ();

            Init();
		}

        void Init()
        {
            //BackgroundColor = Constants.BackgroundColor;
            Lbl_Username.TextColor = Constants.MainTextColor;
            Lbl_Password.TextColor = Constants.MainTextColor;
            ActivitySpinner.IsVisible = false;
            LoginIcon.HeightRequest = Constants.LoginIconHeight;
            App.StartCheckIfInternet(lbl_NoInternet, this);

            Entry_Username.Completed += (s, e) => Entry_Password.Focus();
            //Entry_Password.Completed += (s, e) => SignUpProcedure(s, e);
        }

        async void SignUpProcedure(object sender, EventArgs e)
        {
            User user = new User(Entry_Username.Text, Entry_Password.Text);

            var users = await App.RestService.GetUsers();



            if (user.checkInformation())
            {
                var check = false; //In case you find user's name-email, set check = false. (To avoid dublicates)

                foreach (KeyValuePair<string, User> user_firebase_db in users)
                {
                    if (user.Username == user_firebase_db.Value.Username)
                    {
                        check = true;
                    }
                }
                /*IEnumerable<User> dbUsers = App.UserDatabase.GetUserByName(user.Username);

                var check = true;
                var id = 0;

                foreach (User userCheck in dbUsers)
                {
                    if (user.Username == userCheck.Username)
                        check = false;
                }*/

                if (!check)
                {
                    //id = App.UserDatabase.SaveUser(user);

                    await DisplayAlert("Current Statement", "Sending request to server..", "ok");

                    string test = await App.RestService.SaveUserAsync(user, true);

                    /*Token x = rs.Login(
                        new User {
                            Username = user.Username,
                            Password = user.Password
                        }
                     ).Result;*/

                    await DisplayAlert("Successful", "Your registration has been assigned successfully with id = " + test, "ok");

                    Settings.Username = user.Username;
                    Settings.Password = user.Password;

                    //var result = await App.RestService.Login(user);
                    var result = new Token();
                    //App.TokenDatabase.SaveToken(result);
                    if (result != null)
                    {
                        //not result.access_token
                        //App.UserDatabase.SaveUser(user);
                        //App.TokenDatabase.SaveToken(result);
                        Application.Current.MainPage = new LoginPage();
                        //await Navigation.PushAsync(new Dashboard());
                    }
                }
                else
                {
                    await DisplayAlert("Failed", "Username is already exists. Please try again!", "ok");
                }
            }
            else
            {
                await DisplayAlert("Failed", "Something went wrong. Please try again!", "ok");
            }
        }
    }
}