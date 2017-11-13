using Amiroh.Controllers;
using Newtonsoft.Json;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin;

namespace Amiroh.Classes
{
    

public class MainUser
    {
        public readonly static MainUser MainUserID = new MainUser();

        public string ID { get; set; }
        public string Username { get; set; }
        public string ProfileDescription { get; set; }
        public string ProfilePicture { get; set; }


    }
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string ProfileDescription { get; set; }
        public string ProfilePicture { get; set; }
        public string Password { get; set; }
        public string[] Inspos { get; set; }
        public string Salt { get; set; }



    }

    public class Inspo : IComparable
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
        public string UserId { get; set; }
        public string[] Comments { get; set; }
        public int Claps { get; set; }
        public string[] Tags { get; set; } 
        public string[] ProductsUsed { get; set; }

        public int CompareTo(object obj)
        {
            Inspo inspo = obj as Inspo;
            if (inspo == null)
            {
                throw new ArgumentException("Object is not Inspo");
            }
            return this.Claps.CompareTo(inspo.Claps);
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

                //if (file == null)
                //{
                //    errorPick.Text = "Please pick a photo!";
                //    return;
                //}

                var imageName = await Controller.UploadFileAsync("mycontainer", _image.GetStream());

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

                //if (file == null)
                //{
                //    errorPick.Text = "Please pick a photo!";
                //    return;
                //}

                var imageName = await Controller.UploadFileAsync("mycontainer", _image.GetStream());

                ProfilePictureURL = imageName.ToString();

                return ProfilePictureURL;



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
