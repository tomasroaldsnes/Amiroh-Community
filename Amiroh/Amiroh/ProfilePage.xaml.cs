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
        
        private string url_photo = "http://10.5.50.138:3050/AmirohAPI/inspos/user/";
        private string url_create_inspo = "http://10.5.50.138.10:3050/AmirohAPI/inspos";
      
        private const string url_user = "http://10.5.50.138:3050/AmirohAPI/users/username/";
        private HttpClient _client = new HttpClient(new NativeMessageHandler());
       // private ObservableCollection<Inspo> _profileImages;
        private ObservableCollection<User> _users;
        private ObservableCollection<Inspo> _userPhotos;
        


        public ProfilePage()
        {
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            

        }
        

        protected async override void OnAppearing()
        {
            try
            {
                profilePick.Source = new UriImageSource
                {
                    Uri = new Uri("https://scontent.fbne3-1.fna.fbcdn.net/v/t31.0-8/20248271_10155308253382787_1002352637802164127_o.jpg?oh=6af2bc444aeb8ffd23f656c92e31be38&oe=5A852B09"),
                    CachingEnabled = false,
                    CacheValidity = TimeSpan.FromHours(1)
                };
                errorlbl.Text = MainUser.MainUserID.ProfilePicture;
                userName.Text = MainUser.MainUserID.Username;
                userDescription.Text = MainUser.MainUserID.ProfileDescription;
                profilePick.GestureRecognizers.Add(new TapGestureRecognizer(ProfileImageTap));
                this.IsBusy = true;
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
            try
            {

                if (CrossConnectivity.Current.IsConnected)
                {
                    
                    var content_p = await _client.GetStringAsync(url_photo + MainUser.MainUserID.Username);
                    var pI = JsonConvert.DeserializeObject<List<Inspo>>(content_p);

                    _userPhotos = new ObservableCollection<Inspo>(pI);

                    if (MainUser.MainUserID.ProfilePicture == "")
                    {
                        profilePick.Source = "https://image.freepik.com/free-icon/female-student-silhouette_318-62252.jpg";
                        
                    }
                    
                   


                    

                }
               

                
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
                if (_userPhotos.Count < 1)
                {
                    UserPicturesGrid.RowDefinitions = new RowDefinitionCollection();
                    UserPicturesGrid.ColumnDefinitions = new ColumnDefinitionCollection();

                    UserPicturesGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                    UserPicturesGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(115, GridUnitType.Absolute) });

                    Button AddInspoBtn = new Button { Text = "+", TextColor = Color.White, BackgroundColor = Color.FromHex("#555659"), FontAttributes = FontAttributes.Bold, HeightRequest = 115, WidthRequest = 115 };
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
            //create dynamic grid for user photos
            




            base.OnAppearing();
        }

        private async void ProfileImageTap(View arg1, object arg2)
        {
            string profilePictureURL = "";
            try
            {
                 profilePictureURL = await ImageUpload.ProfilePictureUploadAsync();
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



            if (CrossConnectivity.Current.IsConnected)
              {
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
                        //var inspo_return = JsonConvert.DeserializeObject<User>(responseString);
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
            else
            {
                await DisplayAlert("Network Error", "Are you connected to the internet?", "Maybe, maybe not");
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

        public void GridCreation()
        {
            try
            {
                UserPicturesGrid.RowDefinitions = new RowDefinitionCollection();
                UserPicturesGrid.ColumnDefinitions = new ColumnDefinitionCollection();

                UserPicturesGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                UserPicturesGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                UserPicturesGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

                for (int MyCount = 0; MyCount < 3; MyCount++)
                {

                    UserPicturesGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(115, GridUnitType.Absolute) });

                }



                //set row and columns => column to 1 since AddInspoBtn should always be at position 0,0
                int row = 0;
                int column = 1;

                //create button for uploading a inspo post and set its position to 0,0 in the grid
                Button AddInspoBtn = new Button { Text = "+", TextColor = Color.White, BackgroundColor = Color.FromHex("#555659"), FontAttributes = FontAttributes.Bold, HeightRequest = 115, WidthRequest = 115 };
                AddInspoBtn.Clicked += AddInspo_Clicked;
                UserPicturesGrid.Children.Add(AddInspoBtn, 0, row);
            

           

            
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
    }
}