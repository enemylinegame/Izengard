using Code.UI;
using Configs;
using UnityEngine;
using UserInputSystem;


public class GameInit
{
    private UserInput _userInput;
    
    
    public GameInit(Controller controller, ConfigsHolder configs, AudioSource clickAudioSource, Canvas canvas)
    {
        var UIPanelInit = new UIPanelsInitialization(configs.UIElementsConfig, canvas);
        var userInputController = new UserInputController();
        _userInput = userInputController.UserInput;
        var rayCastController = new RayCastController(_userInput);
    }
}