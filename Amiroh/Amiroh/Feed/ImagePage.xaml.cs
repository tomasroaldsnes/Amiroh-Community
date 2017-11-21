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

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Amiroh
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImagePage : ContentPage
    {
        private const string url_inspo_update = "http://138.68.137.52:3000/AmirohAPI/inspos/";
        private HttpClient _client = new HttpClient(new NativeMessageHandler());
        Inspo Obj = new Inspo();
        

       


        public ImagePage(Inspo obj)
        {
            InitializeComponent();
            Obj = obj;

            var navigationPage = Application.Current.MainPage as NavigationPage;
            navigationPage.BarBackgroundColor = Color.White;
            navigationPage.BarTextColor = Color.Black;

        }

        private async void Points_Tapped(object sender, EventArgs e)
        {
            //needs a ifUserHasLikedInspo function 
            //add a list of users that has liked 
            Obj.Points += 1;
            btnPoints.Source = "pointson.png";

            //HasBeenLiked = true;

            //string postdataJson = JsonConvert.SerializeObject(new { title = Obj.Title, description = Obj.Description, URl = Obj.URL, username = Obj.Username, userId = Obj.UserId, productsUsed = Obj.ProductsUsed, points = Obj.Points, tags = Obj.Tags, comments = Obj.Comments });
            string postdataJson = JsonConvert.SerializeObject(new { points = Obj.Points });
            
            var postdataString = new StringContent(postdataJson, new UTF8Encoding(), "application/json");

            string url = url_inspo_update + Obj._Id.ToString();
            var response = _client.PutAsync(url, postdataString);
            var responseString = response.Result.Content.ReadAsStringAsync().Result;

            if (response.Result.IsSuccessStatusCode)
            {
                //var inspo_return = JsonConvert.DeserializeObject<User>(responseString);
                await Navigation.PopAsync();
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
        }

        private async void Products_Tapped(object sender, EventArgs e)
        {
            //push page with list of products
        }
       

        private async void Backarrow_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}