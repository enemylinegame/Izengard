using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameController : IOnController, IOnStart, IOnUpdate, IDisposable
{ 
    private readonly int _mainMenuSceneId = 0;
    private readonly int _gameSceneId = 1;

    private EndGameScreen _endGameScreen;
    private GeneratorLevelController _levelController;
    private float _t;

    public EndGameController(EndGameScreen endGameScreen, GeneratorLevelController levelController)
    {
        _endGameScreen = endGameScreen;
        _levelController = levelController;
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

        if (_levelController.TowerShot != null)
        {
            if(_levelController.MainBuilding.CurrentHealth <= 0) 
                _endGameScreen.BackGroundGameOverScreen.gameObject.SetActive(true);
        }
    }
    
    private void RestartGame()
    {
        SceneManager.LoadScene(_gameSceneId);
    }

    private void BackToMenu()
    {
        SceneManager.LoadScene(_mainMenuSceneId);
    }

    public void Dispose()
    {
        _endGameScreen.RestartBtn.onClick.RemoveAllListeners();
    }
}
