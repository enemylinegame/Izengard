using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameController : IOnController, IOnStart, IOnUpdate, IDisposable
{
    private EndGameScreen _endGameScreen;
    private float _t;

    public EndGameController(EndGameScreen endGameScreen)
    {
        _endGameScreen = endGameScreen;
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
