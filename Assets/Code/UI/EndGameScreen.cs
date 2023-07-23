using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameScreen : MonoBehaviour
{
    [SerializeField] private Button _restartBtn;
    [SerializeField] private Button _backToMenuBtn;
    [SerializeField] private TMP_Text _gameOverText;
    [SerializeField] private Image _backGroundGameOverScreen;

    public Button RestartBtn => _restartBtn;
    public Button BackToMenuBtn => _backToMenuBtn;

    public TMP_Text GameOverText => _gameOverText;

    public Image BackGroundGameOverScreen => _backGroundGameOverScreen;
}
