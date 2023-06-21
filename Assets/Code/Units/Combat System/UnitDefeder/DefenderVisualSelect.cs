using Code.Units;
using UnityEngine;

namespace CombatSystem
{
    public class DefenderVisualSelect
    {
        private VisualSelectionEffect _selectionEffect;

        public DefenderVisualSelect(GameObject root)
        {
            _selectionEffect = root.GetComponentInChildren<VisualSelectionEffect>();
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