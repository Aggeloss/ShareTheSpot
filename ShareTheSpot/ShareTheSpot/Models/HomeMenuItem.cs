using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ShareTheSpot.Models
{
    public enum MenuItemType
    {
        Browse,
        About,
        Profile,
        Logout
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }

        //public Image image { get; set; }
    }
}
