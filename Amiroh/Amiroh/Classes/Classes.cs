using Amiroh.Controllers;
using IncrementalListView.FormsPlugin;
using ModernHttpClient;
using Newtonsoft.Json;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin;
using Xamarin.Forms;

namespace Amiroh.Classes
{
    

    public class MainUser
    {
        public readonly static MainUser MainUserID = new MainUser();

        public string ID { get; set; }
        public string Username { get; set; }
        public string ProfileDescription { get; set; }
        public string ProfilePicture { get; set; }
        public bool HasNotifications { get; set; }
        

    }
    public class User
    {
        public string _Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string ProfileDescription { get; set; }
        public string ProfilePicture { get; set; }
        public string Email { get; set; }
        public string Salt { get; set; }
        public string[] FavedUsers { get; set; }
        public Notification[] Notifications { get; set; }
        public string[] Collections { get; set; }
        public string[] Inspos { get; set; }





    }

    

    public class Notification
    {
        public string Username { get; set; }
        public string Text { get; set; }
        public string URL { get; set; }
        public bool Fave { get; set; }

        public async void PushNotification(string type, string url, string username, string userID)
        {
            try
            {
                string url_user_notification = "http://138.68.137.52:3000/AmirohAPI/users/notification/";
                HttpClient _client = new HttpClient(new NativeMessageHandler());
                
                if (type == "COMMENT")
                {
                     string postdataJson = JsonConvert.SerializeObject(new
                    {
                        username = username,
                        text = "HAS COMMENTED ON YOUR INSPO",
                        URL = url,
                        fave = false
                    });
                    var postdataString = new StringContent(postdataJson, new UTF8Encoding(), "application/json");

                    string _url = url_user_notification + userID;
                    var response = await _client.PostAsync(_url, postdataString);
                }
                else if (type == "POINT")
                {
                   string postdataJson = JsonConvert.SerializeObject(new {
                        username = username,
                        text = "HAS AWARED A POINT FOR YOUR INSPO",
                        URL = url,
                        fave = false
                    });
                    var postdataString = new StringContent(postdataJson, new UTF8Encoding(), "application/json");

                    string _url = url_user_notification + userID;
                    var response = await _client.PostAsync(_url, postdataString);
                }
                else if (type == "FAVE")
                {
                    string postdataJson = JsonConvert.SerializeObject(new
                    {
                        username = username,
                        text = "HAS FAVED YOU",
                        URL = url,
                        fave = true
                    });
                    var postdataString = new StringContent(postdataJson, new UTF8Encoding(), "application/json");

                    string _url = url_user_notification + userID;
                    var response = await _client.PostAsync(_url, postdataString);
                }
            }
            catch (Exception e)
            {
                Insights.Report(e);
            }
        }
    }

    public class Comment
    {
        public string Username { get; set; }
        public string Text { get; set; }
        public string ProfilePicture { get; set; }
    }

    public class Inspo : IComparable
    {
        public string _Id { get; set; }
        public string Username { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
        public string UserId { get; set; }
        public string[] HasBeenLikedBy{ get; set; }
        public Comment[] Comments { get; set; }
        public Notification[] Notifications { get; set; }
        public int Points { get; set; }
        public string[] Tags { get; set; } 
        public string[] ProductsUsed { get; set; }

        public int CompareTo(object obj)
        {
            Inspo inspo = obj as Inspo;
            if (inspo == null)
            {
                throw new ArgumentException("Object is not Inspo");
            }
            return this.Points.CompareTo(inspo.Points);
        }
    }

  
  

    public class ImageUpload
    {
        public static async Task<string> InspoUploadAsync()
        {

            string errorMessage = "";
            string InspoURL = "";
            try
            {
                await CrossMedia.Current.Initialize();


                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    errorMessage = "Make sure camera is available";
                    return errorMessage;
                }

                var _image = await CrossMedia.Current.PickPhotoAsync();

               
                var imageName = await Controller.UploadFileAsync("inspo", _image.GetStream());

                InspoURL = imageName.ToString();

                return InspoURL;



            }
            catch (Exception er)
            {
                Insights.Report(er);
                return errorMessage = "Failure to upload Inspo";
            }


            
        }

        public static async Task<string> ProfilePictureUploadAsync()
        {
            string errorMessage = "";
            string ProfilePictureURL = "";
            try
            {
                await CrossMedia.Current.Initialize();


                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    errorMessage = "Make sure camera is available";
                    return errorMessage;
                }

                var _image = await CrossMedia.Current.PickPhotoAsync();

                if (_image == null)
                {
                    errorMessage = "Please pick a photo!";
                    return errorMessage;
                }
                else
                {

                    var imageName = await Controller.UploadFileAsync("profilepicture", _image.GetStream());

                    ProfilePictureURL = imageName.ToString();

                    return ProfilePictureURL;

                }

            }
            catch (Exception er)
            {
                Insights.Report(er);
                return errorMessage = "Failure to upload Inspo";
            }



            //await AzureStorage.UploadFileAsync(ContainerType.Text, new MemoryStream(byteData));


            //send PUT request to database with imageID(url) to User.ProfilePicture
        }
        
    }
}
