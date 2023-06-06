using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameController : IOnController, IOnStart, IOnUpdate, IDisposable
{
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
            if(_levelController.MainBuilding.CurrentHealth <= 0) _endGameScreen.BackGroundGameOverScreen.gameObject.SetActive(true);
        }
        
    }
    

    private void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void Dispose()
    {
        _endGameScreen.RestartBtn.onClick.RemoveAllListeners();
    }


}
