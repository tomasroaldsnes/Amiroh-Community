using Amiroh.Classes;
using Amiroh.Feed;
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

namespace Amiroh
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImagePage : ContentPage
    {
        private const string url_inspo_update = "http://138.68.137.52:3000/AmirohAPI/inspos/";
        private const string url_user_collection = "http://138.68.137.52:3000/AmirohAPI/users/";
        private HttpClient _client = new HttpClient(new NativeMessageHandler());
        Inspo Obj = new Inspo();

        private bool PointsAreTapped = false;

       


        public ImagePage(Inspo obj)
        {
            InitializeComponent();
            Obj = obj;

            var navigationPage = Application.Current.MainPage as NavigationPage;
            navigationPage.BarBackgroundColor = Color.White;
            navigationPage.BarTextColor = Color.Black;

            


        }

        protected override void OnAppearing()
        {
            if (Obj.HasBeenLikedBy.Contains<string>(MainUser.MainUserID.Username))
            {
                btnPoints.Source = "pointson.png";
                PointsAreTapped = true;
            }

        }
        

        private async void Points_Tapped(object sender, EventArgs e)
        {
            if (!PointsAreTapped)
            {

                Obj.Points += 1;
                var hasbeenlikedby = Obj.HasBeenLikedBy.ToList();
                hasbeenlikedby.Add(MainUser.MainUserID.Username);

                //HasBeenLiked = true;

                //string postdataJson = JsonConvert.SerializeObject(new { title = Obj.Title, description = Obj.Description, URl = Obj.URL, username = Obj.Username, userId = Obj.UserId, productsUsed = Obj.ProductsUsed, points = Obj.Points, tags = Obj.Tags, comments = Obj.Comments });
                string postdataJson = JsonConvert.SerializeObject(new { points = Obj.Points, hasBeenLikedBy = hasbeenlikedby.ToArray<string>() });

                var postdataString = new StringContent(postdataJson, new UTF8Encoding(), "application/json");

                string url = url_inspo_update + Obj._Id.ToString();
                var response = _client.PutAsync(url, postdataString);
                var responseString = response.Result.Content.ReadAsStringAsync().Result;

                if (response.Result.IsSuccessStatusCode)
                {
                    btnPoints.Source = "pointson.png";
                    PointsAreTapped = true;

                    var _o = new Notification();
                   _o.PushNotification("POINT", Obj.URL, MainUser.MainUserID.Username, Obj.UserId);

                }
                else
                {
                    await DisplayAlert("Upload Error", Obj._Id.ToString() + "  " + Obj.Username, "Try harder");
                }
            }
            else
            {
                Points_Already_Tapped();
            }
           

        }

        private async void Points_Already_Tapped()
        {
            //needs a ifUserHasLikedInspo function 
            //add a list of users that has liked 
            Obj.Points -= 1;
            var hasbeenliked = Obj.HasBeenLikedBy.ToList();
            hasbeenliked.Remove(MainUser.MainUserID.Username);

            //HasBeenLiked = true;

            //string postdataJson = JsonConvert.SerializeObject(new { title = Obj.Title, description = Obj.Description, URl = Obj.URL, username = Obj.Username, userId = Obj.UserId, productsUsed = Obj.ProductsUsed, points = Obj.Points, tags = Obj.Tags, comments = Obj.Comments });
            string postdataJson = JsonConvert.SerializeObject(new { points = Obj.Points, hasBeenLikedBy = hasbeenliked.ToArray<string>() });
            var postdataString = new StringContent(postdataJson, new UTF8Encoding(), "application/json");

            string url = url_inspo_update + Obj._Id.ToString();
            var response = _client.PutAsync(url, postdataString);
            var responseString = response.Result.Content.ReadAsStringAsync().Result;

            if (response.Result.IsSuccessStatusCode)
            {
                btnPoints.Source = "points.png";
                PointsAreTapped = false;
                
            }
            else
            {
                await DisplayAlert("Upload Error", Obj._Id.ToString() + "  " + Obj.Username, "Try harder");
            }

        }

        private async void Comment_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CommentsPage(Obj));
        }

        private async void Collection_Tapped(object sender, EventArgs e)
        {
            //add to main users collection
            //add inspo._id to user collection
            string postdataJson = JsonConvert.SerializeObject(new { _id = Obj._Id.ToString() });

            var postdataString = new StringContent(postdataJson, new UTF8Encoding(), "application/json");

            string url = url_user_collection + MainUser.MainUserID.ID.ToString();
            var response = _client.PostAsync(url, postdataString);
            var responseString = response.Result.Content.ReadAsStringAsync().Result;

            if (response.Result.IsSuccessStatusCode)
            {
                btnCollect.Source = "collectionon.png";
                btnCollect.IsEnabled = false;

            }
            else
            {

                await DisplayAlert("Upload Error", Obj._Id.ToString() + "  " + Obj.Username, "Try harder");

            }
        }

        private async void Products_Tapped(object sender, EventArgs e)
        {
            //push page with list of products
        }

        private async void User_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UserPage(Obj.Username));
        }


        private async void Backarrow_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}