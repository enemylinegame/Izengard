using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Code.TileSystem
{
    public class ResourceView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _resourceValue;
        [SerializeField] private TMP_Text _resourceText;

        [SerializeField] private Button _resourceAddButton;
        [SerializeField] private Button _resourceRemoveButton;

        public string ResourceValueString 
        {
            get { return _resourceValue.text; }
            set { _resourceValue.text = value; }
        }
        public string ResourceTextString 
        {
            get { return _resourceText.text; }
            set { _resourceText.text = value; }
        }
        public Button ResourceAddButton 
        { 
            get { return _resourceAddButton; }
        }
        public Button ResourceRemoveButton 
        {
            get { return _resourceRemoveButton; }
        }
    }
}

