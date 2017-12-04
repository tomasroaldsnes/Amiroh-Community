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
    public partial class ChooseInspoPage : ContentPage
    {
       
        public ChooseInspoPage()
        {
            InitializeComponent();

            var navigationPage = Application.Current.MainPage as NavigationPage;
            navigationPage.BarBackgroundColor = Color.FromHex("#203E4A");
            navigationPage.BarTextColor = Color.White;

           
        }

        private async void ChooseImage_Clicked(object sender, EventArgs e)
        {

            try
            {
                btnChoose.Text = "Uploading, give me a minute...";
                btnChoose.IsEnabled = false;

                 string url_inspo = "http://138.68.137.52:3000/AmirohAPI/inspos/";
                 HttpClient _client = new HttpClient(new NativeMessageHandler());

                string uploadedInspoURL = "";
                uploadedInspoURL = await ImageUpload.ProfilePictureUploadAsync();


                if (uploadedInspoURL != "")
                {
                    var uId = Guid.NewGuid().ToString();

                    string postdataJson = JsonConvert.SerializeObject(new { URL = uploadedInspoURL, username = MainUser.MainUserID.Username, userId = MainUser.MainUserID.ID, points = 0, uploadId = uId });
                    var postdataString = new StringContent(postdataJson, new UTF8Encoding(), "application/json");

                    
                    var response = _client.PostAsync(url_inspo, postdataString);
                    var responseString = response.Result.Content.ReadAsStringAsync().Result;


                    if (response.Result.IsSuccessStatusCode)
                    {
                        string url_find_upload_inspo = "http://138.68.137.52:3000/AmirohAPI/inspos/uploadId/" + uId;
                        var content = await _client.GetStringAsync(url_find_upload_inspo);
                        var inspoObj = JsonConvert.DeserializeObject<List<Inspo>>(content);

                        await Navigation.PushModalAsync(new EditInspoPage(inspoObj[0]._Id));
                    }
                    else
                    {
                        await DisplayAlert("Upload Error", "I really tried my best here. Promise", "Try harder");
                        await Navigation.PushModalAsync(new EditInspoPage("ERROR"));
                    }
                }
                else
                {
                    bool IsPictureReady = false;
                    while (!IsPictureReady)
                    {
                        if (uploadedInspoURL != "" | uploadedInspoURL != null)
                        {
                            IsPictureReady = true;
                        }
                    }

                    var uId = Guid.NewGuid().ToString();

                    string postdataJson = JsonConvert.SerializeObject(new { URL = uploadedInspoURL, username = MainUser.MainUserID.Username, userId = MainUser.MainUserID.ID, points = 0, uploadId = uId });
                    var postdataString = new StringContent(postdataJson, new UTF8Encoding(), "application/json");


                    var response = _client.PostAsync(url_inspo, postdataString);
                    var responseString = response.Result.Content.ReadAsStringAsync().Result;

                    var r = response.Id.ToString();
                    

                    if (response.Result.IsSuccessStatusCode)
                    {
                        string url_find_upload_inspo = "http://138.68.137.52:3000/AmirohAPI/inspos/uploadId/" + uId;
                        var content = await _client.GetStringAsync(url_find_upload_inspo);
                        var inspoObj = JsonConvert.DeserializeObject<List<Inspo>>(content);

                        await Navigation.PushModalAsync(new EditInspoPage(inspoObj[0]._Id));
                    }
                    else
                    {
                        await DisplayAlert("Upload Error", "I really tried my best here. Promise", "Try harder");
                        await Navigation.PushModalAsync(new EditInspoPage("ERROR"));
                    }
                }

            }
            catch (Exception ex)
            {
                try
                {
                    Insights.Report(ex);
                    await DisplayAlert("Error", "I tried to upload your profile picture, but I failed. Miserably.", "*Takes a deep breath*");
                    await Navigation.PushModalAsync(new EditInspoPage("ERROR"));
                }
                catch
                {
                    await DisplayAlert("Oh God, not again", "I tried to load your profile, but I failed. Horribly.", "*Counting backwards from 10*");
                }
            }



        }
       
    }
}