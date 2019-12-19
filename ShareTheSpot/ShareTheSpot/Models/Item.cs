using System;
using Xamarin.Forms;

namespace ShareTheSpot.Models
{
    public class Item
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public ImageSource image { get; set; }
        public string Description { get; set; }
    }
}