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

namespace Amiroh.Profile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditProfilePicPage : ContentPage
    {
       
        public EditProfilePicPage()
        {
            InitializeComponent();

            var navigationPage = Application.Current.MainPage as NavigationPage;
            navigationPage.BarBackgroundColor = Color.FromHex("#203E4A");
            navigationPage.BarTextColor = Color.White;

           
        }

        private async void Later_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new EditDescriptionPage(true));
        }

        private async void ChooseImage_Clicked(object sender, EventArgs e)
        {

            try
            {
                btnChoose.Text = "Uploading, give me a minute...";
                btnChoose.IsEnabled = false;

                 string url_user = "http://138.68.137.52:3000/AmirohAPI/users/username/";
                 HttpClient _client = new HttpClient(new NativeMessageHandler());

                string profilePictureURL = "";
                profilePictureURL = await ImageUpload.ProfilePictureUploadAsync();


                if (profilePictureURL != "")
                {
                    string postdataJson = JsonConvert.SerializeObject(new { profilePicture = profilePictureURL });
                    var postdataString = new StringContent(postdataJson, new UTF8Encoding(), "application/json");

                    string new_url = url_user + MainUser.MainUserID.Username;
                    var response = _client.PutAsync(new_url, postdataString);
                    var responseString = response.Result.Content.ReadAsStringAsync().Result;


                    if (response.Result.IsSuccessStatusCode)
                    {

                        await Navigation.PushModalAsync(new EditDescriptionPage(true));
                    }
                    else
                    {
                        await DisplayAlert("Upload Error", "I really tried my best here. Promise", "Try harder");
                        await Navigation.PushModalAsync(new EditDescriptionPage(true));
                    }
                }
                else
                {
                    bool IsPictureReady = false;
                    while (!IsPictureReady)
                    {
                        if (profilePictureURL != "" | profilePictureURL != null)
                        {
                            IsPictureReady = true;
                        }
                    }
                    string postdataJson = JsonConvert.SerializeObject(new { profilePicture = profilePictureURL });
                    var postdataString = new StringContent(postdataJson, new UTF8Encoding(), "application/json");

                    string new_url = url_user + MainUser.MainUserID.Username;
                    var response = _client.PutAsync(new_url, postdataString);
                    var responseString = response.Result.Content.ReadAsStringAsync().Result;


                    if (response.Result.IsSuccessStatusCode)
                    {
                        await Navigation.PushModalAsync(new EditDescriptionPage(true));
                    }
                    else
                    {
                        await DisplayAlert("Upload Error", "I really tried my best here. Promise", "Try harder");
                        await Navigation.PushModalAsync(new EditDescriptionPage(true));
                    }

                }

            }
            catch (Exception ex)
            {
                try
                {
                    Insights.Report(ex);
                    await DisplayAlert("Error", "I tried to upload your profile picture, but I failed. Miserably.", "*Takes a deep breath*");
                    await Navigation.PushModalAsync(new EditDescriptionPage(true));
                }
                catch
                {
                    await DisplayAlert("Oh God, not again", "I tried to load your profile, but I failed. Horribly.", "*Counting backwards from 10*");
                }
            }



        }
       
    }
}