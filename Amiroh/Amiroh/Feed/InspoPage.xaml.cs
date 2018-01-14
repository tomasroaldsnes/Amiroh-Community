using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Amiroh.Feed;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net.Http;
using ModernHttpClient;
using Xamarin;
using Amiroh.Classes;
using Plugin.Connectivity;
using FFImageLoading.Forms;
using Amiroh.Profile;

namespace Amiroh
{
    


    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InspoPage : ContentPage
    {
        private const string url_photo = "http://138.68.137.52:3000/AmirohAPI/inspos/";

        private string url_faved_users = "http://138.68.137.52:3000/AmirohAPI/users/faved/" + MainUser.MainUserID.ID;

        private HttpClient _client = new HttpClient(new NativeMessageHandler());
        private ObservableCollection<Inspo> _AllInsposList_Sorted;
        ObservableCollection<Inspo> sortedByPoints_First = new ObservableCollection<Inspo>();

        

        public InspoPage()
        {
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            listviewInspo.ItemsSource = _AllInsposList_Sorted;

            



        }

        private async void LoadInspos()
        {
            //get faved users to check if list of inspos contains posts from faved users
            var content_faved = await _client.GetStringAsync(url_faved_users);
            var favedUserList = JsonConvert.DeserializeObject<List<User>>(content_faved);

            var favedUsernameList = favedUserList.Select(u => u.Username);


            //get all inspos
            var content_inspo = await _client.GetStringAsync(url_photo);
            var AllInsposList = JsonConvert.DeserializeObject<List<Inspo>>(content_inspo);

            _AllInsposList_Sorted = new ObservableCollection<Inspo>(
             AllInsposList
                 .OrderByDescending(i => i.InspoCreated)
                 .OrderByDescending(i => i.Points)
                 .OrderByDescending(i => favedUsernameList.Contains<string>(i.Username))
                
                );

            listviewInspo.ItemsSource = _AllInsposList_Sorted;

            

            if (listviewInspo.IsRefreshing == true)
            {
                listviewInspo.IsRefreshing = false;
            }

        }

       

        protected async override void OnAppearing()
        {
            try
            {

                LoadInspos();

                

            }
            catch (Exception e)
            {
                try
                {
                    Insights.Report(e);
                    await DisplayAlert("Useless Load", "I tried to load the inspos, but I failed. Horribly.", "Be better");
                }
                catch
                {
                    await DisplayAlert("Error Load", "Error when trying to connect! Something is wrong! HELP!", "Jesus, calm down already.");
                }
            }

            base.OnAppearing();

        }
        

       

        private async void listviewInspo_ItemTapped(object sender, ItemTappedEventArgs e)
        {

            try
            {
                if (listviewInspo.SelectedItem != null)
                {

                    var obj = e.Item as Inspo;

                    await Navigation.PushAsync(new ImagePage(obj));
                    listviewInspo.SelectedItem = null;
                }
                else
                {
                    LoadInspos();
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

        private void listviewInspo_Refreshing(object sender, EventArgs e)
        {
            LoadInspos();
            
        }

        private async void AddInspo_Tapped(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new ChooseInspoPage());
        }

        private async void Social_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FavedUsersPage());

        }
    }
}