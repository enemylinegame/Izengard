using UI;
using Configs;
using NewBuildingSystem;
using UnityEngine;
using UserInputSystem;


public class GameInit
{
    private UserInput _userInput;
    
    
    public GameInit(Controller controller, ConfigsHolder configs, AudioSource clickAudioSource, Canvas canvas, 
        Grid grid, GameObject plane, GameObject buildings, Map map)
    {
        var UIPanelInit = new UIPanelsInitialization(configs.UIElementsConfig, canvas);
        var userInputController = new UserInputController();
        _userInput = userInputController.UserInput;
        var rayCastController = new RayCastController(_userInput, configs.GameConfig);
        var buildingFactory = new BuildingsFactory(configs.BuildingsSettings, rayCastController, configs.ObjectsHolder, plane, grid, 
            buildings, UIPanelInit);
        var MapController = new MapController(map, configs.GameConfig.BuildingStageMapSize);

        controller.Add(rayCastController);
    }
}