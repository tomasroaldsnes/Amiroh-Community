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

namespace Amiroh.Profile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddProductPage : ContentPage
    {

        private string url_inspo_add_product = "http://138.68.137.52:3000/AmirohAPI/inspos/";
        HttpClient _client = new HttpClient(new NativeMessageHandler());
        private List<string> productList;

        private string _id;

        private bool _FromSettings;

        public AddProductPage(string id, bool fromSettings)
        {

            InitializeComponent();

            var navigationPage = Application.Current.MainPage as NavigationPage;
            navigationPage.BarBackgroundColor = Color.White;
            navigationPage.BarTextColor = Color.Black;

            _id = id;
            _FromSettings = fromSettings;

            if (_FromSettings)
            {
                btnContinue.Text = "Update";
            }



        }

        private async void CleanUp()
        {
            string url_edit_inspo = "http://138.68.137.52:3000/AmirohAPI/inspos/" + _id;
            HttpClient _client = new HttpClient(new NativeMessageHandler());

            var response = await _client.DeleteAsync(url_edit_inspo);
  
        }

        protected override bool OnBackButtonPressed()
        {
            if (!_FromSettings)
            {
                CleanUp();
            }
            return base.OnBackButtonPressed();
        }



        private async void Continue_Tapped(object sender, ItemTappedEventArgs e)
        {

            try
            {
                btnContinue.IsEnabled = false;
                productList = new List<string>();

                if(entryProduct1.Text != null)
                    productList.Add(entryProduct1.Text);

                if (entryProduct2.Text != null)
                    productList.Add(entryProduct2.Text);

                if (entryProduct3.Text != null)
                    productList.Add(entryProduct3.Text);

                if (entryProduct4.Text != null)
                    productList.Add(entryProduct4.Text);

                if (entryProduct5.Text != null)
                    productList.Add(entryProduct5.Text);

                if (entryProduct6.Text != null)
                    productList.Add(entryProduct6.Text);

                if (entryProduct7.Text != null)
                    productList.Add(entryProduct7.Text);

                if (entryProduct8.Text != null)
                    productList.Add(entryProduct8.Text);

                if (entryProduct9.Text != null)
                    productList.Add(entryProduct9.Text);

                if (entryProduct10.Text != null)
                    productList.Add(entryProduct10.Text);

                if (entryProduct11.Text != null)
                    productList.Add(entryProduct11.Text);

                string postdataJson = JsonConvert.SerializeObject(new { productsUsed = productList.ToArray() }); //<-------
                var postdataString = new StringContent(postdataJson, new UTF8Encoding(), "application/json");


                var response = _client.PutAsync(url_inspo_add_product + _id, postdataString);
                var responseString = response.Result.Content.ReadAsStringAsync().Result;


                if (response.Result.IsSuccessStatusCode)
                {
                    if (_FromSettings)
                    {
                        Navigation.InsertPageBefore(new MainPage(), this);
                        await Navigation.PopAsync();
                    }
                    else 
                        await Navigation.PushAsync(new EditInspoPage(_id));

                    btnContinue.IsEnabled = true;
                }
                else
                {
                    btnContinue.IsEnabled = true;
                }

            }
            catch (Exception ex)
            {
                try
                {
                    Insights.Report(ex);
                    await DisplayAlert("Useless Tap", "I tried to upload the products, but I failed. Horribly.", "Be better");
                    btnContinue.IsEnabled = true;

                }
                catch
                {
                    await DisplayAlert("Error Tap", "Error when trying to connect! Something is wrong! HELP!", "Jesus, calm down already.");
                    btnContinue.IsEnabled = true;

                }
            }
        }
    }
}