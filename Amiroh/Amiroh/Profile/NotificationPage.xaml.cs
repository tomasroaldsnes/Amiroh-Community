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

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Amiroh.Profile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationPage : ContentPage
    {

        string url_user_notification = "http://138.68.137.52:3000/AmirohAPI/users/notification/";
        string url_user_delete_notification = "http://192.168.0.2:3000/AmirohAPI/users/";
        HttpClient _client = new HttpClient(new NativeMessageHandler());
        private ObservableCollection<Notification> notificationList;

        public NotificationPage()
        {

            InitializeComponent();

            var navigationPage = Application.Current.MainPage as NavigationPage;
            navigationPage.BarBackgroundColor = Color.White;
            navigationPage.BarTextColor = Color.Black;



        }
        protected async override void OnAppearing()
        {
            var content = await _client.GetStringAsync(url_user_notification + MainUser.MainUserID.ID);
            var o = JsonConvert.DeserializeObject<List<Notification>>(content);

            notificationList = new ObservableCollection<Notification>(o);

            if (notificationList.Count() != 0)
            {
                notificationList = new ObservableCollection<Notification>(notificationList);
                listviewNotifications.ItemsSource = notificationList;
            }
            else
            {
                NoNewNotifications.Text = "YOU HAVE NO NEW NOTIFICATIONS.";
            }
        }


        private async void Fave_Tapped(object sender, EventArgs e)
        {
            //

        }

        protected async override void OnDisappearing()
        {
            string _url = url_user_delete_notification + MainUser.MainUserID.ID;
            var response = await _client.DeleteAsync(_url);
        }
    }
}