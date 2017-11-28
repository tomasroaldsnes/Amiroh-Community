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

        private List<Comment> prevComments = new List<Comment>();

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
            
            try
            {
                Comment obj = new Comment();
                obj.Text = commentEntry.Text;
                obj.Username = MainUser.MainUserID.Username;
                obj.ProfilePicture = MainUser.MainUserID.ProfilePicture;

                

                string postdataJson = JsonConvert.SerializeObject(obj);
                var postdataString = new StringContent(postdataJson, new UTF8Encoding(), "application/json");

                string url = url_update_inspo + _obj._Id.ToString();
                var response = _client.PostAsync(url, postdataString);
                var responseString = await response.Result.Content.ReadAsStringAsync();

                _comments.Add(obj);

            }
            catch
            {
                await DisplayAlert("Upload Error 2", "No success", "OK");
            }
        }
    }
}