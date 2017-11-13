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

namespace Amiroh
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImagePage : ContentPage
    {
        private const string url_inspo_update = "http://10.5.50.138:3050/AmirohAPI/inspos/";
        private HttpClient _client = new HttpClient(new NativeMessageHandler());
        Inspo Obj = new Inspo();
        public ImagePage(Inspo obj)
        {
            InitializeComponent();
            Obj = obj;

        }

        private async void Points_Tapped(object sender, EventArgs e)
        {
            Obj.Claps += 1;
            btnPoints.Source = "pointson.png";
        }

        private async void Comment_Tapped(object sender, EventArgs e)
        {
            //open commentpage
        }

        private async void Collection_Tapped(object sender, EventArgs e)
        {
            //add to main users collection
        }

        private async void Products_Tapped(object sender, EventArgs e)
        {
            //push page with list of products
        }
        protected async override void OnDisappearing()
        {
            string postdataJson = JsonConvert.SerializeObject(Obj);
            var postdataString = new StringContent(postdataJson, new UTF8Encoding(), "application/json");

            var response =  _client.PutAsync(url_inspo_update + Obj.Id, postdataString);
            var responseString = response.Result.Content.ReadAsStringAsync().Result;
        }

    }
}