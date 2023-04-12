using System;
using System.Threading.Tasks;
using ResourceSystem;
using TMPro;
using UnityEngine;

namespace Code
{
    public class BaseCenterText : MonoBehaviour
    {
        [SerializeField]private TMP_Text _text;
        public BaseCenterText(TMP_Text text)
        {
            _text = text;
        }
        
        public async Task NotificationUI(String text, int time)
        {
            _text.text = text.ToString();
            _text.gameObject.SetActive(true);
            await Task.Delay(time);
            _text.gameObject.SetActive(false);
        }
        
        public async Task LotsOfAlerts(String text, int time)
        {
            _text.text += text.ToString();
            _text.gameObject.SetActive(true);
            await Task.Delay(time);
            _text.gameObject.SetActive(false);
            _text.text = "";
        }
    }
}