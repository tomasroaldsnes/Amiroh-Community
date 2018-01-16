using System;
using FFImageLoading.Forms.Droid;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;
using Plugin.Permissions;
using Xamarin.Forms.Platform.Android;
using Android.Graphics;
using FFImageLoading.Config;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Push;

namespace Amiroh.Droid
{
    [Activity(Label = "Amiroh Community", Icon = "@drawable/amirohicon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

           

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            

            CachedImageRenderer.Init(enableFastRenderer: true);

            Push.SetSenderId("50802392920");

            LoadApplication(new App());


        }
    }
}

