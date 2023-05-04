using UnityEngine;


namespace CombatSystem.Views
{
    public sealed class WarsView
    {
        private const int FIRST_SLOT_NUMBER = 1;

        private WarsUIView _warsUIView;
        private DefenderSlotView[] _slots;


        public WarsView(WarsUIView warsUIView)
        {
            _warsUIView = warsUIView;
            CreateSlots();
        }


        private void CreateSlots()
        {
            DefenderSlotUI[] slotsTransforms = _warsUIView.Slots;

            _slots = new DefenderSlotView[slotsTransforms.Length];

            for (int i = 0; i < _slots.Length; i++)
            {
                DefenderSlotView newSlot = new DefenderSlotView(slotsTransforms[i], _warsUIView.UnitDefenderSprite,
                    i + FIRST_SLOT_NUMBER);
                newSlot.OnHireClick += HireButtonClick;
                newSlot.OnDissmisClick += DissmissButtonClick;
                newSlot.OnInBarrackChanged += InBarrackToggleChanged;
                _slots[i] = newSlot;
            }

        }

        private void HireButtonClick(int slotNumber)
        {

        }

        private void DissmissButtonClick(int slotNumber)
        {

        }

        private void InBarrackToggleChanged(bool isOn, int slotNumber)
        {

        }

        public void AddDefenderUnit(IDefenderUnitView unit)
        {

        }

        public void RemoveDefenderUnit(IDefenderUnitView unit)
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i].DefenderUnitView == unit)
                {
                    _slots[i].RemoveUnit();
                }
            }
        }

    }
}
