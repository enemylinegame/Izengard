using UnityEngine;
using System.Collections.Generic;


namespace CombatSystem.Views
{
    public sealed class WarsView
    {
        private const int FIRST_SLOT_NUMBER = 1;

        private WarsUIView _warsUIView;
        private DefenderSlotView[] _slots;

        private IReadOnlyList<IDefenderUnitView> _defendersList;

        private int _maxDefenders;


        public WarsView(WarsUIView warsUIView)
        {
            _warsUIView = warsUIView;
            CreateSlots();
        }


        private void CreateSlots()
        {
            DefenderSlotUI[] slots = _warsUIView.Slots;

            _slots = new DefenderSlotView[slots.Length];

            for (int i = 0; i < _slots.Length; i++)
            {
                DefenderSlotView newSlot = new DefenderSlotView(slots[i], _warsUIView.UnitDefenderSprite,
                    i + FIRST_SLOT_NUMBER);
                newSlot.OnHireClick += HireButtonClick;
                newSlot.OnDissmisClick += DissmissButtonClick;
                newSlot.OnInBarrackChanged += InBarrackToggleChanged;
                _slots[i] = newSlot;
            }

            _maxDefenders = _slots.Length;
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


        public void SetDefenders(IReadOnlyList<IDefenderUnitView> defendersList)
        {
            if (_defendersList != null)
            {
                ClearDefenders();
            }
            if (defendersList != null)
            {
                _defendersList = defendersList;
                int defendersQuantity = _defendersList.Count;

                if (defendersQuantity > _maxDefenders)
                {
                    if (defendersQuantity > _slots.Length)
                    {
                        defendersQuantity = _slots.Length;
                    }
                    SetMexDefenders(defendersQuantity);
                }

                for (int i = 0; i < _defendersList.Count; i++)
                {
                    if (i < _maxDefenders)
                    {
                        _slots[i].SetUnit(_defendersList[i]);
                    }
                }
            }

        }

        public void ClearDefenders()
        {
            for (int i = 0; i < _maxDefenders; i++)
            {
                DefenderSlotView currentSlot = _slots[i];
                if (currentSlot.IsUsed)
                {
                    currentSlot.RemoveUnit();
                }
            }
            _defendersList = null;
        }

        public void UpdateDefenders()
        {
            if (_defendersList != null)
            {
                int defendersQuantity = _defendersList.Count;
                for (int i = 0; i < _maxDefenders; i++)
                {
                    DefenderSlotView currentSlot = _slots[i];
                    if (i < defendersQuantity)
                    {
                        currentSlot.SetUnit(_defendersList[i]);
                    }
                    else
                    {
                        if (currentSlot.IsUsed)
                        {
                            currentSlot.RemoveUnit();
                        }
                    }
                }
            }
        }

        public void SetMexDefenders(int quantity)
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                DefenderSlotView currentSlot = _slots[i];

                if (i < quantity)
                {
                    if (!currentSlot.IsEnabled)
                    {
                        currentSlot.IsEnabled = true;
                    }
                }
                else
                {
                    if (currentSlot.IsEnabled)
                    {
                        if (currentSlot.IsUsed)
                        {
                            currentSlot.RemoveUnit();
                        }
                        currentSlot.IsEnabled = false;
                    }

                }
            }

            _maxDefenders = quantity;
            if (_maxDefenders > _slots.Length)
            {
                _maxDefenders = _slots.Length;
            }
        }

    }
}
