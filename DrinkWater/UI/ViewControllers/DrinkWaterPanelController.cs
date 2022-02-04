using System.Collections;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using DrinkWater.Configuration;
using DrinkWater.UI.FlowCoordinators;
using HMUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace DrinkWater.UI.ViewControllers
{
    [ViewDefinition("DrinkWater.UI.Views.DrinkWaterPanelView.bsml")]
    [HotReload(RelativePathToLayout = @"..\Views\DrinkWaterPanelView")]
    internal class DrinkWaterPanelController : BSMLAutomaticViewController
    {
        public bool displayPanelNeeded;
        private PanelMode _panelMode;
        private FlowCoordinator? _previousFlowCoordinator;
        private readonly string[] _gifRotation = { "https://media1.tenor.com/images/013d560bab2b0fc56a2bc43b8262b4ed/tenor.gif", "https://i.giphy.com/zWOnltJgKVlsc.gif", "https://i.giphy.com/3ohhwF34cGDoFFhRfy.gif", "https://i.giphy.com/eRBa4tzlbNwE8.gif" };
        
        private PluginConfig _pluginConfig = null!;
        private MainFlowCoordinator _mainFlowCoordinator = null!;
        private ResultsViewController _resultsViewController = null!;
        private FlowCoordinator _drinkWaterFlowCoordinator = null!;
        
        public enum PanelMode
        {
            Continue,
            Restart
        }
        
        [Inject]
        public void Construct(PluginConfig pluginConfig, MainFlowCoordinator mainFlowCoordinator, ResultsViewController resultsViewController, DrinkWaterFlowCoordinator drinkWaterFlowCoordinator)
        {
            _pluginConfig = pluginConfig;
            _mainFlowCoordinator = mainFlowCoordinator;
            _resultsViewController = resultsViewController;
            _drinkWaterFlowCoordinator = drinkWaterFlowCoordinator;
        }

        [UIComponent("text-content")] 
        internal readonly TextMeshProUGUI TextContent = null!;

        [UIComponent("drink-image")] 
        internal readonly ImageView DrinkImage = null!;

        [UIComponent("continue-button")] 
        internal readonly Button ContinueButton = null!;
        
        [UIComponent("continue-button")]
        internal readonly TextMeshProUGUI ContinueButtonText = null!;
        
        public void ShowDrinkWaterPanel(PanelMode mode)
        {
            _previousFlowCoordinator = _mainFlowCoordinator.YoungestChildFlowCoordinatorOrSelf();
            _previousFlowCoordinator.PresentFlowCoordinator(_drinkWaterFlowCoordinator, animationDirection: AnimationDirection.Horizontal);
            displayPanelNeeded = false;
            _panelMode = mode;
        }

        private IEnumerator MakeButtonInteractableDelay(Button button, float duration, float delayStep = 1f, string format = "0", bool showInButton = true)
        {
            var buttonTextContent = ContinueButtonText.text;
            if (showInButton)
                button.SetButtonText(buttonTextContent + (duration > 0 ? " (" + duration.ToString(format) + ")" : ""));
            button.interactable = false;
            while (duration > 0)
            {
                yield return new WaitForSeconds(delayStep);
                duration -= delayStep;
                if (duration < 0) duration = 0;
                if (showInButton)
                    button.SetButtonText(buttonTextContent + (duration > 0 ? " (" + duration.ToString(format) + ")" : ""));
            }
            button.interactable = true;
        }
        
        [UIAction("#post-parse")]
        private void PostParse()
        {
            TextContent.text = (_panelMode == PanelMode.Restart ? "Before restarting this song" : "Before browsing some new songs") + ", drink some water, that's important for your body!";
            StartCoroutine(MakeButtonInteractableDelay(ContinueButton, _pluginConfig.WaitDuration, 0.1f, "0.0"));

            if (_pluginConfig.ShowGIFs)
            // I swear I'm going to PR BSML just to remove the damn loading gif it makes me cry
                DrinkImage.SetImage(_gifRotation[Random.Range(0, _gifRotation.Length)]);
        }
        
        [UIAction("continue-clicked")]
        private void ContinueClicked()
        {
            //TODO: Improve transitions
            _previousFlowCoordinator.DismissFlowCoordinator(_mainFlowCoordinator.YoungestChildFlowCoordinatorOrSelf(), immediately: true);

            switch (_panelMode)
            {
                case PanelMode.Continue:
                    _resultsViewController.ContinueButtonPressed();
                    break;
                case PanelMode.Restart:
                    _resultsViewController.RestartButtonPressed();
                    break;
            }
        }
    }
}
