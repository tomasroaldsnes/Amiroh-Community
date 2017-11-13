using Amiroh.Classes;
using ModernHttpClient;
using Newtonsoft.Json;
using Plugin.Connectivity;
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

namespace Amiroh
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DiscoverPageCategoryOverview : ContentPage
    {
        private string CategoryType = "";
        private ObservableCollection<Inspo> _inspos;
        private ObservableCollection<Inspo> _sortedInspos;
        private string url_photo = "http://192.168.1.10:3050/AmirohAPI/inspos/";
        private HttpClient _client = new HttpClient(new NativeMessageHandler());

        public DiscoverPageCategoryOverview(string category)
        {
            InitializeComponent();
            CategoryType = category;

           
        }

        protected async override void OnAppearing()
        {
            try
            {

                if (CrossConnectivity.Current.IsConnected)
                {

                    var content_p = await _client.GetStringAsync(url_photo);
                    var pI = JsonConvert.DeserializeObject<List<Inspo>>(content_p);

                    _inspos = new ObservableCollection<Inspo>(pI);
                    
                    foreach (var inspo in _inspos)
                    {
                        if (inspo.Tags != null)
                        {
                            if (inspo.Tags.Contains<string>(CategoryType))
                            {
                                _sortedInspos.Add(inspo);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                try
                {
                    Insights.Report(e);
                    await DisplayAlert("Useless", "I tried to load your inspos, but I failed. Horribly.", "Be better");
                }
                catch
                {
                    await DisplayAlert("Aww, maaaaan...", "I tried to load your inspos, but I failed. Horribly.", "Be better");
                }
            }

        }

        private async void InspoTap(int position)
        {
            await Navigation.PushAsync(new DiscoverPageCategory(CategoryType, position));
        }

        public void GridCreation()
        {
            try
            {
                gridDiscoverCategoryType.RowDefinitions = new RowDefinitionCollection();
                gridDiscoverCategoryType.ColumnDefinitions = new ColumnDefinitionCollection();

                gridDiscoverCategoryType.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                gridDiscoverCategoryType.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                gridDiscoverCategoryType.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

                for (int MyCount = 0; MyCount < 3; MyCount++)
                {

                    gridDiscoverCategoryType.RowDefinitions.Add(new RowDefinition { Height = new GridLength(115, GridUnitType.Absolute) });

                }



                //set row and columns => column to 1 since AddInspoBtn should always be at position 0,0
                int row = 0;
                int column = 0;

                

                for (int i = 0; i < _inspos.Count(); i++)
                {


                    if (column < 2)
                    {
                        var tappedInspo = new TapGestureRecognizer();
                        tappedInspo.Tapped += (s, e) =>
                        {
                            InspoTap(i);
                        };


                        Image inspo = new Image { Source = _inspos[i].URL, Aspect = Aspect.AspectFill, HeightRequest = 115, WidthRequest = 115, Rotation = -90 };
                        inspo.GestureRecognizers.Add(tappedInspo);
                        gridDiscoverCategoryType.Children.Add(inspo, column, row);
                        column++;
                    }
                    else if (column == 2)
                    {
                        var tappedInspo = new TapGestureRecognizer();
                        tappedInspo.Tapped += (s, e) =>
                        {
                            InspoTap(i);
                        };


                        Image inspo = new Image { Source = _inspos[i].URL, Aspect = Aspect.AspectFill, HeightRequest = 115, WidthRequest = 115, Rotation = -90 };
                        inspo.GestureRecognizers.Add(tappedInspo);
                        gridDiscoverCategoryType.Children.Add(inspo, column, row);
                        column = 0;
                        row++;
                    }
                    else DisplayAlert("Error", "Something went wrong with loading userphotos", "OK");



                }
            }
            catch (Exception ex)
            {
                try
                {
                    Insights.Report(ex);
                    DisplayAlert("Dammit", "I'm not ready to upload an inspo. Like, emotionally, you know?", "*Takes a deep breath*");
                }
                catch
                {
                    DisplayAlert("Dammit", "I'm not ready to upload an inspo. Like, emotionally, you know?", "*Counting backwards from 10*");
                }
            }

        }
    }
}