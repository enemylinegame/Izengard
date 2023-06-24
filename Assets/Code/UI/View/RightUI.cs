using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Code.Units.HireDefendersSystem;


namespace Code.UI
{
    public class RightUI : MonoBehaviour
    {
        [SerializeField] private Button _buttonSelectTileFirst;
        [SerializeField] private Button _buttonSelectTileSecond;
        [SerializeField] private Button _buttonSelectTileThird;
        [SerializeField] private HireUnitUIView _hireUnits;
        [SerializeField] private TMP_Text _timer;
        
        public TMP_Text Timer => _timer;
    
        public Button ButtonSelectTileFirst => _buttonSelectTileFirst;
    
        public Button ButtonSelectTileSecond => _buttonSelectTileSecond;
    
        public Button ButtonSelectTileThird => _buttonSelectTileThird;
        public HireUnitUIView HireUnits => _hireUnits;

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
