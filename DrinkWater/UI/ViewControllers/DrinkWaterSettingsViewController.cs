using System;
using System.Collections.Generic;
using System.ComponentModel;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Settings;
using DrinkWater.Configuration;
using DrinkWater.Utils;
using IPA.Loader;
using SiraUtil.Logging;
using SiraUtil.Web.SiraSync;
using SiraUtil.Zenject;
using TMPro;
using Tweening;
using Zenject;

namespace DrinkWater.UI.ViewControllers
{
	public class DrinkWaterSettingsViewController : IInitializable, IDisposable, INotifyPropertyChanged
	{
		private bool _updateAvailable;
		
		[UIComponent("update-text")] 
		private readonly TextMeshProUGUI _updateText = null!;
		
		private readonly SiraLog _siraLog;
		private readonly PluginConfig _pluginConfig;
		private readonly PluginMetadata _pluginMetadata;
		private readonly ISiraSyncService _siraSyncService;
		private readonly TimeTweeningManager _timeTweeningManager;

		public DrinkWaterSettingsViewController(SiraLog siraLog, PluginConfig pluginConfig, UBinder<Plugin, PluginMetadata> pluginMetadata, ISiraSyncService siraSyncService, TimeTweeningManager timeTweeningManager)
		{
			_siraLog = siraLog;
			_pluginConfig = pluginConfig;
			_pluginMetadata = pluginMetadata.Value;
			_siraSyncService = siraSyncService;
			_timeTweeningManager = timeTweeningManager;
		}

		public event PropertyChangedEventHandler? PropertyChanged;
		
		[UIValue("update-available")]
		private bool UpdateAvailable
		{
			get => _updateAvailable;
			set
			{
				_updateAvailable = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UpdateAvailable)));
			}
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

        [UIAction("#post-parse")]
        private async void PostParse()
        {
	        if (!_updateAvailable)
	        {
		        var gitVersion = await _siraSyncService.LatestVersion();
		        if (gitVersion != null && gitVersion > _pluginMetadata.HVersion)
		        {
			        _siraLog.Info($"{nameof(DrinkWater)} v{gitVersion} is available on GitHub!");
			        _updateText.text = $"{nameof(DrinkWater)} v{gitVersion} is available on GitHub!";
			        _updateText.alpha = 0f;
			        UpdateAvailable = true;
			        _timeTweeningManager.AddTween(new FloatTween(0f, 1f, val => _updateText.alpha = val, 0.4f, EaseType.InCubic), _updateText);
		        }
	        }
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