using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using ShareTheSpot.Models;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;
using ShareTheSpot.Services;
using ShareTheSpot.Helpers;

namespace ShareTheSpot.Models
{
    public class User
    {
        public int Id { set; get; }
        public string Username { set; get; }
        public string Password { set; get; }
        public string Message { set; get; }
        public RestService _apiServices;

        public User()
        {
            //Username = Settings.Username;
            //Password = Settings.Password;
            _apiServices = new RestService();
        }

        public User(string Username, string Password)
        {
            this.Username = Username;
            this.Password = Password;
            _apiServices = new RestService();
        }

        public bool checkInformation()
        {
            if (!this.Username.Equals("") && !this.Password.Equals(""))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public ICommand RegisterCommand
        {
            get
            {
                return new Command(async () =>
                {
                    var user = new User{
                        Username = this.Username,
                        Password = this.Password
                    };

                    string json = JsonConvert.SerializeObject(user);

                    var isSuccess = await _apiServices.PostRequest<Task<Token>>(Constants.LoginUrl, json);
                    //Username, Password);

                    //Settings.Username = Username;
                    //Settings.Password = Password;

                    //if (isSuccess)
                    if (isSuccess != null)
                        Message = "Registered successfully";
                    else
                        Message = "Try again";
                });
            }
        }

        public ICommand LoginCommand
        {
            get
            {
                return new Command(async () =>
                {
                    var accesstoken =  await _apiServices.Login(this);

                    Settings.AccessToken = accesstoken.access_token;
                });
            }
        }

        public string ToString()
        {
            return "ID: " + this.Id + " || Username: " + this.Username + " || Password: " + this.Password;
        }
    }
}
