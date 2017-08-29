// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace nWorksLeaveApp.Helpers
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
        const string UserName = "username";
        private static readonly string UserNameDefault = string.Empty;

        #region Setting Constants

        private const string SettingsKey = "settings_key";
		private static readonly string SettingsDefault = string.Empty;

        #endregion

        public static string Username
        {
            get { return AppSettings.GetValueOrDefault(UserName, UserNameDefault); }
            set { AppSettings.AddOrUpdateValue(UserName, value); }
        }
        const string Password = "password";
        private static readonly string PasswordDefault = string.Empty;

        public static string password
        {
            get { return AppSettings.GetValueOrDefault(Password, PasswordDefault); }
            set { AppSettings.AddOrUpdateValue(Password, value); }
        }
        public static string GeneralSettings
		{
			get
			{
				return AppSettings.GetValueOrDefault(SettingsKey, SettingsDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue(SettingsKey, value);
			}
		}

	}
}