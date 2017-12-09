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
	public partial class FavedUsersPage : ContentPage
	{
        
        private ObservableCollection<User> favedUsers;
        
        private string url_get_faved_users = "http://138.68.137.52:3000/AmirohAPI/users/faved/" + MainUser.MainUserID.ID;
        private HttpClient _client = new HttpClient(new NativeMessageHandler());

        public FavedUsersPage ()
		{

			InitializeComponent ();

            var navigationPage = Application.Current.MainPage as NavigationPage;
            navigationPage.BarBackgroundColor = Color.White;
            navigationPage.BarTextColor = Color.Black;


		}

        protected override async void OnAppearing()
        {
            var content = await _client.GetStringAsync(url_get_faved_users);
            var posts = JsonConvert.DeserializeObject<List<User>>(content);

            favedUsers = new ObservableCollection<User>(posts);

            listviewFavedUsers.ItemsSource = favedUsers;
        }

       

    }
}