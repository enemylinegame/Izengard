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
            ChangeBlockState(_isBlockEnabled);
        }


        public void CheckBlockState(int value)
        {
            if (!_isBlockEnabled) 
                return;
            
            if(value >= _valueForUnlock)
            {
                ChangeBlockState(false);
                return;
            }
            UpdateBlockState(value);
        }

        private void ChangeBlockState(bool state)
        {
            _blockPannel.SetActive(state);
            _blockInfoText.enabled = state;
        }

        private void UpdateBlockState(int amount)
        {
            _blockInfoText.text = $"Need more markets to unlock! ({_valueForUnlock - amount})";
        }
    }
}
