using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Code.Units.HireDefendersSystem;


namespace Code.UI
{
    public class RightPanel : MonoBehaviour
    {
        [field: SerializeField] public Button ButtonSelectTileFirst;
        [field: SerializeField] public Button ButtonSelectTileSecond;
        [field: SerializeField] public Button ButtonSelectTileThird;
        [field: SerializeField] public HireUnitUIView HireUnits;
        [field: SerializeField] public Button OpenMarketButton;
        [field: SerializeField] public TMP_Text Timer;

        #region Comments

        // public Sprite FirstBtnSprite
        // {
        //     set => _firstBtnSprite = value;
        // }
        //
        // public Sprite SecondBtnSprite
        // {
        //     set => _secondBtnSprite = value;
        // }
        //
        // public Sprite ThirdBtnSprite
        // {
        //     set => _thirdBtnSprite = value;
        // }
        //
        // private Sprite _firstBtnSprite;
        // private Sprite _secondBtnSprite;
        // private Sprite _thirdBtnSprite;
        
        //public event Action<int> TileSelected;
        
        // void Start()
        // {
        //     _buttonSelectTileFirst.image.sprite = _firstBtnSprite;
        //     _buttonSelectTileSecond.image.sprite = _secondBtnSprite;
        //     _buttonSelectTileThird.image.sprite = _thirdBtnSprite;
        //     
        //     _buttonSelectTileFirst.onClick.AddListener( () => TileSelected?.Invoke(0));
        //     _buttonSelectTileSecond.onClick.AddListener(() => TileSelected?.Invoke(1));
        //     _buttonSelectTileThird.onClick.AddListener(() => TileSelected?.Invoke(2));
        // }
    
        // private void OnDestroy()
        // {
        //     _buttonSelectTileFirst.onClick.RemoveAllListeners();
        //     _buttonSelectTileSecond.onClick.RemoveAllListeners();
        //     _buttonSelectTileThird.onClick.RemoveAllListeners();
        // }

        #endregion
    }
}
