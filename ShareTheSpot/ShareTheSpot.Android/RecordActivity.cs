using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ShareTheSpot.Droid;
using ShareTheSpot.Services;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(RecordActivity))]
namespace ShareTheSpot.Droid
{
    public class RecordActivity : IDisplayMap
    {
        public void StartNativeIntentOrActivity()
        {
            Intent intent = new Intent(Forms.Context, typeof(DisplayMap));
            Forms.Context.StartActivity(intent);
        }
    }
}