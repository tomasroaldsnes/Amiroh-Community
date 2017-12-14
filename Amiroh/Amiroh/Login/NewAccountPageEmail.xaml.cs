using Amiroh.Classes;
using Amiroh.Controllers;
using ModernHttpClient;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Amiroh.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewAccountPageEmail : ContentPage
    {
        
        private ObservableCollection<User> _users;

        User mainUserObj;



        public NewAccountPageEmail(User o, ObservableCollection<User> list)
        {
            InitializeComponent();

            var navigationPage = Application.Current.MainPage as NavigationPage;
            navigationPage.BarBackgroundColor = Color.White;
            navigationPage.BarTextColor = Color.Black;

            _users = list;
            mainUserObj = o;
        }

      

        private async void Next_Clicked(object sender, EventArgs e)
        {
            btnNext.IsEnabled = false;
            if (emailEntry.Text == "")
            {
                await DisplayAlert("Oops!", "Please type in your email in the field-thingy.", "OK");
                btnNext.IsEnabled = true;
            }
            else if (!emailEntry.Text.Contains("@") | !emailEntry.Text.Contains("."))
            {
                await DisplayAlert("Oops!", "Please type in a valid email in the field-thingy.", "OK");
                btnNext.IsEnabled = true;
            }
            else
            {
                if (AreCredentialsAvailable(emailEntry.Text, _users))
                {
                    mainUserObj.Email = emailEntry.Text;
                    
                    await Navigation.PushAsync(new NewAccountPageUP(mainUserObj, _users));

                    btnNext.IsEnabled = true;
                }
                else
                {
                    await DisplayAlert("Oops!", "This email is already connected to a user", "OK");
                    btnNext.IsEnabled = true;
                }
            }

        }
        bool AreCredentialsAvailable(string email,  ObservableCollection<User> _users)
        {
            if (_users.Count() > 0)
            {
                //her må brukerinformasjon sjekkes opp mot database
                foreach (var _user in _users)
                {
                    if (email == _user.Email)
                    {
                        return false;
                    }
                }
                return true;
            }
            else return true;
        }
    }
}