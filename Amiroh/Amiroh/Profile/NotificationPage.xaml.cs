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
using Xamarin;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Amiroh.Profile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationPage : ContentPage
    {

        string url_user_notification = "http://138.68.137.52:3000/AmirohAPI/users/notification/";
        string url_user_delete_notification = "http://138.68.137.52:3000/AmirohAPI/users/notification/";
        HttpClient _client = new HttpClient(new NativeMessageHandler());
        private ObservableCollection<Notification> _notificationList;
        private List<Notification> _nlist;
        public NotificationPage(List<Notification> nList)
        {

            InitializeComponent();

            var navigationPage = Application.Current.MainPage as NavigationPage;
            navigationPage.BarBackgroundColor = Color.White;
            navigationPage.BarTextColor = Color.Black;

            _nlist = nList;


        }
        protected override void OnAppearing()
        {
            
            _notificationList = new ObservableCollection<Notification>(_nlist);

            if (_notificationList.Count() != 0)
            {
                listviewNotifications.ItemsSource = _notificationList;
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
            _notificationList.Clear();

            MainUser.MainUserID.HasNotifications = false;
        }

        private async void Notification_Tapped(object sender, ItemTappedEventArgs e)
        {

            try
            {
                if (listviewNotifications.SelectedItem != null)
                {

                    var obj = e.Item as Notification;

                    await Navigation.PushAsync(new UserPage(obj.Username));
                    listviewNotifications.SelectedItem = null;
                }
               
            }
            catch (Exception ex)
            {
                try
                {
                    Insights.Report(ex);
                    await DisplayAlert("Useless Tap", "I tried to load the inspo, but I failed. Horribly.", "Be better");


                }
                catch
                {
                    await DisplayAlert("Error Tap", "Error when trying to connect! Something is wrong! HELP!", "Jesus, calm down already.");

                }
            }
        }
    }
}