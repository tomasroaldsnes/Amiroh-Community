using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;
using Plugin.Media;

namespace Amiroh
{
    public partial class App : Application
    {
        public static bool IsUserLoggedIn { get; set; }

        //public string IsFirstTime
        //{
        //    get { return Settings.GeneralSettings; }
        //    set
        //    {
        //        if (Settings.GeneralSettings == value)
        //            return;
        //        Settings.GeneralSettings = value;
        //        OnPropertyChanged();
        //    }
        //}



        public App()
        {
            InitializeComponent();
            CrossMedia.Current.Initialize();

            

            if (!IsUserLoggedIn)
            {
                MainPage = new NavigationPage(new Amiroh.Login.LoginPage());
            }
            else
            {
                MainPage = new NavigationPage(new Amiroh.MainPage());
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            MobileCenter.Start("807dda2e-7898-4a82-9a94-ccd8e28dc754",
                   typeof(Analytics), typeof(Crashes));
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
