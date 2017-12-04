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
using FFImageLoading.Forms;

namespace Amiroh
{
    


    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InspoPage : ContentPage
    {
        private const string url_photo = "http://138.68.137.52:3000/AmirohAPI/inspos/";
        
        private HttpClient _client = new HttpClient(new NativeMessageHandler());
        private ObservableCollection<Inspo> _posts_All;
        ObservableCollection<Inspo> sortedByPoints_First = new ObservableCollection<Inspo>();

        private bool isLoading;

        public InspoPage()
        {
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);


           


        }

        private async void LoadInspos()
        {

            //check is device is connected to the internet

            var content = await _client.GetStringAsync(url_photo);
            var posts = JsonConvert.DeserializeObject<List<Inspo>>(content);

           _posts_All = new ObservableCollection<Inspo>(
            posts.OrderBy(inspo => inspo).Reverse<Inspo>());

            listviewInspo.ItemsSource = _posts_All;

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
                    await DisplayAlert("Useless", "I tried to load the inspos, but I failed. Horribly.", "Be better");
                }
                catch
                {
                    await DisplayAlert("Error", "Error when trying to connect! Something is wrong! HELP!", "Jesus, calm down already.");
                }
            }

            base.OnAppearing();

        }
        

       private async void BtnError_Clicked(object sender, EventArgs e)
        {
            Navigation.InsertPageBefore(new Amiroh.MainPage(), this);
            await Navigation.PopAsync();
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

        private async void AddInspo_Tapped(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new AddInspoPage());
        }

        private void Social_Tapped(object sender, EventArgs e)
        {
            //

        }
    }
}