
using Android.Content;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Amiroh.Views;

[assembly: ExportRenderer(typeof(ScrollViewController), typeof(Amiroh.Droid.Renderers.ScrollExViewRenderer))]
namespace Amiroh.Droid.Renderers
{
    class ScrollExViewRenderer
    {
        public class ScrollBarViewRenderer : ScrollViewRenderer
        {

            public ScrollBarViewRenderer(Context context) : base(context)
            {
                //
            }


            protected override void OnElementChanged(VisualElementChangedEventArgs e)
            {
                base.OnElementChanged(e);

                if (e.OldElement != null || this.Element == null)
                {
                    return;
                }

                if (e.OldElement != null)
                    e.OldElement.PropertyChanged -= OnElementPropertyChanged;

                e.NewElement.PropertyChanged += OnElementPropertyChanged;



            }

            protected void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
            {


                this.HorizontalScrollBarEnabled = false;
                this.VerticalScrollBarEnabled = false;



            }
        }
    }
}