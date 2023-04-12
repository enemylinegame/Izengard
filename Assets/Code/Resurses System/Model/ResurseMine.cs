using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResurseSystem
{
 public enum TypeOfMine:int
    {
        forest=601,
        textile=602,
        deers=603,
        horse=604,
        iron=605,
        steel=606,
        magikStones=607,
    }

    [System.Serializable]
    public class ResurseMine : IResurseMine,IIconHolder
    {
        public string NameOfMine => _nameOfMine;
        public float ExtractionTime => _extractionTime;
        public ResurseHolder ResurseHolderMine => _resurseHolderMine;
        public Sprite Icon => _icon;
        public float CurrentMineValue => _currentMineValue;
        public int TirID => (int)_thisMineTir;
        public int TypeOfMine => (int)_typeOfMine;

        [SerializeField]
        private string _nameOfMine;
        [SerializeField]
        private float _extractionTime;
        [SerializeField]
        private ResurseHolder _resurseHolderMine;
        [SerializeField]
        private Sprite _icon;
        [SerializeField]
        private float _currentMineValue;
        public Action<ResurseHolder> resurseMined;
        
        [SerializeField]
        private TirNumber _thisMineTir;
        [SerializeField]
        private TypeOfMine _typeOfMine;
        public ResurseMine (ResurseMine mine)
        {
            _nameOfMine = mine.NameOfMine;
            _extractionTime = mine.ExtractionTime;
            _resurseHolderMine = new ResurseHolder(mine.ResurseHolderMine.ObjectInHolder, mine.ResurseHolderMine.MaxValue, mine.ResurseHolderMine.CurrentValue);
            _icon = mine.Icon;            
        }

        /// <summary>
        /// Метод переопределения времени добычи
        /// </summary>
        /// <param name="time"></param>
        public void SetExtractionTime(float time)
        {
            _extractionTime=time;
        }

        /// <summary>
        /// метод переопределения к-во ресурса в "шахте" для добычи 1 юнитом за раз
        /// </summary>
        /// <param name="value"></param>
        public void SetCurrentValueMine(float value)
        {
            if (value<=_resurseHolderMine.MaxValue)
            {
                _resurseHolderMine.SetCurrentValueHolder(value);
            }
            else
            {
                Debug.Log("нельзя просто так взять, и заполнить шахту выше максимума!");
            }
        }
        /// <summary>
        /// метод добычи из шахты возвращает сразу "холдер" для транспортировки ресурса
        /// </summary>
        /// <returns></returns>
        public ResurseHolder MineResurse()
        {
            ResurseHolder tempResHolder = new ResurseHolder(_resurseHolderMine.ObjectInHolder, 0f, _currentMineValue);
            _resurseHolderMine.GetFromHolder(tempResHolder);
            resurseMined?.Invoke(_resurseHolderMine);
            return tempResHolder;
        }
        
        /// <summary>
        /// метод переопределения иконки шахты
        /// </summary>
        /// <param name="icon"></param>
        public void SetIconMine(Sprite icon)
        {
            _icon = icon;
        }           

        public ResurseMine(string name, float time, ResurseHolder resholder, Sprite icon, int value)
        {
            _nameOfMine = name;
            _extractionTime = time;
            _resurseHolderMine = resholder;
            _icon = icon;
            _currentMineValue = value;
        }
    }
}
