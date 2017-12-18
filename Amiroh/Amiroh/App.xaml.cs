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
