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
    public partial class NewAccountPage : ContentPage
    {
        private const string url_user = "http://10.5.50.138:3050/AmirohAPI/users/";
        private const string url_create_user = "http://10.5.50.138:3050/AmirohAPI/users";

        private HttpClient _client = new HttpClient(new NativeMessageHandler());
        private ObservableCollection<User> _users;
        private string ProfilePictureURL = "";



        public NewAccountPage()
        {
            InitializeComponent();
        }

        

        protected override async void OnAppearing()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                try
                {

                    var content = await _client.GetStringAsync(url_user);
                    var userobj = JsonConvert.DeserializeObject<List<User>>(content);


                    _users = new ObservableCollection<User>(userobj);

                }
                catch (Exception ex)
                {
                    try
                    {
                        Insights.Report(ex);
                    }
                    catch
                    {
                        await DisplayAlert("Heeeelp!", "Something went wrong. I have failed. I am useless.", "Yes, you are.");
                    }
                }
            }
        }

        private async void SignupButton_Clicked(object sender, EventArgs e)
        {

            var salt = Crypto.CreateSalt(16);
            
            string stringSalt = Convert.ToBase64String(salt);

            string securePassword = Crypto.EncryptAes(passwordEntry.Text, salt);

            var user = new User
            {
                Username = usernameEntry.Text,
                Password = securePassword, //securepswd
                Name = nameEntry.Text,
                ProfileDescription = profileDescriptionEntry.Text,
                ProfilePicture = ProfilePictureURL,
                Email = emailEntry.Text,
                Salt = stringSalt
                
            };

            var isValid = AreCredentialsAvailable(user, _users);
            if (isValid)
            {
                try
                {

                    User user_return;
                    //sjekk plassering av stuff
                    string postdataJson = JsonConvert.SerializeObject(new { username = user.Username, password = user.Password, name = user.Name, profileDescription = profileDescriptionEntry.Text, profilePicture = "", email = user.Email, salt = user.Salt });
                    var postdataString = new StringContent(postdataJson, new UTF8Encoding(), "application/json");

                    var response = _client.PostAsync(url_create_user, postdataString);
                    var responseString = response.Result.Content.ReadAsStringAsync().Result;


                    if (response.Result.IsSuccessStatusCode)
                    {

                        user_return = JsonConvert.DeserializeObject<User>(responseString);

                        MainUser.MainUserID.Username = user.Username;
                        



                        Navigation.InsertPageBefore(new LoginPage(), this);
                        await Navigation.PopAsync();

                    }

                }
                catch
                {

                    whatsmyid.Text = "nope2";
                }

               
                
            }
            else
            {
                await DisplayAlert("Error!", "Username not available or email already in use", "OK");
                passwordEntry.Text = string.Empty;
            }

        }
        bool AreCredentialsAvailable(User user, ObservableCollection<User> _users)
        {

            //her må brukerinformasjon sjekkes opp mot database
            foreach (var _user in _users)
            {
                if(user.Username == _user.Username || user.Email == _user.Email)
                {
                    return false;
                }
            }
            return true;
        }
    }
}