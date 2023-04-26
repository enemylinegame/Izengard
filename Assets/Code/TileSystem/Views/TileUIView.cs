using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Code.TileSystem
{
    public class TileUIView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _LVLText;
        [SerializeField] private TMP_Text _nameTile;
        [SerializeField] private Button _upgrade;
        [SerializeField] private Button _destroy;
        [SerializeField] private TMP_Text _unitMax;
        [SerializeField] private TileResourcesView _tileResourcesView;

        public Image Icon => _icon;
        public TMP_Text LvlText => _LVLText;
        public TMP_Text NameTile => _nameTile;
        public TMP_Text UnitMax => _unitMax;
        public Button Upgrade => _upgrade;
        public Button Destroy => _destroy;
        public TileResourcesView TileResourcesView => _tileResourcesView;
    }
}