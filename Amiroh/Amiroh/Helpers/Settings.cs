// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Amiroh.Helpers
{
	/// <summary>
	/// This is the Settings static class that can be used in your Core solution or in any
	/// of your client applications. All settings are laid out the same exact way with getters
	/// and setters. 
	/// </summary>
	public static class Settings
	{
		private static ISettings AppSettings
		{
			get
			{
				return CrossSettings.Current;
			}
		}

		#region Setting Constants

		private const string LoginKey = "login_key";
		private static readonly string LoginDefault = "no";

        private const string IdKey = "id_key";
        private static readonly string IdDefault = "empty";

        private const string UsernameKey = "username_key";
        private static readonly string UsernameDefault = "empty";

        private const string DescriptionKey = "description_key";
        private static readonly string DescriptionDefault = "empty";

        private const string ProfilePictureKey = "profilepicture_key";
        private static readonly string ProfilePictureDefault = "empty";




        #endregion


        public static string LoginSettings
		{
			get
			{
				return AppSettings.GetValueOrDefault(LoginKey, LoginDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue(LoginKey, value);
			}
		}

        public static string IdSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(IdKey, IdDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(IdKey, value);
            }
        }

        public static string UsernameSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(UsernameKey, UsernameDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(UsernameKey, value);
            }
        }

        public static string DescriptionSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(DescriptionKey, DescriptionDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(DescriptionKey, value);
            }
        }

        public static string ProfilePictureSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(ProfilePictureKey, ProfilePictureDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(ProfilePictureKey, value);
            }
        }

    }
}