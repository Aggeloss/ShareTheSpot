using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ShareTheSpot.Models
{
    class Constants
    {
        public static bool IsDev = true;

        public static Color BackgroundColor = Color.FromRgb(131, 171, 0);
        public static Color MainTextColor = Color.White;
        public static int LoginIconHeight = 120;

        //===== Login Routes ============================
        public static string LoginUrl = "https://xamarinmapsdb.firebaseio.com/Users.json";

        //===== Messages ================================
        public static string NoInternetText = "No Internet, please reconnect.";
    }
}
