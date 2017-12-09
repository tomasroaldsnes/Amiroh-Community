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
    public partial class EditInspoPage : ContentPage
    {
        private string _id;
        private List<string> Tags = new List<string>();

        public EditInspoPage(string id)
        {
            InitializeComponent();
            _id = id;

            var navigationPage = Application.Current.MainPage as NavigationPage;
            navigationPage.BarBackgroundColor = Color.FromHex("White");
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

        

        private async void Publish_Clicked(object sender, EventArgs e)
        {
            string url_edit_inspo = "http://138.68.137.52:3000/AmirohAPI/inspos/" + _id;
            HttpClient _client = new HttpClient(new NativeMessageHandler());

            string postdataJson = JsonConvert.SerializeObject(new {  title = titleEntry.Text, description = descriptionEntry.Text, tags = Tags.ToArray() }); //<-------
            var postdataString = new StringContent(postdataJson, new UTF8Encoding(), "application/json");
            

            var response = await _client.PutAsync(url_edit_inspo, postdataString);

            await Navigation.PushModalAsync(new MainPage());

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