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
        
        private string url_photo = "http://192.168.1.7:3050/AmirohAPI/inspos/user/";
        private string url_create_inspo = "http://192.168.1.7:3050/AmirohAPI/inspos";
        //private const string url_user = "http://192.168.1.7:3050/AmirohAPI/users/";
        private const string url_user = "http://192.168.1.7:3050/AmirohAPI/users/username/";
        private HttpClient _client = new HttpClient(new NativeMessageHandler());
       // private ObservableCollection<Inspo> _profileImages;
        private ObservableCollection<User> _users;
        private ObservableCollection<Inspo> _userPhotos;
        


        public ProfilePage()
        {
            InitializeComponent();

            
            
        }

        protected async override void OnAppearing()
        {
            this.IsBusy = true;
           
            try
            {

                if (CrossConnectivity.Current.IsConnected)
                {
                    //check is device is connected to the internet
                        var content_u = await _client.GetStringAsync(url_user + MainUser.MainUserID.USERNAME);
                        var userInfo = JsonConvert.DeserializeObject<List<User>>(content_u);
                        _users = new ObservableCollection<User>(userInfo);
                   

                    
                    

                    var content_p = await _client.GetStringAsync(url_photo + _users[0].Username);
                    var pI = JsonConvert.DeserializeObject<List<Inspo>>(content_p);

                    _userPhotos = new ObservableCollection<Inspo>(pI);

                    if (_users[0].ProfilePicture == "")
                    {
                        profilePick.Source = new UriImageSource
                        {

                            Uri = new Uri("https://image.freepik.com/free-icon/female-student-silhouette_318-62252.jpg"),
                            CachingEnabled = false,
                            CacheValidity = TimeSpan.FromHours(1)

                        };
                    }
                    else
                    {
                        profilePick.Source = new UriImageSource
                        {

                            Uri = new Uri(_users[0].ProfilePicture),
                            CachingEnabled = false,
                            CacheValidity = TimeSpan.FromHours(1)

                        };
                    }

                
                   
                    userName.Text = _users[0].Username;
                    userDescription.Text = _users[0].ProfileDescription;

                }
               

                
            }
            catch (Exception e)
            {
                Insights.Report(e);
                await DisplayAlert("Error", "Error when trying to connect!", "OK");
            }
            finally
            {
                this.IsBusy = false;
            }

            //create grid view of photos from user's image collection

            try
            {
                if (_userPhotos.Count < 1)
                {
                    UserPicturesGrid.RowDefinitions = new RowDefinitionCollection();
                    UserPicturesGrid.ColumnDefinitions = new ColumnDefinitionCollection();

                    UserPicturesGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                    UserPicturesGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(115, GridUnitType.Absolute) });

                    Button AddInspoBtn = new Button { Text = "+", TextColor = Color.White, BackgroundColor = Color.FromHex("#203E4A"), FontAttributes = FontAttributes.Bold, HeightRequest = 115, WidthRequest = 115 };
                    AddInspoBtn.Clicked += AddInspo_Clicked;
                    UserPicturesGrid.Children.Add(AddInspoBtn , 0, 0);

                }
                else
                {
                    GridCreation();
                }
            }
            catch(Exception e)
            {
                await DisplayAlert("Error", "Can't load user grid", "OK");
            }
            //create dynamic grid for user photos
            




            base.OnAppearing();
        }

        private async void AddInspo_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddInspoPage());
        }

        public void GridCreation()
        {
            UserPicturesGrid.RowDefinitions = new RowDefinitionCollection();
            UserPicturesGrid.ColumnDefinitions = new ColumnDefinitionCollection();

            UserPicturesGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            UserPicturesGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            UserPicturesGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

            for (int MyCount = 0; MyCount < 3; MyCount++)
            {

                UserPicturesGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(115, GridUnitType.Absolute) });
                //UserPicturesGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            }
            //trenger denne å være her?
            //var userPicture = new Image
            //{
            //    Source = new UriImageSource
            //    {
            //        Uri = new Uri(_users[0].ProfilePicture),
            //        CachingEnabled = false,
            //        CacheValidity = TimeSpan.FromHours(1)
            //    }
            //};



            int row = 0;
            int column = 1;
            Button AddInspoBtn = new Button { Text = "+", TextColor = Color.White, BackgroundColor = Color.FromHex("#203E4A"), FontAttributes = FontAttributes.Bold, HeightRequest = 115, WidthRequest = 115 };
            AddInspoBtn.Clicked += AddInspo_Clicked;
            UserPicturesGrid.Children.Add(AddInspoBtn, 0, row);

           

            try
            {
                for (int i = 0; i < _userPhotos.Count(); i++)
                {


                    if (column < 2)
                    {
                        UserPicturesGrid.Children.Add(new Image { Source = _userPhotos[i].URL, Aspect = Aspect.AspectFill, HeightRequest = 115, WidthRequest = 115, Rotation = -90 }, column, row);
                        column++;
                    }
                    else if (column == 2)
                    {
                        UserPicturesGrid.Children.Add(new Image { Source = _userPhotos[i].URL, Aspect = Aspect.AspectFill, HeightRequest = 115, WidthRequest = 115, Rotation = -90 }, column, row);
                        column = 0;
                        row++;
                    }
                    else DisplayAlert("Error", "Something went wrong with loading userphotos", "OK");



                }
            }
            catch(Exception e)
            {
                Insights.Report(e);

            }



            //UserPicturesGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            //UserPicturesGrid.Children.Add(new Image { Source = _userPhoto[0].URL });
        }
    }
}