using ModernHttpClient;
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
using Amiroh.Login;
using Plugin.Connectivity;
using FFImageLoading.Forms;

namespace Amiroh
{
    
    
    
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserPage : ContentPage
    {
        
        private string url_photo = "http://138.68.137.52:3000/AmirohAPI/inspos/user/";

        private string url_user_collection = "http://138.68.137.52:3000/AmirohAPI/users/collection/";

        private const string url_addFaved = "http://138.68.137.52:3000/AmirohAPI/users/faved/";

        private const string url_user = "http://138.68.137.52:3000/AmirohAPI/users/username/";
        private HttpClient _client = new HttpClient(new NativeMessageHandler());
       // private ObservableCollection<Inspo> _profileImages;
        //private ObservableCollection<User> _users;
        private ObservableCollection<Inspo> _userPhotos;
        private ObservableCollection<Inspo> _userCollection;

        private bool IsInspoLoaded = false;
        private bool NewInspoUploaded = false;

        private string Username = "";
        private ObservableCollection<User> _user;
        

        public UserPage(string _username)
        {
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            Username = _username;

            



        }
        

        protected async override void OnAppearing()
        {
            
            try
            {
            var content_u = await _client.GetStringAsync(url_user + Username);
            var uObj = JsonConvert.DeserializeObject<List<User>>(content_u);

             _user = new ObservableCollection<User>(uObj);

             this.BindingContext = _user[0];
             lblUsername.Text = lblUsername.Text.ToUpper();




                var content_p = await _client.GetStringAsync(url_photo + Username);
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
                        ProfileGrid.Children.Add(new CachedImage{ Source = _userPhotos[i].URL, WidthRequest = 145, HeightRequest = 145, DownsampleHeight = 145, DownsampleWidth = 145, VerticalOptions = LayoutOptions.StartAndExpand }, 0, 5, row, row+1);
                        
                        column++;
                    }
                    else if (column == 1)
                    {
                        ProfileGrid.Children.Add(new CachedImage { Source = _userPhotos[i].URL, WidthRequest = 145, HeightRequest = 145, DownsampleHeight = 145, DownsampleWidth = 145, VerticalOptions = LayoutOptions.StartAndExpand}, 5 , 10, row, row+1);
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

      
        private void InspoGrid_Clicked(object sender, EventArgs e)
        {
            btnInspoGrid.FontFamily = "Lato-Bold.ttf#Lato-Bold";
            btnCollection.FontFamily = "Lato-Light.ttf#Lato-Light";



            int row = 4;
            int column = 0;

            for (int i = _userCollection.Count()+4; i > 4; i--)
            {
                ProfileGrid.Children.RemoveAt(i);
            }

            for (int i = 0; i < _userPhotos.Count(); i++)
            {


                if (column < 1)
                {
                    ProfileGrid.Children.Add(new CachedImage { Source = _userPhotos[i].URL, WidthRequest = 145, HeightRequest = 145, DownsampleHeight = 145, DownsampleWidth = 145, VerticalOptions = LayoutOptions.StartAndExpand }, 0, 5, row, row + 1);

                    column++;
                }
                else if (column == 1)
                {
                    ProfileGrid.Children.Add(new CachedImage { Source = _userPhotos[i].URL, WidthRequest = 145, HeightRequest = 145, DownsampleHeight = 145, DownsampleWidth = 145, VerticalOptions = LayoutOptions.StartAndExpand }, 5, 10, row, row + 1);
                    column = 0;
                    row++;
                }
                else DisplayAlert("Error", "Something went wrong with loading userphotos", "OK");




            }
        }

        private async void Fave_Clicked(object sender, EventArgs e)
        {


            string postdataJson = JsonConvert.SerializeObject(new { _id = _user[0]._Id });
            var postdataString = new StringContent(postdataJson, new UTF8Encoding(), "application/json");

            string new_url = url_addFaved + MainUser.MainUserID.ID;
            var response = _client.PostAsync(new_url, postdataString);
            var responseString = response.Result.Content.ReadAsStringAsync().Result;

            if (response.Result.IsSuccessStatusCode)
            {
                btnFave.Source = "faved.png";
                btnFave.IsEnabled = false;

                var _o = new Notification();
                _o.PushNotification("FAVE", "faved.png", MainUser.MainUserID.Username, _user[0]._Id);
            }
            else
            {
                await DisplayAlert("Fave Error", "I really tried my best here. Promise", "Try harder");
            }
        }

        private async void Collection_Clicked(object sender, EventArgs e)
        {
            for (int i = _userPhotos.Count()+4; i > 4; i--)
            {
                ProfileGrid.Children.RemoveAt(i);
            }

            var content_p = await _client.GetStringAsync(url_user_collection + MainUser.MainUserID.ID);
            var user_obj = JsonConvert.DeserializeObject<List<Inspo>>(content_p);

            _userCollection = new ObservableCollection<Inspo>(user_obj);

            btnInspoGrid.FontFamily = "Lato-Light.ttf#Lato-Light";
            btnCollection.FontFamily = "Lato-Bold.ttf#Lato-Bold";

            if (_userCollection.Count() != 0 | _userCollection != null)
            {

                int row = 4;
                int column = 0;

                for (int i = 0; i < _userCollection.Count(); i++)
                {
                    if (column < 1)
                    {
                        ProfileGrid.Children.Add(new CachedImage { Source = _userCollection[i].URL, WidthRequest = 145, HeightRequest = 145, DownsampleHeight = 145, DownsampleWidth = 145, VerticalOptions = LayoutOptions.StartAndExpand }, 0, 5, row, row + 1);

                        column++;
                    }
                    else if (column == 1)
                    {
                        ProfileGrid.Children.Add(new CachedImage { Source = _userCollection[i].URL, WidthRequest = 145, HeightRequest = 145, DownsampleHeight = 145, DownsampleWidth = 145, VerticalOptions = LayoutOptions.StartAndExpand }, 5, 10, row, row + 1);
                        column = 0;
                        row++;
                    }
                    else
                    {
                        await DisplayAlert("Error", "Something went wrong with loading Collection", "OK");
                    }

                }
            }
            else
            {
                await DisplayAlert("Collection Error", "This user has no inspos collected.", "Dang.");
            }
            
        }
    }
}