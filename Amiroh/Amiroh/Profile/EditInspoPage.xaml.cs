using Amiroh.Classes;
using Amiroh.Controllers;
using Microsoft.AppCenter.Analytics;
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
    public partial class EditInspoPage : ContentPage
    {
        private string _id;
        private List<string> Tags = new List<string>();

        private int TagCounter = 0;

        private bool IsEyesTapped = false;
        private bool IsLipsTapped = false;
        private bool IsEyebrowsTapped = false;
        private bool IsContouringTapped = false;
        private bool IsDayTapped = false;
        private bool IsNightTapped = false;

        public EditInspoPage(string id)
        {
            InitializeComponent();
            _id = id;

            var navigationPage = Application.Current.MainPage as NavigationPage;
            navigationPage.BarBackgroundColor = Color.White;
            navigationPage.BarTextColor = Color.Black;

            tagEyes.GestureRecognizers.Add(new TapGestureRecognizer(EyesTag_Tapped));
            tagLips.GestureRecognizers.Add(new TapGestureRecognizer(LipsTag_Tapped));
            tagEyebrows.GestureRecognizers.Add(new TapGestureRecognizer(EyebrowsTag_Tapped));
            tagContouring.GestureRecognizers.Add(new TapGestureRecognizer(ContouringTag_Tapped));
            tagDay.GestureRecognizers.Add(new TapGestureRecognizer(DayTag_Tapped));
            tagNight.GestureRecognizers.Add(new TapGestureRecognizer(NightTag_Tapped));


        }

        private async void EyesTag_Tapped(View arg1, object arg2)
        {
            if (!IsEyesTapped)
            {
                if(TagCounter < 3){

                    Tags.Add("Eyes");
                    tagEyes.FontFamily = "Lato-Bold.ttf#Lato-Bold";
                    IsEyesTapped = true;
                    TagCounter++;
                }

            }
            else
            {
                tagEyes.FontFamily = "Lato-Light.ttf#Lato-Light";
                Tags.Remove("Eyes");
                IsEyesTapped = false;
                TagCounter--;
            }
        }

        private async void LipsTag_Tapped(View arg1, object arg2)
        {
            if (!IsLipsTapped)
            {
                if (TagCounter < 3)
                {
                    Tags.Add("Lips");
                    tagLips.FontFamily = "Lato-Bold.ttf#Lato-Bold";
                    IsLipsTapped = true;
                    TagCounter++;
                }
            }
            else
            {
                tagLips.FontFamily = "Lato-Light.ttf#Lato-Light";
                Tags.Remove("Lips");
                IsLipsTapped = false;
                TagCounter--;
            }
        }

        private async void EyebrowsTag_Tapped(View arg1, object arg2)
        {
            if (!IsEyebrowsTapped)
            {
                if (TagCounter < 3)
                {
                    Tags.Add("Eyebrows");
                    tagEyebrows.FontFamily = "Lato-Bold.ttf#Lato-Bold";
                    IsEyebrowsTapped = true;
                    TagCounter++;
                }
            }
            else
            {
                tagEyebrows.FontFamily = "Lato-Light.ttf#Lato-Light";
                Tags.Remove("Eyebrows");
                IsEyebrowsTapped = false;
                TagCounter--;
            }
        }

        private async void ContouringTag_Tapped(View arg1, object arg2)
        {
            if (!IsContouringTapped)
            {
                if (TagCounter < 3)
                {
                    Tags.Add("Contouring");
                    tagContouring.FontFamily = "Lato-Bold.ttf#Lato-Bold";
                    IsContouringTapped = true;
                    TagCounter++;
                }
            }
            else
            {
                tagContouring.FontFamily = "Lato-Light.ttf#Lato-Light";
                Tags.Remove("Contouring");
                IsContouringTapped = false;
                TagCounter--;
            }
        }

        private async void DayTag_Tapped(View arg1, object arg2)
        {
            if (!IsDayTapped)
            {
                if (TagCounter < 3)
                {
                    Tags.Add("Day");
                    tagDay.FontFamily = "Lato-Bold.ttf#Lato-Bold";
                    IsDayTapped = true;
                    TagCounter++;
                }

            }
            else
            {
                tagDay.FontFamily = "Lato-Light.ttf#Lato-Light";
                Tags.Remove("Day");
                IsDayTapped = false;
                TagCounter--;
            }
        }

        private async void NightTag_Tapped(View arg1, object arg2)
        {
            if (!IsNightTapped)
            {
                if (TagCounter < 3)
                {
                    Tags.Add("Night");
                    tagNight.FontFamily = "Lato-Bold.ttf#Lato-Bold";
                    IsNightTapped = true;
                    TagCounter++;
                }
            }
            else
            {
                tagNight.FontFamily = "Lato-Light.ttf#Lato-Light";
                Tags.Remove("Night");
                IsNightTapped = false;
                TagCounter--;
            }
        }



        private async void Publish_Clicked(object sender, EventArgs e)
        {
            btnPublish.IsEnabled = false;
            string url_edit_inspo = "http://138.68.137.52:3000/AmirohAPI/inspos/" + _id;
            HttpClient _client = new HttpClient(new NativeMessageHandler());

            string postdataJson = JsonConvert.SerializeObject(new { description = descriptionEntry.Text, tags = Tags.ToArray() }); //<-------
            var postdataString = new StringContent(postdataJson, new UTF8Encoding(), "application/json");


            var response = _client.PutAsync(url_edit_inspo, postdataString);
            var responseString = response.Result.Content.ReadAsStringAsync().Result;


            if (response.Result.IsSuccessStatusCode)
            {
                Analytics.TrackEvent("User has published an Inspo.");
                await Task.Delay(3000);
                Navigation.InsertPageBefore(new MainPage(), this);
                await Navigation.PopAsync();
                btnPublish.IsEnabled = true;
            }
            else
            {
                btnPublish.IsEnabled = true;
            }
        }

        private void Editor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length > 150)
            {
                descriptionEntry.Text = descriptionEntry.Text.Remove(descriptionEntry.Text.Length - 1);

            }
        }

    }




}