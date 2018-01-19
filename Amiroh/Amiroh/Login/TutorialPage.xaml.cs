using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Amiroh.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TutorialPage : ContentPage
    {
        private int ClickCounter = 0;
        public TutorialPage()
        {
            InitializeComponent();

            lblImageText.Text = "Tap on the images. See Faved users. Swipe to navigate.";
        }

        private async void Next_Clicked(object sender, EventArgs e)
        {
            ClickCounter++;

            if(ClickCounter == 1)
            {
                imgTutorialImage.Source = "tutdiscover.png";
                lblImageText.Text = "Tap the squares to search for Inspos with certain tags.";
            }
            else if(ClickCounter == 2)
            {
                imgTutorialImage.Source = "tutprofile.png";
                lblImageText.Text = "Change stuff in Settings. See Collection. Upload a Inspo.";
                btnNext.Text = "Ok, got it.";
            }
            else if(ClickCounter == 3)
            {
                await Navigation.PushAsync(new MainPage());
            }
        }
    }
}