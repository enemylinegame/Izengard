using Code.Units;
using UnityEngine;

namespace CombatSystem
{
    public class DefenderVisualSelect
    {
        private VisualSelectionEffect _selectionEffect;

        public DefenderVisualSelect(GameObject root, GameObject visualEffectPrefab)
        {
            if (visualEffectPrefab)
            {
                GameObject effect = GameObject.Instantiate(visualEffectPrefab, root.transform);
                _selectionEffect = effect.GetComponent<VisualSelectionEffect>();
            }
        }

        public void On()
        {
            _selectionEffect.On();
        }

        public void Off()
        {
            _selectionEffect.Off();
        }
    }
}