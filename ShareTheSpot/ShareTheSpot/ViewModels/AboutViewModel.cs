using ShareTheSpot.Services;
using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace ShareTheSpot.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";

            //OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://xamarin.com/platform")));
            OpenWebCommand = new Command(() => DependencyService.Get<IDisplayMap>().StartNativeIntentOrActivity());
        }

        public ICommand OpenWebCommand { get; }
    }
}