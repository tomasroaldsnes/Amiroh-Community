using Amiroh.Classes;
using ModernHttpClient;
using Newtonsoft.Json;
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
using Amiroh.Controllers;
using Plugin.Connectivity;
using Amiroh.Helpers;
using Microsoft.AppCenter.Analytics;

namespace Amiroh.Login
{
    
    public partial class LoginPage : ContentPage
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

        private const string url_user = "http://138.68.137.52:3000/AmirohAPI/users/username/";

        private HttpClient _client = new HttpClient(new NativeMessageHandler());
        private ObservableCollection<User> _user;

        public LoginPage()
        {
            InitializeComponent();
        }

       


    private async void LoginButton_Clicked(object sender, EventArgs e)
    {

            
           

            try
            {
                    btnLogin.IsEnabled = false;
                //check is device is connected to the internet

                    string url = url_user + usernameEntry.Text;
                    var content = await _client.GetStringAsync(url);
                    var userobj = JsonConvert.DeserializeObject<List<User>>(content);


                    _user = new ObservableCollection<User>(userobj);
                

            }
            catch (Exception ex)
            {
                try
                {
                    Insights.Report(ex);
                    errorLabel.Text = "Connection failed. Are you connected to the internet?";
                    
                    btnLogin.IsEnabled = true;
                }
                catch
                {
                    
                    btnLogin.IsEnabled = true;
                }


            }
            

            try
            {

                var userLogin = new User
                {
                    Username = usernameEntry.Text,
                    Password = passwordEntry.Text
                };

                var isValid = AreCredentialsCorrect(userLogin, _user);
                if (isValid) //isValid
                {
                    SetUserId = _user[0]._Id;
                    SetUsername = _user[0].Username;
                    SetUserDescription = _user[0].ProfileDescription;
                    SetProfilePicture = _user[0].ProfilePicture;

                    MainUser.MainUserID.Username = _user[0].Username;
                    MainUser.MainUserID.ID = _user[0]._Id;
                    MainUser.MainUserID.ProfileDescription = _user[0].ProfileDescription;
                    MainUser.MainUserID.ProfilePicture = _user[0].ProfilePicture;

                    if(_user[0].Notifications.Length > 0)
                    {
                        MainUser.MainUserID.HasNotifications = true;
                    }

                    IsLoggedIn = "yes";
                    Analytics.TrackEvent("User has logged in.");
                    Navigation.InsertPageBefore(new Amiroh.MainPage(), this);
                    await Navigation.PopAsync();
                }
                else
                {
                    errorLabel.Text = "Username or password is incorrect";
                    passwordEntry.Text = string.Empty;
                    
                    btnLogin.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Insights.Report(ex);
                    errorLabel.Text = "Connection failed. Are you connected to the internet?";
                    
                    btnLogin.IsEnabled = true;
                }
                catch
                {
                    await DisplayAlert("Ooops.", "I failed to do what you wanted me to do.", "You are useless.");
                    
                    btnLogin.IsEnabled = true;
                }

            }
        }
        private async void OnSignUpButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewAccountPageName());
        }
        bool AreCredentialsCorrect(User userLogin, ObservableCollection<User> _user)
        {
            //check user credentials against DB

            string decryptedPassword = Crypto.DecryptAes(_user[0].Password, Convert.FromBase64String(_user[0].Salt));

            return userLogin.Username == _user[0].Username && userLogin.Password == decryptedPassword;
            
            
            
        }

        
    }
}