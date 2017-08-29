using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace nWorksLeaveApp
{
	public static class SettingsConfig
	{
		private static ISettings AppSettings
		{
			get
			{
				return CrossSettings.Current;
			}
		}
		#region Setting Constants

		private const string SettingsKey = "settings_key";
		private static readonly string SettingsDefault = string.Empty;

		#endregion


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

