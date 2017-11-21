using Amiroh.Classes;
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

namespace Amiroh.Feed
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CommentsPage : ContentPage
	{
        Inspo _obj = new Inspo();
        private ObservableCollection<Comment> _comments;

        private string url_update_inspo = "http://138.68.137.52:3000/AmirohAPI/inspos/";
        private HttpClient _client = new HttpClient(new NativeMessageHandler());

        public CommentsPage (Inspo obj)
		{

			InitializeComponent ();

            var navigationPage = Application.Current.MainPage as NavigationPage;
            navigationPage.BarBackgroundColor = Color.White;
            navigationPage.BarTextColor = Color.Black;


            _obj = obj;

            _comments = new ObservableCollection<Comment>(_obj.Comments);
            listviewComments.ItemsSource = _comments;
		}

        private async void btnComment_Clicked(object sender, EventArgs e)
        {
            Comment obj = new Comment();
            obj.Text = commentEntry.Text;
            obj.Username = MainUser.MainUserID.Username;
            obj.ProfilePicture = MainUser.MainUserID.ProfilePicture;

            _obj.Comments.ToList().Add(obj);
            _obj.Comments.ToArray();

            _comments.Add(obj);


            try
            {
                string postdataJson = JsonConvert.SerializeObject(new { title = _obj.Title, description = _obj.Description, URl = _obj.URL, username = _obj.Username, userId = _obj.UserId, productsUsed = _obj.ProductsUsed, points = _obj.Points, tags = _obj.Tags, comments = new { username = obj.Username, text = obj.Username, profilePicture = obj.ProfilePicture } } );
                var postdataString = new StringContent(postdataJson, new UTF8Encoding(), "application/json");

                string url = url_update_inspo + _obj._Id.ToString();
                var response = _client.PutAsync(url, postdataString);
                var responseString = response.Result.Content.ReadAsStringAsync().Result;


                if (response.Result.IsSuccessStatusCode)
                {
                    
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
    }
}