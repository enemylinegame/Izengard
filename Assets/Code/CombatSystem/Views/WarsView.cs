using UnityEngine;
using System.Collections.Generic;

using Controllers;
using Interfaces;
using Code.TileSystem;


namespace CombatSystem.Views
{
    public sealed class WarsView : ITileSelector
    {
        private const int FIRST_SLOT_NUMBER = 1;

        private WarsUIView _warsUIView;
        private InputController _inputController;
        private DefenderSlotView[] _slots;

        private IReadOnlyList<DefenderUnit> _defendersList;
        private IDefendersManager _defendersManager;

        private int _maxDefenders;

        private bool _isSendDefendersMode;


        public WarsView(WarsUIView warsUIView)
        {
            _warsUIView = warsUIView;
            _warsUIView.EnterToBarracks.onClick.AddListener(InBarrackButtonClick);
            _warsUIView.DismissButton.onClick.AddListener(GlobalDismissButtonClick);
            _warsUIView.ToOtherTileButton.onClick.AddListener(ToOtherTileButtonClick);
            CreateSlots();
        }


        public void SetInputController(InputController inputController)
        {
            _inputController = inputController;
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
            SendDefendersModeOff();
            _defendersManager?.HireDefender();
        }

        private void DissmissButtonClick(int slotNumber)
        {
            SendDefendersModeOff();

            if (_defendersList != null)
            {
                int index = slotNumber - FIRST_SLOT_NUMBER;
                if (index >= 0 && index < _defendersList.Count)
                {
                    DefenderUnit[] units = new DefenderUnit[1];
                    units[0] = _defendersList[index];
                    _defendersManager?.DismissDefender(units);
                }
            }
        }

        private void InBarrackToggleChanged(bool isOn, int slotNumber)
        {
            if (_defendersList != null)
            {
                int index = slotNumber - FIRST_SLOT_NUMBER;
                if (index >= 0 && index < _defendersList.Count)
                {
                    DefenderUnit[] units = new DefenderUnit[1];
                    units[0] = _defendersList[index];
                    if (isOn)
                    {
                        _defendersManager?.SendToBarrack(units);
                    }
                    else
                    {
                        _defendersManager?.KickoutFromBarrack(units);
                    }
                }
            }
        }

        private void InBarrackButtonClick()
        {
            SendDefendersModeOff();

            _defendersManager?.BarrackButtonClick();
        }

        private void GlobalDismissButtonClick()
        {
            SendDefendersModeOff();

            DefenderUnit[] units = CreateSelectedDefendersArray();
            if (units.Length > 0)
            {
                _defendersManager?.DismissDefender(units);
            }
        }

        private void ToOtherTileButtonClick()
        {
            SendDefendersModeOn();
        }

        private DefenderUnit[] CreateSelectedDefendersArray()
        {
            int selectedSlotsQuantity = CalculateSelectedSlots();
            DefenderUnit[] units = new DefenderUnit[selectedSlotsQuantity];

            if (selectedSlotsQuantity > 0)
            {
                int index = 0;
                for (int i = 0; i < _slots.Length; i++)
                {
                    DefenderSlotView slot = _slots[i];
                    if (slot.IsEnabled && slot.IsUsed && slot.IsSelected)
                    {
                        units[index] = slot.DefenderUnitView;
                        index++;
                    }
                }
                
            }

            return units;
        }

        private int CalculateSelectedSlots()
        {
            int counter = 0;

            for (int i = 0; i < _slots.Length; i++)
            {
                DefenderSlotView slot = _slots[i];
                if (slot.IsEnabled && slot.IsUsed && slot.IsSelected)
                {
                    counter++;
                }
            }

            return counter;
        }

        public void SetDefenders(IReadOnlyList<DefenderUnit> defendersList)
        {
            SendDefendersModeOff();

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
            SendDefendersModeOff();

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

        public void SetDefendersManager(IDefendersManager manager)
        {
            _defendersManager = manager;
        }

        public void Cancel()
        {
            SendDefendersModeOff();
        }

        public void SelectTile(TileView tile)
        {
            SendDefendersModeOff();
            if (tile != null)
            {
                DefenderUnit[] units = CreateSelectedDefendersArray();
                if (units.Length > 0)
                {
                    _defendersManager?.SendToOtherTile(units, tile);
                }
            }
        }

        private void SendDefendersModeOn()
        {
            if (!_isSendDefendersMode)
            {
                _isSendDefendersMode = true;
                _inputController.SetSpecialTileSelector(this);
            }
        }

        private void SendDefendersModeOff()
        {
            if (_isSendDefendersMode)
            {
                _isSendDefendersMode = false;
                _inputController.SetSpecialTileSelector(null);
            }
        }

    }
}
