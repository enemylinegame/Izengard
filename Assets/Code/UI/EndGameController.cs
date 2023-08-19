using System;
using Code.BuildingSystem;
using Code.Game;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameController : IOnController, IOnStart, IOnUpdate, IDisposable
{

    private GameStateManager _gameStateManager;
    private EndGameScreen _endGameScreen;
    private BuildingFactory _buildingFactory;
    private float _t;

    public EndGameController(GameStateManager gameStateManager, EndGameScreen endGameScreen, 
        BuildingFactory buildingFactory)
    {
        _gameStateManager = gameStateManager;
        _endGameScreen = endGameScreen;
        _buildingFactory = buildingFactory;
    }

    public void OnStart()
    {
        _endGameScreen.RestartBtn.onClick.AddListener(RestartGame);
        _endGameScreen.BackToMenuBtn.onClick.AddListener(BackToMenu);
    }
    
    public void OnUpdate(float deltaTime)
    {
        if (_endGameScreen.BackGroundGameOverScreen.color.a < 1f)
        {
            _t += deltaTime/2;
            _endGameScreen.BackGroundGameOverScreen.color =
                Color.Lerp(new Color(0f, 0f, 0f, 0f), new Color(0f, 0f, 0f, 1f), _t);
        }

        if (_buildingFactory.MainBuilding != null)
        {
            if(_buildingFactory.MainBuilding.CurrentHealth <= 0) 
                _endGameScreen.BackGroundGameOverScreen.gameObject.SetActive(true);
        }
    }
    
    private void RestartGame()
    {
        _gameStateManager.RestartGame();
    }

    private void BackToMenu()
    {
        _gameStateManager.SwitchToMainMenu();
    }

    public void Dispose()
    {
        _endGameScreen.RestartBtn.onClick.RemoveAllListeners();
    }
}
