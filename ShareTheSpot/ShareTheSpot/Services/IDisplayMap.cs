using Android.OS;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ShareTheSpot.Services
{
    public interface IDisplayMap
    {
        void StartNativeIntentOrActivity();
    }
}
