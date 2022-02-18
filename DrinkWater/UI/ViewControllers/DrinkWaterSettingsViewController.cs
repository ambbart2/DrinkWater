using System;
using System.Collections.Generic;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Settings;
using DrinkWater.Configuration;
using DrinkWater.Utils;
using SiraUtil.Logging;
using Zenject;

namespace DrinkWater.UI.ViewControllers
{
	public class DrinkWaterSettingsViewController : IInitializable, IDisposable
	{
		private readonly SiraLog _siraLog;
		private readonly PluginConfig _pluginConfig;

		public DrinkWaterSettingsViewController(SiraLog siraLog, PluginConfig pluginConfig)
		{
			_siraLog = siraLog;
			_pluginConfig = pluginConfig;
		}

		[UIValue("enabled-bool")]
		private bool EnabledValue
        {
            get => _pluginConfig.EnablePlugin;
            set => _pluginConfig.EnablePlugin = value;
        }

        [UIValue("show-image-bool")]
        private bool ShowGifValue
        {
            get => _pluginConfig.ShowImages;
            set => _pluginConfig.ShowImages = value;
        }

        [UIValue("image-source")]
        private string ImageSource
        {
	        get => _pluginConfig.ImageSource.ToString();
	        set => _pluginConfig.ImageSource = (ImageSources.Sources) Enum.Parse(typeof(ImageSources.Sources), value);
        }

        [UIValue("image-sources-list")] 
        private readonly List<object> _imageSourcesList = new List<object>();
	        
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
            _pluginConfig.ShowImages = ShowGifValue;
            _pluginConfig.ImageSource = (ImageSources.Sources) Enum.Parse(typeof(ImageSources.Sources), ImageSource);
            _pluginConfig.WaitDuration = WaitDurationValue;
            _pluginConfig.EnableByPlaytime = EnableByPlaytimeValue;
            _pluginConfig.EnableByPlaycount = EnableByPlaytimeCount;
            _pluginConfig.PlaytimeBeforeWarning = PlaytimeBeforeWarningValue;
            _pluginConfig.PlaycountBeforeWarning = PlaycountBeforeWarningValue;
        }
		
		public void Initialize()
		{
			BSMLSettings.instance.AddSettingsMenu("Drink Water", $"{nameof(DrinkWater)}.UI.Views.SettingsView.bsml", this);

			_imageSourcesList.Clear();
			foreach (var source in Enum.GetNames(typeof(ImageSources.Sources)))
			{
				_imageSourcesList.Add(source);
			}
			_siraLog.Info(_imageSourcesList.Count);
		}

		public void Dispose()
		{
			if (BSMLSettings.instance != null)
			{
				_imageSourcesList.Clear();
				BSMLSettings.instance.RemoveSettingsMenu(this);
			}
		}
	}
}