using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BrewSystem.UI
{
    internal class IngridientUI : MonoBehaviour, IDisposable
    {
        [SerializeField]
        private Button _selfButon;
        [SerializeField]
        private TMP_Text _name;
        [SerializeField]
        private Image _icone;

        private string _description = "Ingridient Description";

        private void Awake()
        {
            _selfButon.onClick.AddListener(ShowDescription);
        }

        private void ShowDescription()
        {
            Debug.Log(_description);
        }

        public void InitUI(IIngridienData data)
        {
            _name.text = data.Name;
            _icone.sprite = data.Icon;
            _description = data.Description;
        }

        public void Dispose()
        {
            _selfButon.onClick.RemoveListener(ShowDescription);
        }
    }
}
