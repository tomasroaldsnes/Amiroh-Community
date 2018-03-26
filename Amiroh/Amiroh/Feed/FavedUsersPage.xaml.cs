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
using Xamarin;
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

			InitializeComponent();

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

            if(favedUsers.Count() == 0)
            {
                NoFavorittedUsers.Text = "You haven't favoritted any users yet!";
            }
                
            
        }

        private async void listviewFavedUsers_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                if (listviewFavedUsers.SelectedItem != null)
                {

                    var obj = e.Item as User;

                    await Navigation.PushAsync(new UserPage(obj.Username));
                    listviewFavedUsers.SelectedItem = null;
                }
                
            }
            catch (Exception ex)
            {
                try
                {
                    Insights.Report(ex);
                    await DisplayAlert("Useless Tap", "I tried to load the inspo, but I failed. Horribly.", "Be better");


                }
                catch
                {
                    await DisplayAlert("Error Tap", "Error when trying to connect! Something is wrong! HELP!", "Jesus, calm down already.");

                }
            }

        }
    }
}