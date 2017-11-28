using Amiroh.Classes;
using ModernHttpClient;
using Newtonsoft.Json;
using Plugin.Connectivity;
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

namespace Amiroh
{


    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddInspoPage : ContentPage
    {
        private string _URL = "";
        private string url_create_inspo = "http://138.68.137.52:3000/AmirohAPI/inspos/";
        private HttpClient _client = new HttpClient(new NativeMessageHandler());
        private List<string> Tags = new List<string>();


        public AddInspoPage()
        {
            InitializeComponent();

            var navigationPage = Application.Current.MainPage as NavigationPage;
            navigationPage.BarBackgroundColor = Color.FromHex("#203E4A");
            navigationPage.BarTextColor = Color.White;

            tagEyes.GestureRecognizers.Add(new TapGestureRecognizer(EyesTag_Tapped));
            tagLips.GestureRecognizers.Add(new TapGestureRecognizer(LipsTag_Tapped));
            tagEyebrows.GestureRecognizers.Add(new TapGestureRecognizer(EyebrowsTag_Tapped));
            tagContouring.GestureRecognizers.Add(new TapGestureRecognizer(ContouringTag_Tapped));
            tagDay.GestureRecognizers.Add(new TapGestureRecognizer(DayTag_Tapped));
            tagNight.GestureRecognizers.Add(new TapGestureRecognizer(NightTag_Tapped));
            
        }

        private async void EyesTag_Tapped(View arg1, object arg2)
        {
            Tags.Add("Eyes");
            tagEyes.FontFamily = "Lato-Bold.ttf#Lato-Bold";
        }

        private async void LipsTag_Tapped(View arg1, object arg2)
        {
            Tags.Add("Lips");
            tagLips.FontFamily = "Lato-Bold.ttf#Lato-Bold";
        }

        private async void EyebrowsTag_Tapped(View arg1, object arg2)
        {
            Tags.Add("Eyebrows");
            tagEyebrows.FontFamily = "Lato-Bold.ttf#Lato-Bold";
        }

        private async void ContouringTag_Tapped(View arg1, object arg2)
        {
            Tags.Add("Contouring");
            tagContouring.FontFamily = "Lato-Bold.ttf#Lato-Bold";
        }

        private async void DayTag_Tapped(View arg1, object arg2)
        {
            Tags.Add("Day");
            tagDay.FontFamily = "Lato-Bold.ttf#Lato-Bold";
        }

        private async void NightTag_Tapped(View arg1, object arg2)
        {
            Tags.Add("Night");
            tagNight.FontFamily = "Lato-Bold.ttf#Lato-Bold";
        }

        private async void AddImageButton_Clicked(object sender, EventArgs e)
        {
            
                btnUpload.Text = "Uploading inspo...";
                _URL = await Classes.ImageUpload.InspoUploadAsync();

                btnUpload.IsEnabled = true;
                btnUpload.Text = "Upload Inspo";

        }

        private async void UploadButton_Clicked(object sender, EventArgs e)
        {
            
                
                    string postdataJson = JsonConvert.SerializeObject(new {  description = descriptionEntry.Text, URL = _URL, username = MainUser.MainUserID.Username,  tags = Tags.ToArray<string>() }); 
                    var postdataString = new StringContent(postdataJson, new UTF8Encoding(), "application/json");

                    var response = _client.PostAsync(url_create_inspo, postdataString);
                    var responseString = response.Result.Content.ReadAsStringAsync().Result;


                    if (response.Result.IsSuccessStatusCode)
                    {
                        await DisplayAlert("Success!", "Your Inspo was uploaded.", "Woop Woop!");

                
                    }
                    else
                    {
                        await DisplayAlert("Upload Error", "No success", "OK");
                    }
              
            
            
        }

        private void Switch_Toggled_1(object sender, ToggledEventArgs e)
        {
            string t = "Tag 1";
            Tags.Add(t);           
            
        }
        private void Switch_Toggled_2(object sender, ToggledEventArgs e)
        {
            string t = "Tag 2";
            Tags.Add(t);
        }
        private void Switch_Toggled_3(object sender, ToggledEventArgs e)
        {
            string t = "Tag 3";
            Tags.Add(t);
        }
        private void Switch_Toggled_4(object sender, ToggledEventArgs e)
        {
            string t = "Tag 4";
            Tags.Add(t);
        }
        private void Switch_Toggled_5(object sender, ToggledEventArgs e)
        {
            string t = "Tag 5";
            Tags.Add(t);
        }
        private void Switch_Toggled_6(object sender, ToggledEventArgs e)
        {
            string t = "Tag 6";
            Tags.Add(t);
        }

        private void descriptionEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length > 250)
            {
                descriptionEntry.Text = descriptionEntry.Text.Remove(descriptionEntry.Text.Length - 1);

            }
        }
    }
}