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

namespace Amiroh.Discover
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {
        private string url_all_users = "http://138.68.137.52:3000/AmirohAPI/users";

        private HttpClient _client = new HttpClient(new NativeMessageHandler());
        private ObservableCollection<User> _userList;
        private string _searchText;
        public SearchPage(string searchText)
        {
            InitializeComponent();

            var navigationPage = Application.Current.MainPage as NavigationPage;
            navigationPage.BarBackgroundColor = Color.White;
            navigationPage.BarTextColor = Color.Black;

            _searchText = searchText;
        }

        protected override async void OnAppearing()
        {
            

            var userCall = await _client.GetStringAsync(url_all_users);
            var UserList = JsonConvert.DeserializeObject<List<User>>(userCall);

            _userList = new ObservableCollection<User>(UserList);

            searchUsersBar.Text = _searchText;
        }

        private async void searchListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                if (searchListView.SelectedItem != null)
                {

                    var obj = e.Item as User;

                    await Navigation.PushAsync(new UserPage(obj.Username));
                    searchListView.SelectedItem = null;
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

        private void searchContent_TextChanged(object sender, TextChangedEventArgs e)
        {
             searchListView.ItemsSource = GetUsers(e.NewTextValue);
        }

        IEnumerable<User> GetUsers(string searchText = null)
        {
            if (String.IsNullOrWhiteSpace(searchText))
            {
                return _userList;
            }

            return _userList.Where(u => u.Username.StartsWith(searchText));

        }
    }
}