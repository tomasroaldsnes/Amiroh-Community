using Amiroh.Classes;
using Microsoft.AppCenter.Analytics;
using ModernHttpClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Amiroh.Feed
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ReportPage : ContentPage
	{
        private Inspo _objInspoReported;

        public ReportPage (Inspo objInspoReported)
		{
			InitializeComponent ();

            _objInspoReported = objInspoReported;
		}

        private async void BlockUser()
        {
            HttpClient block_client = new HttpClient(new NativeMessageHandler());
            string url_addBlockedUser = "http://138.68.137.52:3000/AmirohAPI/users/blockUser/" + MainUser.MainUserID.ID;


            string postNewBlockedUser = JsonConvert.SerializeObject(new { _id = _objInspoReported.UserId });
            var poststringNewBlockedUser = new StringContent(postNewBlockedUser, new UTF8Encoding(), "application/json");


            var responseNewBlockedUser = block_client.PostAsync(url_addBlockedUser, poststringNewBlockedUser);
            var responsestringNewBlockedUser = responseNewBlockedUser.Result.Content.ReadAsStringAsync().Result;

            if (responseNewBlockedUser.Result.IsSuccessStatusCode)
            {
                await Navigation.PushAsync(new MainPage());
            }
        }

        private async void Report_Clicked(object sender, EventArgs e)
        {
            string url_report = "http://138.68.137.52:3000/AmirohAPI/reports";
            HttpClient _client = new HttpClient(new NativeMessageHandler());
            string reportMessage = "Reported " + _objInspoReported._Id + " for:";

            if (switchDrugs.IsToggled)
            {
                reportMessage = reportMessage +  "  Drugs  ";
            }
            if (switchFirearm.IsToggled)
            {
                reportMessage = reportMessage + "  Firearms  ";
            }
            if (switchHarassment.IsToggled)
            {
                reportMessage  = reportMessage +  "  Harrasment  ";
            }
            if (switchHate.IsToggled)
            {
                reportMessage = reportMessage + "  Hate  ";
            }
            if (switchIP.IsToggled)
            {
                reportMessage = reportMessage + "  IP violation  ";
            }
            if (switchNudity.IsToggled)
            {
                reportMessage = reportMessage + "  Nudity  ";
            }
            if (switchSelfHarm.IsToggled)
            {
                reportMessage = reportMessage + "  Self Harm  ";
            }
            if (switchUgh.IsToggled)
            {
                reportMessage = reportMessage + "  Ugh  ";
            }

            

            string postdataJson = JsonConvert.SerializeObject(new {
                usernameReported = _objInspoReported.Username,
                userIdReported = _objInspoReported.UserId,
                usernameHasMadeComplaint = MainUser.MainUserID.Username,
                userIdHasMadeComplaint = MainUser.MainUserID.ID,
                reasonForReport = reportMessage,
                dateOfCompaint = DateTime.Now.ToUniversalTime()
            }); 
            var postdataString = new StringContent(postdataJson, new UTF8Encoding(), "application/json");


            var response = _client.PostAsync(url_report, postdataString);
            var responseString = response.Result.Content.ReadAsStringAsync().Result;


            if (response.Result.IsSuccessStatusCode)
            {
                Analytics.TrackEvent("Inspo has been reported.");

                bool blockUser =  await DisplayAlert("User Reported", "You have reported user " + _objInspoReported.Username + " for content that goes against Amiroh Community's Guidelines. Our support team will look into the report within 24 hours.", "Block User", "OK");

                if (blockUser)
                {
                    BlockUser();
                }
            }
            else
            {
                await DisplayAlert("Error!", "Something went wrong, please try again.", "OK");
                await Navigation.PopAsync();
            }
        }

        private void Block_Tapped(object sender, EventArgs e)
        {
            BlockUser();
        }
    }
}