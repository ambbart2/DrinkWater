using System;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Settings;
using DrinkWater.Configuration;
using Zenject;

namespace DrinkWater.UI.ViewControllers
{
	public class DrinkWaterSettingsViewController : IInitializable, IDisposable
	{
		private readonly PluginConfig _pluginConfig;

		public DrinkWaterSettingsViewController(PluginConfig pluginConfig)
		{
			_pluginConfig = pluginConfig;
		}

		[UIValue("enabled-bool")]
		private bool EnabledValue
        {
            get => _pluginConfig.EnablePlugin;
            set => _pluginConfig.EnablePlugin = value;
        }

        [UIValue("show-gif-bool")]
        private bool ShowGifValue
        {
            get => _pluginConfig.ShowGIFs;
            set => _pluginConfig.ShowGIFs = value;
        }

        [UIValue("wait-duration-int")]
        private int WaitDurationValue
        {
            get => _pluginConfig.WaitDuration;
            set => _pluginConfig.WaitDuration = value;
        }

        [UIValue("enable-playtime-bool")]
        private bool EnableByPlaytimeValue
        {
            get => _pluginConfig.EnableByPlaytime;
            set => _pluginConfig.EnableByPlaytime = value;
        }

        [UIValue("enable-playtime-count-bool")]
        private bool EnableByPlaytimeCount
        {
            get => _pluginConfig.EnableByPlaycount;
            set => _pluginConfig.EnableByPlaycount = value;
        }

        [UIValue("playtime-warning-int")]
        private int PlaytimeBeforeWarningValue
        {
            get => _pluginConfig.PlaytimeBeforeWarning;
            set => _pluginConfig.PlaytimeBeforeWarning = value;
        }

        [UIValue("playcount-warning-int")]
        private int PlaycountBeforeWarningValue
        {
            get => _pluginConfig.PlaycountBeforeWarning;
            set => _pluginConfig.PlaycountBeforeWarning = value;
        }

        [UIAction("#apply")]
        public void OnApply()
        {
            _pluginConfig.EnablePlugin = EnabledValue;
            _pluginConfig.ShowGIFs = ShowGifValue;
            _pluginConfig.WaitDuration = WaitDurationValue;
            _pluginConfig.EnableByPlaytime = EnableByPlaytimeValue;
            _pluginConfig.EnableByPlaycount = EnableByPlaytimeCount;
            _pluginConfig.PlaytimeBeforeWarning = PlaytimeBeforeWarningValue;
            _pluginConfig.PlaycountBeforeWarning = PlaycountBeforeWarningValue;
        }
		
		public void Initialize()
		{
			BSMLSettings.instance.AddSettingsMenu("Drink Water", $"{nameof(DrinkWater)}.UI.Views.SettingsView.bsml", this);
		}

		public void Dispose()
		{
			if (BSMLSettings.instance != null) BSMLSettings.instance.RemoveSettingsMenu(this);
		}
	}
}