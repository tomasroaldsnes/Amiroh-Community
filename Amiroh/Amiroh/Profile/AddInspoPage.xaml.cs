﻿using Amiroh.Classes;
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
            

        }

        private async void AddImageButton_Clicked(object sender, EventArgs e)
        {
            this.IsBusy = true;
            _URL = await Classes.ImageUpload.InspoUploadAsync();
            this.IsBusy = false;
            btnUpload.IsEnabled = true;
            // _URL = newInspoUrl;
        }

        private async void UploadButton_Clicked(object sender, EventArgs e)
        {
            
                try
                {
                    string postdataJson = JsonConvert.SerializeObject(new {  description = descriptionEntry.Text, URL = _URL, username = MainUser.MainUserID.Username, points = 0 }); /*, tags = Tags.ToArray<string>()*/
                    var postdataString = new StringContent(postdataJson, new UTF8Encoding(), "application/json");

                    var response = _client.PostAsync(url_create_inspo, postdataString);
                    var responseString = response.Result.Content.ReadAsStringAsync().Result;


                    if (response.Result.IsSuccessStatusCode)
                    {
                        //var inspo_return = JsonConvert.DeserializeObject<User>(responseString);
                        Navigation.InsertPageBefore(new Amiroh.MainPage(), this);
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        await DisplayAlert("Upload Error", "No success", "OK");
                    }
                }
                catch
                {
                    await DisplayAlert("Upload Error 2", "No success", "OK");
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
    }
}