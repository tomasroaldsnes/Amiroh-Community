using Amiroh.Classes;
using ModernHttpClient;
using Newtonsoft.Json;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Amiroh.Controllers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using Xamarin;
using Plugin.Connectivity;

namespace Amiroh
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditProfilePage : ContentPage
    {
        private const string url_user = "http://192.168.1.7:3050/AmirohAPI/users/";
        private const string url_username = "http://192.168.1.7:3050/AmirohAPI/users/username/";
        private HttpClient _client = new HttpClient(new NativeMessageHandler());
        
        private ObservableCollection<User> _user;


        public EditProfilePage()
        {
            InitializeComponent();
            
            message.Text = "Congratulations! You have signed up to Amiroh. Let's set up the rest of your profile." + MainUser.MainUserID.USERNAME;
            
        }

        protected override async void OnAppearing()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                try
                {

                    var content_u = await _client.GetStringAsync(url_username + MainUser.MainUserID.USERNAME);
                    var userInfo = JsonConvert.DeserializeObject<List<User>>(content_u);
                    _user = new ObservableCollection<User>(userInfo);



                }
                catch (Exception ex)
                {

                    Insights.Report(ex);
                    errorPick.Text = "Ah, comon";

                }
            }
        }

        async void Button_Clicked(object sender, EventArgs e)
        {
           

            _user[0].ProfileDescription = profileDescriptionEntry.Text;
            
            try
            {
                var post = _user[0];
                string postdataJson = JsonConvert.SerializeObject(post);
                //var postdataString = new StringContent(postdataJson, new UTF8Encoding(), "application/json");

                string _url = url_user + _user[0].Id;
                var response = await _client.PutAsync(_url, new StringContent(postdataJson));
                //var responseString = response.Result.Content.ReadAsStringAsync().Result;
                
                
                Navigation.InsertPageBefore(new MainPage(), this);
                await Navigation.PopAsync();

                //if (response.Result.IsSuccessStatusCode)
                //{
                    
                //}
                //else
                //{
                //    errorPick.Text = "nope";
                //}
            }
            catch
            {
                errorPick.Text = "NOPE:" + _user[0].Id;
            }

        }

        private async void PictureButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                await CrossMedia.Current.Initialize();


                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    errorPick.Text = "Make sure camera is available";
                    return;
                }

                var _image = await CrossMedia.Current.PickPhotoAsync();

                //if (file == null)
                //{
                //    errorPick.Text = "Please pick a photo!";
                //    return;
                //}

                var imageName = await Controller.UploadFileAsync("mycontainer", _image.GetStream());

                _user[0].ProfilePicture = imageName.ToString();
                


                imageblub.Source = new UriImageSource
                {
                    
                    Uri = new Uri(imageName.ToString()),
                    CachingEnabled = false,
                    CacheValidity = TimeSpan.FromHours(1)

                };

            }
            catch (Exception er)
            {
                Insights.Report(er);
                await DisplayAlert("Error", "Error when trying to connect!", "OK");
            }



            //await AzureStorage.UploadFileAsync(ContainerType.Text, new MemoryStream(byteData));


            //send PUT request to database with imageID(url) to User.ProfilePicture

        }
    }
}