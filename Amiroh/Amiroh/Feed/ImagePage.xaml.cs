using Amiroh.Classes;
using Amiroh.Feed;
using Amiroh.Profile;
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
        private string url_user_collection = "http://138.68.137.52:3000/AmirohAPI/users/collection/";
        private HttpClient _client = new HttpClient(new NativeMessageHandler());
        Inspo Obj = new Inspo();

        private bool PointsAreTapped = false;
        private bool CollectionsAreTapped = false;

        private bool UserOwnsInspo;



        public ImagePage(Inspo obj)
        {
            InitializeComponent();
            Obj = obj;

            var navigationPage = Application.Current.MainPage as NavigationPage;
            navigationPage.BarBackgroundColor = Color.White;
            navigationPage.BarTextColor = Color.Black;

            this.BindingContext = Obj;

            lblUsernameText.Text = lblUsernameText.Text.ToUpper();
            lblShowComments.Text = "Show all " + obj.Comments.Length + " comments.";


        }

        protected async override void OnAppearing()
        {

            if (Obj.UserId != MainUser.MainUserID.ID)
            {

                if (this.ToolbarItems.Count > 0)
                    this.ToolbarItems.Clear();
            }

            if (Obj.HasBeenLikedBy.Contains<string>(MainUser.MainUserID.Username))
            {
                btnPoints.Source = "pointson.png";
                PointsAreTapped = true;
            }

            var content_c = await _client.GetStringAsync(url_user_collection + MainUser.MainUserID.ID);
            var collection_list = JsonConvert.DeserializeObject<List<Inspo>>(content_c);
            var collection_list_id = collection_list.Select(c => c._Id);

            if (collection_list_id.Contains<string>(Obj._Id))
            {
                btnCollect.Source = "collectionon.png";
                CollectionsAreTapped = true;
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
            if (!CollectionsAreTapped)
            {

                string url_user_collection = "http://138.68.137.52:3000/AmirohAPI/users/collection/" + MainUser.MainUserID.ID;

                string postdataJson = JsonConvert.SerializeObject(new { _id = Obj._Id });

                var postdataString = new StringContent(postdataJson, new UTF8Encoding(), "application/json");


                var response = _client.PostAsync(url_user_collection, postdataString);
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
            else await DisplayAlert("Already in Collection", "You have already added this inspo to your collection.", "Ah, I forgot");
        }

        private async void Products_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ShowProductsPage(Obj));
        }

        private async void User_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UserPage(Obj.Username));
        }

        private async void EditDescription_Activated(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditInspoPage(Obj._Id));
        }

        private async void EditProducts_Activated(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddProductPage(Obj._Id));
        }
        private async void DeleteInspo_Activated(object sender, EventArgs e)
        {
            string url_edit_inspo = "http://138.68.137.52:3000/AmirohAPI/inspos/" + Obj._Id;
            HttpClient _client = new HttpClient(new NativeMessageHandler());

            var response = _client.DeleteAsync(url_edit_inspo);
            var responseString = response.Result.Content.ReadAsStringAsync().Result;


            if (response.Result.IsSuccessStatusCode)
            {
                await Navigation.PopAsync();

            }
        }
    }
}