using TMPro;
using UnityEngine;

namespace ResourceMarket
{
    public class ItemsContainerView : MonoBehaviour
    {
        [SerializeField] private Transform _itemPlacement;
        [SerializeField] private bool _isBlockEnabled;
        [SerializeField] private GameObject _blockPannel;
        [SerializeField] private TMP_Text _blockInfoText;

        public Transform ItemPlacement => _itemPlacement;

        private int _valueForUnlock;

        public void Init(int valueForUnlock)
        {
            _valueForUnlock = valueForUnlock;
            _blockInfoText.text = $"Open from {_valueForUnlock} markets!";
            ChangeBlockState(_isBlockEnabled);
        }

        public void CheckBlockState(int value)
        {
            if (!_isBlockEnabled) 
                return;
            
            if(value >= _valueForUnlock)
            {
                ChangeBlockState(false);
            }
        }

        private void ChangeBlockState(bool state)
        {
            _blockPannel.SetActive(state);
            _blockInfoText.enabled = state;
        }
    }
}
