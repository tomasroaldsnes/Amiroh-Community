using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net.Http;
using ModernHttpClient;
using Xamarin;
using Amiroh.Classes;
using Plugin.Connectivity;

namespace Amiroh
{
    

    
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DiscoverPageCategoryOverview : ContentPage
    {
        private const string url_photo = "http://138.68.137.52:3000/AmirohAPI/inspos/";
        
        private HttpClient _client = new HttpClient(new NativeMessageHandler());
        private ObservableCollection<Inspo> _posts;
        ObservableCollection<Inspo> DiscoverInsposList_Sorted;
        private string _category = "";
        private bool InspoListLoaded = false;

        public DiscoverPageCategoryOverview(string category)
        {
            InitializeComponent();

            var navigationPage = Application.Current.MainPage as NavigationPage;
            navigationPage.BarBackgroundColor = Color.White;
            navigationPage.BarTextColor = Color.Black;

            
            _category = category;
            
        }

        private async void LoadInspos()
        {
            try
            {

                //check is device is connected to the internet

                var content = await _client.GetStringAsync(url_photo);
                var posts = JsonConvert.DeserializeObject<List<Inspo>>(content);

                


                //Sort list after Categories if/else statement
                if (_category == "Eyes")
                {
                    posts.RemoveAll(inspo => !inspo.Tags.Contains<string>("Eyes"));
                    this.Title = "EYES";
                    

                }
                else if(_category == "Lips")
                {
                    posts.RemoveAll(inspo => !inspo.Tags.Contains<string>("Lips"));
                    this.Title = "LIPS";
                }
                else if (_category == "Eyebrows")
                {
                    posts.RemoveAll(inspo => !inspo.Tags.Contains<string>("Eyebrows"));
                    this.Title = "EYEBROWS";
                }
                else if (_category == "Contouring")
                {
                    posts.RemoveAll(inspo => !inspo.Tags.Contains<string>("Contouring"));
                    this.Title = "COUNTOURING";
                }
                else if (_category == "Night")
                {
                    posts.RemoveAll(inspo => !inspo.Tags.Contains<string>("Night"));
                    this.Title = "NIGHT";
                }
                else if (_category == "Day")
                {
                    posts.RemoveAll(inspo => !inspo.Tags.Contains<string>("Day"));
                    this.Title = "DAY";
                }
                else if (_category == "Trending")
                {
                    posts.RemoveAll(inspo => !inspo.Tags.Contains<string>("Trending"));
                    this.Title = "TRENDING";
                }
                else if (_category == "EditorsPick")
                {
                    posts.RemoveAll(inspo => !inspo.Tags.Contains<string>("EditorsPick"));
                    this.Title = "EDITOR's PICK";
                }

                

                DiscoverInsposList_Sorted = new ObservableCollection<Inspo>(
                     posts
                         .OrderByDescending(i => i.InspoCreated)
                         .OrderByDescending(i => i.Points)

                );

                listviewInspo.ItemsSource = DiscoverInsposList_Sorted;




                if (listviewInspo.IsRefreshing == true)
                {
                    listviewInspo.IsRefreshing = false;
                }


            }
            catch (Exception e)
            {
                try
                {
                    Insights.Report(e);
                    await DisplayAlert("Useless", "I tried to load the inspos, but I failed. Horribly.", "Be better");
                }
                catch
                {
                    await DisplayAlert("Error", "Error when trying to connect! Something is wrong! HELP!", "Jesus, calm down already.");
                }
            }
            finally
            {
                this.IsBusy = false;
            }


            base.OnAppearing();
        }

        protected override void OnAppearing()
        {
            //see if app needs to load new list of inspos
            if (!InspoListLoaded)
            {
                LoadInspos();
                InspoListLoaded = true;
            }
           
               
              
        }
        protected override void OnDisappearing()
        {
            InspoListLoaded = false;
        }

        
        private async void listviewInspo_ItemTapped(object sender, ItemTappedEventArgs e)
        {

            try
            {
                if (listviewInspo.SelectedItem != null)
                {

                    var obj = e.Item as Inspo;
                    var page = new ImagePage(obj);
                    page.BindingContext = obj;


                    await Navigation.PushAsync(page);
                    //listviewInspo.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Insights.Report(ex);
                    await DisplayAlert("Useless", "I tried to load the inspo, but I failed. Horribly.", "Be better");
                }
                catch
                {
                    await DisplayAlert("Error", "Error when trying to connect! Something is wrong! HELP!", "Jesus, calm down already.");
                }
            }
        }

        private void listviewInspo_Refreshing(object sender, EventArgs e)
        {
            LoadInspos();
            
        }
    }
}