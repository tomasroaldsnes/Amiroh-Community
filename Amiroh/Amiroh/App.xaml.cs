using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;
using Plugin.Media;
using Amiroh.Helpers;
using Amiroh.Classes;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Push;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace Amiroh
{
    public partial class App : Application
    {
        public string IsLoggedIn
        {
            get { return Settings.LoginSettings; }
            set
            {
                if (Settings.LoginSettings == value)
                    return;
                Settings.LoginSettings = value;
                OnPropertyChanged();
            }
        }

        public string SetUsername
        {
            get { return Settings.UsernameSettings; }
            set
            {
                if (Settings.UsernameSettings == value)
                    return;
                Settings.UsernameSettings = value;
                OnPropertyChanged();
            }
        }

        public string SetUserId
        {
            get { return Settings.IdSettings; }
            set
            {
                if (Settings.IdSettings == value)
                    return;
                Settings.IdSettings = value;
                OnPropertyChanged();
            }
        }

        public string SetUserDescription
        {
            get { return Settings.DescriptionSettings; }
            set
            {
                if (Settings.DescriptionSettings == value)
                    return;
                Settings.DescriptionSettings = value;
                OnPropertyChanged();
            }
        }

        public string SetProfilePicture
        {
            get { return Settings.ProfilePictureSettings; }
            set
            {
                if (Settings.ProfilePictureSettings == value)
                    return;
                Settings.ProfilePictureSettings = value;
                OnPropertyChanged();
            }
        }



        public App()
        {
            InitializeComponent();
            CrossMedia.Current.Initialize();

            

            if (IsLoggedIn == "yes")
            {
                MainUser.MainUserID.ID = SetUserId;
                MainUser.MainUserID.Username = SetUsername;
                MainUser.MainUserID.ProfileDescription = SetUserDescription;
                MainUser.MainUserID.ProfilePicture = SetProfilePicture;
                MainPage = new NavigationPage(new Amiroh.MainPage());
            }
            else
            {
                MainPage = new NavigationPage(new Amiroh.Login.LoginPage());
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            AppCenter.Start("android=807dda2e-7898-4a82-9a94-ccd8e28dc754;" +
                   "ios=dc9fa6db-6cf9-4d76-9002-a85f6037ea6e",
                   typeof(Microsoft.AppCenter.Analytics.Analytics), typeof(Microsoft.AppCenter.Crashes.Crashes));

            AppCenter.Start("807dda2e-7898-4a82-9a94-ccd8e28dc754", typeof(Push));

            AppCenter.Start("dc9fa6db-6cf9-4d76-9002-a85f6037ea6e", typeof(Push));
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
