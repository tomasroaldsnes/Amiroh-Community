using Amiroh.Helpers;
using Amiroh.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Amiroh.Profile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
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

        public SettingsPage()
        {
            InitializeComponent();
            var navigationPage = Application.Current.MainPage as NavigationPage;
            navigationPage.BarBackgroundColor = Color.White;
            navigationPage.BarTextColor = Color.Black;
        }

        private async void EditDescription_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditDescriptionPage(false));
        }

        private async void EditProfilePicture_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditProfilePicPage());
        }

        private async void EditName_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewAccountPageName(true)); //set a bool for navigation from settings
        }

        private async void EditEmail_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewAccountPageEmail()); //make a new constructor 
        }

        private async void Rapport_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Need to Repport Something?", "Send an email with your problem to hello@amiroh.com.", "OK"); 
        }

        private async void Feedback_Clicked(object sender, EventArgs e)
        {
             await DisplayAlert("Do you want to give us feedback?", "Great! Send an email with your feedback to hello@amiroh.com.", "OK");
        }

        private async void Rate_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Thank you for rating us!", "The Beta version of Amiroh Community does not have a direct link to your App store yet. But feel free to go rate us anyway. Sorry for the extra trouble.", "God dammit Amiroh.");
        }

        private async void SignOut_Clicked(object sender, EventArgs e)
        {
            //set logged in to false
            IsLoggedIn = "no";

            
            Navigation.InsertPageBefore(new LoginPage(), this);
            await Navigation.PopAsync();
        }




    }
}