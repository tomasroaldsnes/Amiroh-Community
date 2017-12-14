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
    public partial class NewAccountPageName : ContentPage
    {
        private const string url_user = "http://138.68.137.52:3000/AmirohAPI/users/";

        private HttpClient _client = new HttpClient(new NativeMessageHandler());
        private ObservableCollection<User> _users;

        User mainUserObj = new User();



        public NewAccountPageName()
        {
            InitializeComponent();

            var navigationPage = Application.Current.MainPage as NavigationPage;
            navigationPage.BarBackgroundColor = Color.White;
            navigationPage.BarTextColor = Color.Black;
        }

        

        protected override async void OnAppearing()
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

        private async void Next_Clicked(object sender, EventArgs e)
        {
            btnNext.IsEnabled = false;
            if (nameEntry.Text.Length < 3)
            {
                await DisplayAlert("Oops!", "Name has to be longer than 3 characters.", "Fine...");
                btnNext.IsEnabled = true;
            }
            else
            {
                mainUserObj.Name = nameEntry.Text;
                await Navigation.PushAsync(new NewAccountPageEmail(mainUserObj, _users));

                btnNext.IsEnabled = true;
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