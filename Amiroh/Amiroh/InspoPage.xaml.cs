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
    public partial class InspoPage : ContentPage
    {
        private const string url_photo = "http://10.5.50.138:3050/AmirohAPI/inspos/";
        
        private HttpClient _client = new HttpClient(new NativeMessageHandler());
        private ObservableCollection<Inspo> _posts;
        ObservableCollection<Inspo> sortedByPoints_posts;

        public InspoPage()
        {
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            
        }

        

        protected async override void OnAppearing()
        {
            if (CrossConnectivity.Current.IsConnected)
            {

                this.IsBusy = true;
                try
                {

                    //check is device is connected to the internet

                    var content = await _client.GetStringAsync(url_photo);
                    var posts = JsonConvert.DeserializeObject<List<Inspo>>(content);

                    _posts = new ObservableCollection<Inspo>(posts);

                    sortedByPoints_posts = new ObservableCollection<Inspo>(
                         _posts.OrderBy(inspo => inspo)
                         );

                    listviewInspo.ItemsSource = sortedByPoints_posts;




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

            }
            else
            {
                var errorNetwork = new Label { Text = "Are you sure you are connected to the internet? Please try again." , FontSize = 16, FontFamily = "Lato-Bold.ttf#Lato-Bold" };
                var btnError = new Button { Text = "Try Again" };
                btnError.Clicked += BtnError_Clicked;
                layout.Children.Add(errorNetwork);
                layout.Children.Add(btnError);

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
                    listviewInspo.SelectedItem = null;
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
    }
}