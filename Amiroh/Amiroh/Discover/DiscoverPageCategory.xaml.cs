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
    public partial class DiscoverPageCategory : ContentPage
    {
        private const string url_photo = "http://192.168.1.10:3050/AmirohAPI/inspos/";

        private int Position = 0;
        private string CategoryType = "";
        
        private HttpClient _client = new HttpClient(new NativeMessageHandler());
        private ObservableCollection<Inspo> _posts;

        public DiscoverPageCategory(string categoryType, int position)
        {
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, true);
            CategoryType = categoryType;
            Position = position;
        }



        protected async override void OnAppearing()
        {
            this.IsBusy = true;
            try{
                if (CrossConnectivity.Current.IsConnected)
                {
                    //check is device is connected to the internet

                    var content = await _client.GetStringAsync(url_photo);
                    var posts = JsonConvert.DeserializeObject<List<Inspo>>(content);

                    _posts = new ObservableCollection<Inspo>(posts);

                    //here do we sort the list according to tags
                    
                   if(CategoryType == "Category 1") //add all other categories ||
                    {
                        ObservableCollection<Inspo> sortedInspoList = new ObservableCollection<Inspo>();
                        foreach (var inspo in _posts)
                        {
                            if (inspo.Tags != null)
                            {
                                if (inspo.Tags.Contains<string>(CategoryType))
                                {
                                    sortedInspoList.Add(inspo);
                                }
                            }
                        }
                        listviewInspo.ItemsSource = sortedInspoList;
                        sortedInspoList.Move(Position, 0);
                    }
                    else
                    {
                        await DisplayAlert("AHHHH", "I failed to find the proper category.", "Why u do dis");
                    }


                    //sorting stop
                    
                }
            }
            catch(Exception e)
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

        private async void ViewCell_Tapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                if (listviewInspo.SelectedItem != null)
                {

                    var obj = e.Item as Inspo;
                    var page = new ImagePage(obj);
                    page.BindingContext = obj;


                    await Navigation.PushModalAsync(page);
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