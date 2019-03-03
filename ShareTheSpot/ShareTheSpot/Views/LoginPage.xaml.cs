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
	public partial class LoginPage : ContentPage
	{
        public LoginPage()
        {
            InitializeComponent();
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
            //Entry_Password.Completed += (s, e) => SignInProcedure(s, e);
        }

        async void SignInProcedure(object sender, EventArgs e)
        {

            User user = new User(Entry_Username.Text, Entry_Password.Text);
            /*IEnumerable<User> dbUsers = App.UserDatabase.GetUserByName(user.Username);

            var name = "";

            foreach (User user_local_db in dbUsers)
            {
                name = user_local_db.Username;
            }*/

            var users = await App.RestService.GetUsers();
            await DisplayAlert("Test", "Testingg => " + users[2].Value.ToString() + " @token => " + users[2].Key, "Ok");
            var check = false;

            foreach (KeyValuePair<string, User> user_firebase_db in users)
            {
                if (user.Username == user_firebase_db.Value.Username && user.Password == user_firebase_db.Value.Password)
                {
                    check = true;
                    Settings.AccessToken = user_firebase_db.Key;
                }
            }
            //User usertest = dbUser.FirstOrDefault<User>;

            if (user.checkInformation()) //&& name == user.Username
            {
                //Settings.AccessToken = user_token.access_token;
                //Token user_token = new Token();
                //user_token.access_token = "1ZYX89HJQKS90KSLAS123KNK80"; //generateToken(name);
                //user_token.expire_date = DateTime.UtcNow.AddHours(1);

                //await DisplayAlert("Login", "Login Success with token " + user_token.ToString(), "Ok");
                await DisplayAlert("Login", "Login Success with token => " + Settings.AccessToken, "Ok");

                //Settings.AccessToken = user_token.access_token;

                //string test_value = this.translateToken(System.Text.Encoding.ASCII.GetBytes(user_token.access_token), false);

                if (Settings.AccessToken != null && check == true)
                {

                    //not result.access_token
                    //App.UserDatabase.SaveUser(user);
                    //App.TokenDatabase.SaveToken(result);

                    if (Device.OS == TargetPlatform.Android)
                    {
                        Application.Current.MainPage = new MainPage();
                    }
                    else if (Device.OS == TargetPlatform.iOS)
                    {
                        await Navigation.PushModalAsync(new MainPage());
                    }
                    //await Navigation.PushAsync(new Dashboard());
                }
            }
            else
            {
                await DisplayAlert("Login", "Login Not Correct, empty username password.", "ok");
            }
        }

        /*public string generateToken(string input)
        {
            MD5 md5 = MD5.Create(input);
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);

            byte[] hashBytes = md5.ComputeHash(inputBytes);

            StringBuilder strBuild = new StringBuilder();

            for (int i = 0; i < hashBytes.Length; i++)
                strBuild.Append(hashBytes[i].ToString("X2"));

            return strBuild.ToString();
        }

        public string translateToken(byte[] bytes, bool upperCase)
        {
            StringBuilder result = new StringBuilder(bytes.Length * 2);

            for (int i = 0; i < bytes.Length; i++)
                result.Append(bytes[i].ToString(upperCase ? "X2" : "x2"));

            return result.ToString();
        }*/
    }
}