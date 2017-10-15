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
        private const string url_photo = "http://192.168.1.7:3050/AmirohAPI/inspos/";
        
        private HttpClient _client = new HttpClient(new NativeMessageHandler());
        private ObservableCollection<Inspo> _posts;

        public InspoPage()
        {
            InitializeComponent();
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
                    listviewInspo.ItemsSource = _posts;
                }
            }
            catch(Exception e)
            {
                Insights.Report(e);
               await DisplayAlert("Error", "Error when trying to connect!", "OK");
            }
            finally
            {
                this.IsBusy = false;
            }
        

            
            base.OnAppearing();
        }

        private async void ViewCell_Tapped(object sender, ItemTappedEventArgs e)
        {
            if (listviewInspo.SelectedItem != null)
            {
                

                var page = new ImagePage();
                page.BindingContext = e.Item as Inspo;
                
                
                await Navigation.PushModalAsync(page);
                listviewInspo.SelectedItem = null;
            }

        }
    }
}