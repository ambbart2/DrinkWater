using System;
using DrinkWater.Configuration;
using DrinkWater.UI.ViewControllers;
using SiraUtil.Logging;
using SiraUtil.Services;
using Zenject;

namespace DrinkWater.Managers
{
	internal class DrinkWaterManager : IInitializable, IDisposable
	{
		private int _playCount;
		private float _playTime;

		private readonly SiraLog _siraLog;
		private readonly PluginConfig _pluginConfig;
		private readonly ILevelFinisher _levelFinisher;
		private readonly DrinkWaterPanelController _drinkWaterPanelController;
		private readonly StandardLevelScenesTransitionSetupDataSO _standardLevelScenesTransitionSetupData;

		public DrinkWaterManager(SiraLog siraLog, PluginConfig pluginConfig, ILevelFinisher levelFinisher, DrinkWaterPanelController drinkWaterPanelController, StandardLevelScenesTransitionSetupDataSO standardLevelScenesTransitionSetupData)
		{
			_siraLog = siraLog;
			_pluginConfig = pluginConfig;
			_levelFinisher = levelFinisher;
			_drinkWaterPanelController = drinkWaterPanelController;
			_standardLevelScenesTransitionSetupData = standardLevelScenesTransitionSetupData;
		}
		
		private void LevelFinisherOnStandardLevelFinished(LevelCompletionResults obj)
		{
			if (!_pluginConfig.EnablePlugin) return;

			var practiceSettings = _standardLevelScenesTransitionSetupData.practiceSettings;
			if (practiceSettings != null)
				_playTime += (obj.endSongTime - practiceSettings.startSongTime) / practiceSettings.songSpeedMul;
			else
				_playTime += obj.endSongTime / obj.gameplayModifiers.songSpeedMul;
			_playCount += 1;
			
			if (_pluginConfig.EnableByPlaytime && _playTime >= _pluginConfig.PlaytimeBeforeWarning)
			{
				_siraLog.Info("Required play time met");
				_drinkWaterPanelController.displayPanelNeeded = true;
				_playTime = 0f;
			}
			else if (_pluginConfig.EnableByPlaycount && _playCount >= _pluginConfig.PlaycountBeforeWarning)
			{
				_siraLog.Info("Required play count met");
				_drinkWaterPanelController.displayPanelNeeded = true;
				_playCount = 0;
			}
		}

		public void Initialize()
		{
			_levelFinisher.StandardLevelFinished += LevelFinisherOnStandardLevelFinished;
		}

		public void Dispose()
		{
			_levelFinisher.StandardLevelFinished -= LevelFinisherOnStandardLevelFinished;
		}
	}
}