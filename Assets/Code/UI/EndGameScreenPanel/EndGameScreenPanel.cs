using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EndGameScreenPanel : MonoBehaviour
{
    [FormerlySerializedAs("RestartBtn")] [field: SerializeField] public Button RestartButton;
    [FormerlySerializedAs("BackToMenuBtn")] [field: SerializeField] public Button BackToMenuButton;
    [field: SerializeField] public TMP_Text GameOverText;
    [field: SerializeField] public Image BackGroundGameOverScreen;
}
