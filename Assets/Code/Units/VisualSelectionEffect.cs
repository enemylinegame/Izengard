using UnityEngine;

namespace Code.Units
{
    public class VisualSelectionEffect : MonoBehaviour
    {
        public void On()
        {
            gameObject.SetActive(true);
        }

        public void Off()
        {
            gameObject.SetActive(false);
        }
    }
}