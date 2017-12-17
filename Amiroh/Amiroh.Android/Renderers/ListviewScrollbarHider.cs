using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Amiroh.Droid.Renderers;


[assembly: ExportRenderer(typeof(Xamarin.Forms.ListView), typeof(ListviewScrollbarHider))]

namespace Amiroh.Droid.Renderers
{
    class ListviewScrollbarHider : ListViewRenderer
    {
         protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ListView> e)
         {
                base.OnElementChanged(e);
                if (Control != null)
                {
                    Control.VerticalScrollBarEnabled = false;
                }
        }
        
    }
}