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
    public partial class ShowProductsPage : ContentPage
    {

        
        HttpClient _client = new HttpClient(new NativeMessageHandler());
        private ObservableCollection<string> productsList;
        private Inspo _obj;

        public ShowProductsPage(Inspo obj)
        {

            InitializeComponent();

            var navigationPage = Application.Current.MainPage as NavigationPage;
            navigationPage.BarBackgroundColor = Color.White;
            navigationPage.BarTextColor = Color.Black;

            _obj = obj;

            productsList = new ObservableCollection<string>(_obj.ProductsUsed);
            listviewProducts.ItemsSource = productsList;

        }
       

        
    }
}