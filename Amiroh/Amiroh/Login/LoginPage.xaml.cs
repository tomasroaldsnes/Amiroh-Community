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
    [assembly: ExportRenderer(typeof(Xamarin.Forms.Button), typeof(GenericButtonRenderer))]

    

    public partial class LoginPage : ContentPage
    {
        private const string url_user = "http://138.68.137.52:3000/AmirohAPI/users/username/";

        private HttpClient _client = new HttpClient(new NativeMessageHandler());
        private ObservableCollection<User> _user;

        public class GenericButtonRenderer : Xamarin.Forms.Button
        {
        }


        public LoginPage()
        {
            InitializeComponent();
        }

       


    private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            this.IsBusy = true;
            try
            {
                
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
                }
                catch
                {
                   //
                }


            }
            finally
            {
                this.IsBusy = false;
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

                    MainUser.MainUserID.Username = _user[0].Username;
                    MainUser.MainUserID.ID = _user[0]._Id;
                    MainUser.MainUserID.ProfileDescription = _user[0].ProfileDescription;
                    MainUser.MainUserID.ProfilePicture = _user[0].ProfilePicture;

                    if(_user[0].Notifications.Length > 0)
                    {
                        MainUser.MainUserID.HasNotifications = true;
                    }

                    var app = Application.Current as App;
                    app.IsUserLoggedIn = false;

                    Navigation.InsertPageBefore(new Amiroh.MainPage(), this);
                    await Navigation.PopAsync();
                }
                else
                {
                    errorLabel.Text = "Username or password is incorrect";
                    passwordEntry.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Insights.Report(ex);
                    errorLabel.Text = "Connection failed. Are you connected to the internet?";
                }
                catch
                {
                    await DisplayAlert("Ooops.", "I failed to do what you wanted me to do.", "You are useless.");
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