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
using Amiroh.Profile;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Amiroh.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewAccountPageUP : ContentPage
    {
        
        private ObservableCollection<User> _users;

        User mainUserObj;



        public NewAccountPageUP(User o, ObservableCollection<User> list)
        {
            InitializeComponent();

            var navigationPage = Application.Current.MainPage as NavigationPage;
            navigationPage.BarBackgroundColor = Color.White;
            navigationPage.BarTextColor = Color.White;

            _users = list;
            mainUserObj = o;
        }

      

        private async void Next_Clicked(object sender, EventArgs e)
        {
            btnSignup.IsEnabled = false;
            if (usernameEntry.Text == "" | passwordEntry.Text == "")
            {
                await DisplayAlert("Oops!", "Please type in your username in the field-thingy.", "OK");
                btnSignup.IsEnabled = true;
            }
            else if (passwordEntry.Text.Length < 7 )
            {
                await DisplayAlert("Oops!", "Password must be longer than 7 characters.", "OK");
                btnSignup.IsEnabled = true;
            }
            else
            {
                if (AreCredentialsAvailable(usernameEntry.Text, _users))
                {
                    mainUserObj.Username = usernameEntry.Text;

                    var salt = Crypto.CreateSalt(16);

                    string stringSalt = Convert.ToBase64String(salt);

                    string securePassword = Crypto.EncryptAes(passwordEntry.Text, salt);

                    mainUserObj.Password = securePassword;
                    mainUserObj.Salt = stringSalt;

                    try
                    {
                        string url_create_user = "http://138.68.137.52:3000/AmirohAPI/users";

                        HttpClient _client = new HttpClient(new NativeMessageHandler());

                        string postdataJson = JsonConvert.SerializeObject(new { username = mainUserObj.Username, password = mainUserObj.Password, name = mainUserObj.Name, email = mainUserObj.Email, salt = mainUserObj.Salt });
                        var postdataString = new StringContent(postdataJson, new UTF8Encoding(), "application/json");

                        var response = await _client.PostAsync(url_create_user, postdataString);

                        MainUser.MainUserID.Username = mainUserObj.Username;

                        await Navigation.PushAsync(new EditProfilePicPage());

                        btnSignup.IsEnabled = true;



                    }
                    catch
                    {

                        btnSignup.IsEnabled = true;
                    }

                    
                }
                else
                {
                    await DisplayAlert("Oops!", "This email is already connected to a user", "OK");
                    btnSignup.IsEnabled = true;
                }
            }

        }
        bool AreCredentialsAvailable(string username,  ObservableCollection<User> _users)
        {
            if (_users.Count() > 0)
            {
                //her må brukerinformasjon sjekkes opp mot database
                foreach (var _user in _users)
                {
                    if (username.ToLower() == _user.Username.ToLower())
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