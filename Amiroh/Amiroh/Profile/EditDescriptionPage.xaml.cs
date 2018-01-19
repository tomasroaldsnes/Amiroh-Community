using Amiroh.Classes;
using Amiroh.Controllers;
using Amiroh.Login;
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

namespace Amiroh.Profile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditDescriptionPage : ContentPage
    {
        private bool _firstTime;
        public EditDescriptionPage(bool FirstTime)
        {
            InitializeComponent();

            var navigationPage = Application.Current.MainPage as NavigationPage;
            navigationPage.BarBackgroundColor = Color.White;
            navigationPage.BarTextColor = Color.Black;

            _firstTime = FirstTime;

        }

        protected async override void OnAppearing()
        {
            if (_firstTime)
            {
                string url_user = "http://138.68.137.52:3000/AmirohAPI/users/username/" + MainUser.MainUserID.Username;
                HttpClient _client = new HttpClient(new NativeMessageHandler());

                var content = await _client.GetStringAsync(url_user);
                var userobj = JsonConvert.DeserializeObject<List<User>>(content);

                MainUser.MainUserID.ID = userobj[0]._Id;

            }
        }

        private async void Later_Clicked(object sender, EventArgs e)
        {
            if(_firstTime)
                MainUser.MainUserID.ProfileDescription = "Write a catchy profile description! You can change it at any time from Settings => Change Profile Description.";

            if (_firstTime)
            {
                await Navigation.PushAsync(new TutorialPage());
            }
            else
                await Navigation.PushAsync(new MainPage());
        }

        private async void Continue_Clicked(object sender, EventArgs e)
        {
            btnContinue.IsEnabled = false;
            lblLater.IsVisible = false;
            string url_user = "http://138.68.137.52:3000/AmirohAPI/users/username/" + MainUser.MainUserID.Username;
            HttpClient _client = new HttpClient(new NativeMessageHandler());

            string postdataJson = JsonConvert.SerializeObject(new { profileDescription = entryDescription.Text });
            var postdataString = new StringContent(postdataJson, new UTF8Encoding(), "application/json");

            MainUser.MainUserID.ProfileDescription = entryDescription.Text;

            var response = await _client.PutAsync(url_user, postdataString);

           

            btnContinue.IsEnabled = true;
            if (_firstTime)
            {
                await Navigation.PushAsync(new TutorialPage());
            }
            else
                await Navigation.PushAsync(new MainPage());

        }

        private void Editor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length > 150)
            {
                entryDescription.Text = entryDescription.Text.Remove(entryDescription.Text.Length - 1);

            }
        }

    }




}