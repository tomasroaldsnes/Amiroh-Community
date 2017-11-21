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

namespace Amiroh
{
    
    
    
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        
        private string url_photo = "http://138.68.137.52:3000/AmirohAPI/inspos/user/";
        private string url_create_inspo = "http://138.68.137.52:3000/AmirohAPI/inspos";
      
        private const string url_user = "http://10.5.50.138:3050/AmirohAPI/users/username/";
        private HttpClient _client = new HttpClient(new NativeMessageHandler());
       // private ObservableCollection<Inspo> _profileImages;
        private ObservableCollection<User> _users;
        private ObservableCollection<Inspo> _userPhotos;
        


        public ProfilePage()
        {
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);

            if(MainUser.MainUserID.ProfilePicture == "" | MainUser.MainUserID.ProfilePicture == null)
            {
                MainUser.MainUserID.ProfilePicture = "https://i.pinimg.com/736x/d9/ea/0d/d9ea0d78ace304f631df1fd6e679b31b--prom-makeup-looks-pretty-prom-makeup.jpg";
            }

            if(MainUser.MainUserID.ProfileDescription == "" | MainUser.MainUserID.ProfileDescription == null)
            {
                MainUser.MainUserID.ProfileDescription = "Welcome to Amiroh! This is your profile description. Write something catchy. You can edit me in Settings.";
            }

            this.BindingContext = MainUser.MainUserID;

            imgProfilePicture.GestureRecognizers.Add(new TapGestureRecognizer(ProfileImageTap));

        }
        

        protected async override void OnAppearing()
        {
            
            try
            {
                
                var content_p = await _client.GetStringAsync(url_photo + MainUser.MainUserID.Username);
             var pI = JsonConvert.DeserializeObject<List<Inspo>>(content_p);

             _userPhotos = new ObservableCollection<Inspo>(pI);
  
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
               
             //GridCreation();
                
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
            string profilePictureURL = "";
            try
            {
                 profilePictureURL = await ImageUpload.ProfilePictureUploadAsync();
                MainUser.MainUserID.ProfilePicture = profilePictureURL;
            }
            catch (Exception e)
            {
                try
                {
                    Insights.Report(e);
                    await DisplayAlert("Oh God, not again", "I tried to upload your profile picture, but I failed. Miserably.", "*Takes a deep breath*");
                }
                catch
                {
                    await DisplayAlert("Oh God, not again", "I tried to load your profile, but I failed. Horribly.", "*Counting backwards from 10*");
                }
            }



            
                try
                {
                    await Task.Delay(3000);
                    string postdataJson = JsonConvert.SerializeObject(new { profilePicture = profilePictureURL });
                    var postdataString = new StringContent(postdataJson, new UTF8Encoding(), "application/json");

                    string new_url = url_user + MainUser.MainUserID.Username;
                    var response = _client.PutAsync(new_url, postdataString);
                    var responseString = response.Result.Content.ReadAsStringAsync().Result;
                   

                    if (response.Result.IsSuccessStatusCode)
                    {
                        //this should be changed to...something
                        await Navigation.PushModalAsync(new MainPage());
                    }
                    else
                    {
                        await DisplayAlert("Upload Error", "I really tried my best here. Promise", "Try harder");
                    }
                }
                catch (Exception e)
                {
                    try
                    {
                        Insights.Report(e);
                        await DisplayAlert("Oh God, not again", "I tried to upload your profile picture, but I failed. Miserably.", "*Takes a deep breath*");
                    }
                    catch
                    {
                        await DisplayAlert("Oh God, not again", "I tried to load your profile, but I failed. Horribly.", "*Counting backwards from 10*");
                    }
                }
            
           
        }

        private async void AddInspo_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new AddInspoPage());
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

        //public void GridCreation()
        //{
        //    try
        //    {
        //        UserPicturesGrid.RowDefinitions = new RowDefinitionCollection();
        //        UserPicturesGrid.ColumnDefinitions = new ColumnDefinitionCollection();

        //        UserPicturesGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
        //        UserPicturesGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
        //        UserPicturesGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

        //        for (int MyCount = 0; MyCount < 3; MyCount++)
        //        {

        //            UserPicturesGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(115, GridUnitType.Absolute) });

        //        }



        //        //set row and columns => column to 1 since AddInspoBtn should always be at position 0,0
        //        int row = 0;
        //        int column = 0;

                

        //        for (int i = 0; i < _userPhotos.Count(); i++)
        //        {


        //            if (column < 2)
        //            {
        //                UserPicturesGrid.Children.Add(new Image { Source = _userPhotos[i].URL, Aspect = Aspect.AspectFill, HeightRequest = 115, WidthRequest = 115, Rotation = -90 }, column, row);
        //                column++;
        //            }
        //            else if (column == 2)
        //            {
        //                UserPicturesGrid.Children.Add(new Image { Source = _userPhotos[i].URL, Aspect = Aspect.AspectFill, HeightRequest = 115, WidthRequest = 115, Rotation = -90 }, column, row);
        //                column = 0;
        //                row++;
        //            }
        //            else DisplayAlert("Error", "Something went wrong with loading userphotos", "OK");



        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        try
        //        {
        //            Insights.Report(ex);
        //            DisplayAlert("Dammit", "I'm not ready to upload an inspo. Like, emotionally, you know?", "*Takes a deep breath*");
        //        }
        //        catch
        //        {
        //            DisplayAlert("Dammit", "I'm not ready to upload an inspo. Like, emotionally, you know?", "*Counting backwards from 10*");
        //        }
        //    }

        //}

        private void Notifications_Tapped(object sender, EventArgs e)
        {
            //
        }

        private void x_Tapped(object sender, EventArgs e)
        {
            //
        }
        private void InspoGrid_Clicked(object sender, EventArgs e)
        {
            //
        }
        private void ProductPage_Clicked(object sender, EventArgs e)
        {
            //
        }
        private void Collection_Clicked(object sender, EventArgs e)
        {
            //
        }
    }
}