using UnityEngine;
using System.Collections.Generic;

using Controllers;
using Interfaces;
using Code.TileSystem;


namespace CombatSystem.Views
{
    public sealed class WarsView 
    {
        private enum BarrackButtonsStatus
        {
            None    = 0,
            Enter   = 1,
            Kickout = 2
        }


        private const int FIRST_SLOT_NUMBER = 1;

        private WarsUIView _warsUIView;
        private DefenderSlotView[] _slots;
        private GameObject _enterBarrackButton;
        private GameObject _exitBarrackButton;

        private IReadOnlyList<DefenderUnit> _defendersList;
        private List<DefenderSlotView> _selectedSlots;
        private List<DefenderUnit> _unitsInsideBarrack;
        private List<DefenderUnit> _unitsOutsideBarrack;
        private IDefendersManager _defendersManager;


        private int _maxDefenders;
        private BarrackButtonsStatus _barrackButtonsStatus;


        public WarsView(WarsUIView warsUIView)
        {
            _warsUIView = warsUIView;
            _warsUIView.EnterToBarracks.onClick.AddListener(InBarrackButtonClick);
            _warsUIView.ExitFromBarracks.onClick.AddListener(InBarrackButtonClick);
            _warsUIView.DismissButton.onClick.AddListener(GlobalDismissButtonClick);
            _warsUIView.ToOtherTileButton.onClick.AddListener(ToOtherTileButtonClick);

            _enterBarrackButton = _warsUIView.EnterToBarracks.gameObject;
            _exitBarrackButton = _warsUIView.ExitFromBarracks.gameObject;
            _exitBarrackButton.SetActive(false);
            _barrackButtonsStatus = BarrackButtonsStatus.Enter;

            _unitsInsideBarrack = new List<DefenderUnit>();
            _unitsOutsideBarrack = new List<DefenderUnit>();

            CreateSlots();
        }


        public void SetInputController(InputController inputController)
        {
            //_inputController = inputController;
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
                newSlot.OnSelected += SlotSelected;
                _slots[i] = newSlot;
            }

            _maxDefenders = _slots.Length;

            _selectedSlots = new List<DefenderSlotView>();
        }

        private void HireButtonClick(int slotNumber)
        {
            _defendersManager?.HireDefender();
        }

        private void DissmissButtonClick(int slotNumber)
        {
            if (_defendersList != null)
            {
                int index = slotNumber - FIRST_SLOT_NUMBER;
                if (index >= 0 && index < _defendersList.Count)
                {
                    List<DefenderUnit> units = new List<DefenderUnit>(1);
                    units.Add(_defendersList[index]);
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
                    List<DefenderUnit> units = new List<DefenderUnit>(1);
                    units.Add(_defendersList[index]);
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

            if (_unitsOutsideBarrack.Count > 0)
            {
                _defendersManager?.SendToBarrack(_unitsOutsideBarrack);
            }
            else
            {
                _defendersManager?.KickoutFromBarrack(_unitsInsideBarrack);
            }

        }

        private void GlobalDismissButtonClick()
        {
            List<DefenderUnit> units = new List<DefenderUnit>();
            bool isUnitsSelected = false;
            for (int i = 0; i < _slots.Length; i++)
            {
                DefenderSlotView slot = _slots[i];
                if (slot.IsEnabled && slot.IsUsed && slot.IsSelected)
                {
                    units.Add(slot.DefenderUnitView);
                    isUnitsSelected = true;
                }
            }
            if (isUnitsSelected)
            {
                _defendersManager?.DismissDefender(units);
            }
        }

        private void SlotSelected(bool isSelected, int number)
        {
            bool isChanged = false;

            DefenderSlotView slot = _slots[number - FIRST_SLOT_NUMBER];
            if (isSelected)
            {
                if (!_selectedSlots.Contains(slot))
                {
                    _selectedSlots.Add(slot);
                    isChanged = true;
                }
            }
            else
            {
                _selectedSlots.Remove(slot);
                isChanged = true;
            }

            if (isChanged)
            {
                RecalculateDefendersLists();
                UpdateBarracksButtonsStatus();
            }
        }

        private void RecalculateDefendersLists()
        {
            _unitsInsideBarrack.Clear();
            _unitsOutsideBarrack.Clear();

            DefenderSlotView[] slots;

            if (_selectedSlots.Count > 0)
            {
                slots = _selectedSlots.ToArray();
            }
            else
            {
                slots = _slots;
            }

            int outsideBarrackCounter = 0;
            for (int i = 0; i < slots.Length; i++)
            {
                DefenderSlotView slot = slots[i];
                if (slot.IsEnabled && slot.IsUsed)
                {
                    DefenderUnit unit = slot.DefenderUnitView;
                    if (unit.IsInsideBarrack)
                    {
                        _unitsInsideBarrack.Add(unit);
                    }
                    else
                    {
                        _unitsOutsideBarrack.Add(unit);
                        outsideBarrackCounter++;
                    }
                }
            }
        }

        private void UpdateBarracksButtonsStatus()
        {
            if (_unitsOutsideBarrack.Count > 0)
            {
                if (_barrackButtonsStatus != BarrackButtonsStatus.Enter)
                {
                    _exitBarrackButton.SetActive(false);
                    _enterBarrackButton.SetActive(true);
                    _barrackButtonsStatus = BarrackButtonsStatus.Enter;
                }
            }
            else
            {
                if (_barrackButtonsStatus != BarrackButtonsStatus.Kickout)
                {
                    _enterBarrackButton.SetActive(false);
                    _exitBarrackButton.SetActive(true);
                    _barrackButtonsStatus = BarrackButtonsStatus.Kickout;
                }
            }
        }

        private void ToOtherTileButtonClick()
        {
            Debug.Log("WarsView->ToOtherTileButtonClick: Sending to other Tile not implemented yet");
        }

        public void SetDefenders(IReadOnlyList<DefenderUnit> defendersList)
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

                RecalculateDefendersLists();
                UpdateBarracksButtonsStatus();
            }

        }

        public void ClearDefenders()
        {
            for (int i = 0; i < _slots.Length; i++)
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
                RemoveFromSlotsDeletedUnits();

                for (int i = 0; i < _defendersList.Count; i++)
                {
                    DefenderUnit searchedUnit = _defendersList[i];
                    DefenderSlotView firstEmpty = null;
                    bool isFound = false;

                    for (int j = 0; j < _slots.Length; j++)
                    {
                        DefenderSlotView slot = _slots[j];
                        if (slot.IsUsed)
                        {
                            if (slot.DefenderUnitView == searchedUnit)
                            {
                                isFound = true;
                                slot.IsInBarrack = searchedUnit.IsInsideBarrack;
                                break;
                            }
                        }
                        else
                        {
                            if (firstEmpty == null && slot.IsEnabled)
                            {
                                firstEmpty = slot;
                            }
                        }

                    }

                    if (!isFound)
                    {
                        if (firstEmpty != null)
                        {
                            firstEmpty.SetUnit(searchedUnit);
                        }
                    }

                }

                RecalculateDefendersLists();
                UpdateBarracksButtonsStatus();
            }
        }

        private void RemoveFromSlotsDeletedUnits()
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                DefenderSlotView currentSlot = _slots[i];
                if (currentSlot.IsUsed)
                {
                    bool isFound = false;

                    for (int j = 0; j < _defendersList.Count; j++)
                    {
                        if (_defendersList[j] == currentSlot.DefenderUnitView)
                        {
                            isFound = true;
                            break;
                        }
                    }

                    if (!isFound)
                    {
                        currentSlot.RemoveUnit();
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

    }
}
