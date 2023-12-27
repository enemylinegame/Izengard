using UnityEngine;
using TMPro;

namespace UI
{
    public class SpawnerHUD : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _spawnerIndexText;

        public void Init(int index) 
        {
            _spawnerIndexText.text = index.ToString();
        }
    }
}
