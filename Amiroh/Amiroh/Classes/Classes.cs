using Amiroh.Controllers;
using ModernHttpClient;
using Newtonsoft.Json;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
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
        public DateTime UserCreated { get; set; }
        public string[] BlockedUsers { get; set; }





    }

    public class Report
    {
        public string UsernameReported { get; set; }
        public string UserIdReported { get; set; }
        public string UsernameHasMadeComplaint { get; set; }
        public string UserIdHasMadeComplaint { get; set; }
        public string ReasonForReport { get; set; }
        public DateTime DateOfCompaint { get; set; }


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

    public class Inspo : IComparable, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _title;
        private string _description;
        private string _URL;
        private string[] _hasbeenlikedby;
        private Comment[] _comments;
        private int _points;
        private string[] _tags;
        private string[] _productsused;



        public string _Id { get; set; }
        public string Username { get; set; }
        public string Title {
            get { return _title; }
            set
            {
                if (_title == value)
                    return;

                _title = value;

                OnPropertyChanged();
            }
        }


        public string Description
        {
            get { return _description; }
            set
            {
                if (_description == value)
                    return;

                _description = value;

                OnPropertyChanged();
            }
        }
        public string URL
        {
            get { return _URL; }
            set
            {
                if (_URL == value)
                    return;

                _URL = value;

                OnPropertyChanged();
            }
        }
        public string UserId { get; set; }

        public string[] HasBeenLikedBy
        {
            get { return _hasbeenlikedby; }
            set
            {
                if (_hasbeenlikedby == value)
                    return;

                _hasbeenlikedby = value;

                OnPropertyChanged();
            }
        }
        public Comment[] Comments
        {
            get { return _comments; }
            set
            {
                if (_comments == value)
                    return;

                _comments = value;

                OnPropertyChanged();
            }
        }
        
        public int Points
        {
            get { return _points; }
            set
            {
                if (_points == value)
                    return;

                _points = value;

                OnPropertyChanged();
            }
        }
        public string[] Tags
        {
            get { return _tags; }
            set
            {
                if (_tags == value)
                    return;

                _tags = value;

                OnPropertyChanged();
            }
        }
        public string[] ProductsUsed
        {
            get { return _productsused; }
            set
            {
                if (_productsused == value)
                    return;

                _productsused = value;

                OnPropertyChanged();
            }
        }
        public string UploadId { get; set; }
        public DateTime InspoCreated { get; set; }

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
                    errorMessage = "ERROR";
                    return errorMessage;
                }

                var _image = await CrossMedia.Current.PickPhotoAsync();

                if (_image == null)
                {
                    errorMessage = "ERROR";
                    return errorMessage;
                }



                var imageName = await Controller.UploadFileAsync("inspo", _image.GetStream());

                InspoURL = imageName.ToString();

                return InspoURL;



            }
            catch (Exception er)
            {
                Insights.Report(er);
                return errorMessage = "ERROR";
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
                    errorMessage = "ERROR";
                    return errorMessage;
                }

                var _image = await CrossMedia.Current.PickPhotoAsync();

                if (_image == null)
                {
                    errorMessage = "ERROR";
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
                return errorMessage = "ERROR";
            }



            //await AzureStorage.UploadFileAsync(ContainerType.Text, new MemoryStream(byteData));


            //send PUT request to database with imageID(url) to User.ProfilePicture
        }
        
    }
}
