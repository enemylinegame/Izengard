using UnityEngine;

namespace Izengard.UnitSystem.View
{
    public abstract class BaseUnitView : MonoBehaviour
    {
        private Transform _selfTransform;
        public Transform SelfTransform => _selfTransform;
        
        public void ChangeActiveState(bool state)
        {
            gameObject.SetActive(state);
        }

        public abstract void ChangeHealth(int hpValue);
        public abstract void ChangeSize(float sizeValue);
        public abstract void ChangeArmor(int armorValue);

        private void OnEnable()
        {
            _selfTransform ??= transform;
        }   
    }
}
