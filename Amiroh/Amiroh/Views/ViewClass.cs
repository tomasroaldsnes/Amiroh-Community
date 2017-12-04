using Amiroh.Classes;
using FFImageLoading.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Amiroh.Views
{
    class ViewClass
    {
        public class CustomCell : ViewCell
        {
            CachedImage image = null;

            public CustomCell()
            {
                image = new CachedImage() {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    DownsampleHeight = 300,
                    HeightRequest = 300,
                    CacheDuration = TimeSpan.FromDays(1),
                    DownsampleToViewSize = true,
                    RetryCount = 1,
                    RetryDelay = 250,
                    LoadingPlaceholder = "settings.png",
                    ErrorPlaceholder = "error.png",
                    
                };
                View = image;
            }

            protected override void OnBindingContextChanged()
            {
                image.Source = null;
                base.OnBindingContextChanged();

                var item = BindingContext as Inspo;
                if (item != null)
                {
                    image.Source = item.URL;
                }
            }
        }
    }
}
