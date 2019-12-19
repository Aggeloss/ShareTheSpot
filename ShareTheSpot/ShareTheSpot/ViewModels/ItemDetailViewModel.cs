using System;
using Android.Content;
using Android.Graphics;
using ShareTheSpot.Models;
using Xamarin.Forms;

namespace ShareTheSpot.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public Item Item { get; set; }
        public ItemDetailViewModel(Item item = null)
        {
            Title = item?.Text;
            Item = item;
        }
    }
}