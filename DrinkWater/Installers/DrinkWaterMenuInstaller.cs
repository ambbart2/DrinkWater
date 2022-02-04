using DrinkWater.AffinityPatches;
using DrinkWater.Configuration;
using DrinkWater.Managers;
using DrinkWater.UI.FlowCoordinators;
using DrinkWater.UI.ViewControllers;
using Zenject;

namespace DrinkWater.Installers
{
	internal class DrinkWaterMenuInstaller : Installer
	{
		private readonly PluginConfig _pluginConfig;

		public DrinkWaterMenuInstaller(PluginConfig pluginConfig)
		{
			_pluginConfig = pluginConfig;
		}

		public override void InstallBindings()
		{
			Container.BindInstance(_pluginConfig).AsSingle();
			Container.BindInterfacesAndSelfTo<DrinkWaterManager>().AsSingle();
			
			Container.BindInterfacesTo<ResultsViewControllerPatches>().AsSingle();

			Container.BindInterfacesTo<DrinkWaterSettingsViewController>().AsSingle();
			Container.Bind<DrinkWaterFlowCoordinator>().FromNewComponentOnNewGameObject().AsSingle();
			Container.Bind<DrinkWaterPanelController>().FromNewComponentAsViewController().AsSingle();
		}
	}
}