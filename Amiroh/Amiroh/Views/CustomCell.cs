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
    public class CustomCell : ViewCell
    {
        CachedImage image = null;

        public CustomCell()
        {
            image = new CachedImage();
           
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
