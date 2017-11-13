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
            category1.GestureRecognizers.Add(new TapGestureRecognizer(CategoryTapped_1));
            category2.GestureRecognizers.Add(new TapGestureRecognizer(CategoryTapped_2));
            category3.GestureRecognizers.Add(new TapGestureRecognizer(CategoryTapped_3));
            category4.GestureRecognizers.Add(new TapGestureRecognizer(CategoryTapped_4));
            category5.GestureRecognizers.Add(new TapGestureRecognizer(CategoryTapped_5));
            category6.GestureRecognizers.Add(new TapGestureRecognizer(CategoryTapped_6));
            category7.GestureRecognizers.Add(new TapGestureRecognizer(CategoryTapped_7));
            category8.GestureRecognizers.Add(new TapGestureRecognizer(CategoryTapped_8));
        }

        private async void CategoryTapped_1(View arg1, object arg2)
        {
            await Navigation.PushAsync(new DiscoverPageCategoryOverview("Category 1"));
        }
        private async void CategoryTapped_2(View arg1, object arg2)
        {
            await Navigation.PushAsync(new DiscoverPageCategoryOverview("Category 2"));
        }
        private async void CategoryTapped_3(View arg1, object arg2)
        {
            await Navigation.PushAsync(new DiscoverPageCategoryOverview("Category 3"));
        }
        private async void CategoryTapped_4(View arg1, object arg2)
        {
            await Navigation.PushAsync(new DiscoverPageCategoryOverview("Category 4"));

        }
        private async void CategoryTapped_5(View arg1, object arg2)
        {
            await Navigation.PushAsync(new DiscoverPageCategoryOverview("Category 5"));
        }
        private async void CategoryTapped_6(View arg1, object arg2)
        {
            await Navigation.PushAsync(new DiscoverPageCategoryOverview("Category 6"));
        }
        private async void CategoryTapped_7(View arg1, object arg2)
        {
            await Navigation.PushAsync(new DiscoverPageCategoryOverview("Category 7"));
        }
        private async void CategoryTapped_8(View arg1, object arg2)
        {
            await Navigation.PushAsync(new DiscoverPageCategoryOverview("Category 8"));
        }

    }
}