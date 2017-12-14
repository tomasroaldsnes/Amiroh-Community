﻿using ModernHttpClient;
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
using ImageCircle.Forms.Plugin.Abstractions;
using ImageCircle;
using Amiroh.Classes;
using Amiroh.Profile;
using Amiroh.Login;
using Amiroh.Feed;
using Plugin.Connectivity;
using FFImageLoading.Forms;

namespace Amiroh
{
    
    
    
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        
        private string url_photo = "http://138.68.137.52:3000/AmirohAPI/inspos/user/";
        private string url_user_collection = "http://138.68.137.52:3000/AmirohAPI/users/collection/";
      
        private const string url_user = "http://138.68.137.52:3000/AmirohAPI/users/username/";
        private HttpClient _client = new HttpClient(new NativeMessageHandler());
       // private ObservableCollection<Inspo> _profileImages;
        //private ObservableCollection<User> _users;
        private ObservableCollection<Inspo> _userPhotos;
        private ObservableCollection<Inspo> _userCollection;

        private List<Notification> notificationList = new List<Notification>();

        private bool IsInspoLoaded = false;
        private bool NewInspoUploaded = false;


        public ProfilePage()
        {
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);

            

            if (MainUser.MainUserID.HasNotifications == true)
            {
                btnNotifications.Source = "notifications.png";
            }

            this.BindingContext = MainUser.MainUserID;
            lblUsername.Text = lblUsername.Text.ToUpper();

            imgProfilePicture.GestureRecognizers.Add(new TapGestureRecognizer(ProfileImageTap));

        }

        private async void UserInspoImageTapped(Inspo i)
        {
             await Navigation.PushAsync(new ImagePage(i));
        }





        protected async override void OnAppearing()
        {
            
            try
            {
                
             var content_p = await _client.GetStringAsync(url_photo + MainUser.MainUserID.Username);
             var pI = JsonConvert.DeserializeObject<List<Inspo>>(content_p);

             _userPhotos = new ObservableCollection<Inspo>(pI);

                //set the number of points to the correct amount
                int userPoints = 0;
                
                foreach (var image in _userPhotos)
                {
                    userPoints += image.Points;

                   
                }
                numberOfPoints.Text = userPoints.ToString();
  
            }
            catch (Exception e)
            {
                try
                {
                    Insights.Report(e);
                    await DisplayAlert("Useless", "I tried to load your profile, but I failed. Horribly.", "Be better");
                }
                catch
                {
                    await DisplayAlert("Aww, maaaaan...", "I tried to load your profile, but I failed. Horribly.", "Be better");
                }
            }
            finally
            {
                this.IsBusy = false;
            }

            //create grid view of photos from user's image collection

            try
            {
                if (!IsInspoLoaded)
                {
                    GridCreation();
                }
             
                
            }
            catch(Exception e)
            {
                try
                {
                    Insights.Report(e);
                    await DisplayAlert("Useless", "I tried to load your profile, but I failed. Horribly.", "Be better");
                }
                catch
                {
                    await DisplayAlert("Aww, maaaaan...", "I tried to load your profile, but I failed. Horribly.", "Be better");
                }
            }
           base.OnAppearing();
        }

        private async void ProfileImageTap(View arg1, object arg2)
        {

            try
            {
                await Navigation.PushAsync(new EditProfilePicPage());
            }
            catch(Exception e)
            {
                Insights.Report(e);
                await DisplayAlert("Dammit", "I'm not ready to upload your profile picture. Like, emotionally, you know?", "*Takes a deep breath*");
            }
           
        }

        private async void AddInspo_Clicked(object sender, EventArgs e)
        {
            try
            {
                NewInspoUploaded = true;
                await Navigation.PushAsync(new ChooseInspoPage());
                

            }
            catch (Exception ex)
            {
                try
                {
                    Insights.Report(ex);
                    await DisplayAlert("Dammit", "I'm not ready to upload an inspo. Like, emotionally, you know?", "*Takes a deep breath*");
                }
                catch
                {
                    await DisplayAlert("Dammit", "I'm not ready to upload an inspo. Like, emotionally, you know?", "*Counting backwards from 10*");
                }
            }
        }

        public void GridCreation()
        {
            try
            {

                if (!IsInspoLoaded)
                {
                    for (int MyCount = 0; MyCount < _userPhotos.Count()+2; MyCount++)
                    {

                        ProfileGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(150, GridUnitType.Absolute) });

                    }

                }
                else if(NewInspoUploaded)
                {
                    ProfileGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(150, GridUnitType.Absolute) });
                    NewInspoUploaded = false;
                }

                //set row and columns => column to 1 since AddInspoBtn should always be at position 0,0
                int row = 4;
                int column = 0;



                for (int i = 0; i < _userPhotos.Count(); i++)
                {


                    if (column < 1)
                    {
                        var userInspoObject = new CachedImage { Source = _userPhotos[i].URL, WidthRequest = 145, HeightRequest = 145, DownsampleHeight = 145, DownsampleWidth = 145, VerticalOptions = LayoutOptions.StartAndExpand };

                        Inspo obj = _userPhotos[i];
                        var tappedInspo = new TapGestureRecognizer();
                        tappedInspo.Tapped += (s, e) =>
                        {
                            UserInspoImageTapped(obj);
                        };
                        userInspoObject.GestureRecognizers.Add(tappedInspo);

                        ProfileGrid.Children.Add(userInspoObject, 0, 5, row, row+1);
                        column++;
                    }
                    else if (column == 1)
                    {
                        var userInspoObject = new CachedImage { Source = _userPhotos[i].URL, WidthRequest = 145, HeightRequest = 145, DownsampleHeight = 145, DownsampleWidth = 145, VerticalOptions = LayoutOptions.StartAndExpand };

                        Inspo obj = _userPhotos[i];
                        var tappedInspo = new TapGestureRecognizer();
                        tappedInspo.Tapped += (s, e) =>
                        {
                            UserInspoImageTapped(obj);
                        };
                        userInspoObject.GestureRecognizers.Add(tappedInspo);

                        ProfileGrid.Children.Add(userInspoObject, 5, 10, row, row+1);
                        column = 0;
                        row++;
                    }
                    else DisplayAlert("Error", "Something went wrong with loading userphotos", "OK");

                    


                }
                //Inspos are now loaded and grid does not need to update gridrows
                IsInspoLoaded = true;
            }
            catch (Exception ex)
            {
                try
                {
                    Insights.Report(ex);
                    DisplayAlert("Dammit", "I'm not ready to upload an inspo. Like, emotionally, you know?", "*Takes a deep breath*");
                }
                catch
                {
                    DisplayAlert("Dammit", "I'm not ready to upload an inspo. Like, emotionally, you know?", "*Counting backwards from 10*");
                }
            }

        }

        private async void Notifications_Tapped(object sender, EventArgs e)
        {
            if (MainUser.MainUserID.HasNotifications == true)
            {
                MainUser.MainUserID.HasNotifications = false;
            }
            btnNotifications.Source = "notificationnull.png";
            await Navigation.PushAsync(new NotificationPage());
            
        }

        private void Settings_Tapped(object sender, EventArgs e)
        {
            //
        }
        private void InspoGrid_Clicked(object sender, EventArgs e)
        {
            btnInspoGrid.FontFamily = "Lato-Bold.ttf#Lato-Bold";
            btnCollection.FontFamily = "Lato-Light.ttf#Lato-Light";



            int row = 4;
            int column = 0;

            for (int i = _userCollection.Count()+6; i > 6; i--)
            {
                ProfileGrid.Children.RemoveAt(i);
            }

            for (int i = 0; i < _userPhotos.Count(); i++)
            {


                if (column < 1)
                {
                    var userInspoObject = new CachedImage { Source = _userPhotos[i].URL, WidthRequest = 145, HeightRequest = 145, DownsampleHeight = 145, DownsampleWidth = 145, VerticalOptions = LayoutOptions.StartAndExpand };

                    Inspo obj = _userPhotos[i];
                    var tappedInspo = new TapGestureRecognizer();
                    tappedInspo.Tapped += (s, ex) =>
                    {
                        UserInspoImageTapped(obj);
                    };
                    userInspoObject.GestureRecognizers.Add(tappedInspo);

                    ProfileGrid.Children.Add(userInspoObject, 0, 5, row, row + 1);
                    column++;
                }
                else if (column == 1)
                {
                    var userInspoObject = new CachedImage { Source = _userPhotos[i].URL, WidthRequest = 145, HeightRequest = 145, DownsampleHeight = 145, DownsampleWidth = 145, VerticalOptions = LayoutOptions.StartAndExpand };

                    Inspo obj = _userPhotos[i];
                    var tappedInspo = new TapGestureRecognizer();
                    tappedInspo.Tapped += (s, ex) =>
                    {
                        UserInspoImageTapped(obj);
                    };
                    userInspoObject.GestureRecognizers.Add(tappedInspo);

                    ProfileGrid.Children.Add(userInspoObject, 5, 10, row, row + 1);
                    column = 0;
                    row++;
                }
                else DisplayAlert("Error", "Something went wrong with loading userphotos", "OK");




            }
        }
        private void ProductPage_Clicked(object sender, EventArgs e)
        {
            //
        }
        private async void Collection_Clicked(object sender, EventArgs e)
        {
            for (int i = _userPhotos.Count()+6; i > 6; i--)
            {
                ProfileGrid.Children.RemoveAt(i);
            }

            var content_p = await _client.GetStringAsync(url_user_collection + MainUser.MainUserID.ID);
            var user_obj = JsonConvert.DeserializeObject<List<Inspo>>(content_p);

            _userCollection = new ObservableCollection<Inspo>(user_obj);

            btnInspoGrid.FontFamily = "Lato-Light.ttf#Lato-Light";
            btnCollection.FontFamily = "Lato-Bold.ttf#Lato-Bold";

            int row = 4;
            int column = 0;

            for (int i = 0; i <_userCollection.Count() ; i++)
            {
                if (column < 1)
                {
                    var userInspoObject = new CachedImage { Source = _userCollection[i].URL, WidthRequest = 145, HeightRequest = 145, DownsampleHeight = 145, DownsampleWidth = 145, VerticalOptions = LayoutOptions.StartAndExpand };

                    Inspo obj = _userPhotos[i];
                    var tappedInspo = new TapGestureRecognizer();
                    tappedInspo.Tapped += (s, ex) =>
                    {
                        UserInspoImageTapped(obj);
                    };
                    userInspoObject.GestureRecognizers.Add(tappedInspo);

                    ProfileGrid.Children.Add(userInspoObject, 0, 5, row, row + 1);
                    column++;
                }
                else if (column == 1)
                {
                    var userInspoObject = new CachedImage { Source = _userCollection[i].URL, WidthRequest = 145, HeightRequest = 145, DownsampleHeight = 145, DownsampleWidth = 145, VerticalOptions = LayoutOptions.StartAndExpand };

                    Inspo obj = _userPhotos[i];
                    var tappedInspo = new TapGestureRecognizer();
                    tappedInspo.Tapped += (s, ex) =>
                    {
                        UserInspoImageTapped(obj);
                    };
                    userInspoObject.GestureRecognizers.Add(tappedInspo);

                    ProfileGrid.Children.Add(userInspoObject, 5, 10, row, row + 1);
                    column = 0;
                    row++;
                }
                else
                {
                    await DisplayAlert("Error", "Something went wrong with loading Collection", "OK");
                }

            }
            
        }
    }
}