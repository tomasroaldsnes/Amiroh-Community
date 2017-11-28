using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Amiroh
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DiscoverPage : ContentPage
    {
        public DiscoverPage()
        {
            InitializeComponent();
            ctgEyes.GestureRecognizers.Add(new TapGestureRecognizer(Eyes_Tapped));
            ctgLips.GestureRecognizers.Add(new TapGestureRecognizer(Lips_Tapped));
            ctgContouring.GestureRecognizers.Add(new TapGestureRecognizer(Contouring_Tapped));
            ctgEyebrows.GestureRecognizers.Add(new TapGestureRecognizer(Eyebrows_Tapped));
            ctgNight.GestureRecognizers.Add(new TapGestureRecognizer(Night_Tapped));
            ctgDay.GestureRecognizers.Add(new TapGestureRecognizer(Day_Tapped));
            ctgTrending.GestureRecognizers.Add(new TapGestureRecognizer(Trending_Tapped));
            ctgEditor.GestureRecognizers.Add(new TapGestureRecognizer(EditorsPick_Tapped));


        }

        private async void Eyes_Tapped(View arg1, object arg2)
        {
            await Navigation.PushAsync(new DiscoverPageCategoryOverview("Eyes"));
        }
        private async void Lips_Tapped(View arg1, object arg2)
        {
            await Navigation.PushAsync(new DiscoverPageCategoryOverview("Lips"));
        }
        private async void Contouring_Tapped(View arg1, object arg2)
        {
            await Navigation.PushAsync(new DiscoverPageCategoryOverview("Contouring"));
        }
        private async void Eyebrows_Tapped(View arg1, object arg2)
        {
            await Navigation.PushAsync(new DiscoverPageCategoryOverview("Eyebrows"));

        }
        private async void Night_Tapped(View arg1, object arg2)
        {
            await Navigation.PushAsync(new DiscoverPageCategoryOverview("Night"));
        }
        private async void Day_Tapped(View arg1, object arg2)
        {
            await Navigation.PushAsync(new DiscoverPageCategoryOverview("Day"));
        }
        private async void Trending_Tapped(View arg1, object arg2)
        {
            await Navigation.PushAsync(new DiscoverPageCategoryOverview("Trending"));
        }
        private async void EditorsPick_Tapped(View arg1, object arg2)
        {
            await Navigation.PushAsync(new DiscoverPageCategoryOverview("EditorsPick"));
        }

    }
}