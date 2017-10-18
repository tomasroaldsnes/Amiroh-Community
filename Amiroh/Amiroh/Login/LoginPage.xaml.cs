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

namespace Amiroh.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private const string url_user = "http://192.168.1.7:3050/AmirohAPI/users/username/";

        private HttpClient _client = new HttpClient(new NativeMessageHandler());
        private ObservableCollection<User> _user;
        
       

        public LoginPage()
        {
            InitializeComponent();
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            this.IsBusy = true;
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    //check is device is connected to the internet

                    string url = url_user + usernameEntry.Text;
                    var content = await _client.GetStringAsync(url);
                    var userobj = JsonConvert.DeserializeObject<List<User>>(content);


                    _user = new ObservableCollection<User>(userobj);
                }
                else
                {
                    await DisplayAlert("Network Error", "Are you connected to the internet?", "OK");
                }

            }
            catch(Exception ex) {

                Insights.Report(ex);
                errorLabel.Text = "Connection failed. Are you connected to the internet?";

            }
            finally
            {
                this.IsBusy = false;
            }

            var userLogin = new User
            {
                Username = usernameEntry.Text,
                Password = passwordEntry.Text
            };

            var isValid = AreCredentialsCorrect(userLogin, _user);
            if (isValid)
            {

                MainUser.MainUserID.Username = _user[0].Username;
                MainUser.MainUserID.ProfileDescription = _user[0].ProfileDescription;
                MainUser.MainUserID.ProfilePicture = _user[0].ProfilePicture;
                App.IsUserLoggedIn = true;
                Navigation.InsertPageBefore(new Amiroh.MainPage(), this);
                await Navigation.PopAsync();
                //errorLabel.Text = "This is username: " + MainUser.MainUserID.USERNAME;
            }
            else
            {
                errorLabel.Text = "Username or password is incorrect";
                passwordEntry.Text = string.Empty;
            }

        }
        private async void OnSignUpButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewAccountPage());
        }
        bool AreCredentialsCorrect(User userLogin, ObservableCollection<User> _user)
        {
            //check user credentials against DB

            return userLogin.Username == _user[0].Username && userLogin.Password == _user[0].Password;
            
        }

        
    }
}