using System;
using Code.BuildingSystem;
using Code.Game;
using Code.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameController : IOnController, IOnStart, IOnUpdate, IDisposable
{

    private GameStateManager _gameStateManager;
    private EndGameScreenPanelController _endGameScreenPanel;
    private BuildingFactory _buildingFactory;
    private float _t;

    public EndGameController(GameStateManager gameStateManager, EndGameScreenPanelController endGameScreenPanel, 
        BuildingFactory buildingFactory)
    {
        _gameStateManager = gameStateManager;
        _endGameScreenPanel = endGameScreenPanel;
        _buildingFactory = buildingFactory;
        
        _endGameScreenPanel.GetBackGroundGameOverScreen().gameObject.SetActive(false);
    }

    public void OnStart()
    {
        _endGameScreenPanel.RestartButton += RestartGame;
        _endGameScreenPanel.BackToMenuButton += BackToMenu;
    }
    
    public void OnUpdate(float deltaTime)
    {
        if (_buildingFactory.MainBuilding == null) return;
        if (_buildingFactory.MainBuilding.CurrentHealth >= 0) return;
        
        if (_endGameScreenPanel.GetBackGroundGameOverScreen().color.a < 1f)
        {
            _t += deltaTime/2;
            _endGameScreenPanel.GetBackGroundGameOverScreen().color =
                Color.Lerp(new Color(0f, 0f, 0f, 0f), new Color(0f, 0f, 0f, 1f), _t);
        }
        _endGameScreenPanel.GetBackGroundGameOverScreen().gameObject.SetActive(true);
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
        _endGameScreenPanel.DisposeButtons();
        _endGameScreenPanel.RestartButton -= RestartGame;
        _endGameScreenPanel.BackToMenuButton -= BackToMenu;
    }
}
