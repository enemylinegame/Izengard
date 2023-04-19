using System;
using ResourceSystem.SupportClases;
using UnityEngine;

namespace ResourceSystem
{
 public enum TypeOfMine:int
    {
        Forest=601,
        Textile=602,
        Deers=603,
        Horse=604,
        Iron=605,
        Steel=606,
        MagicStones=607,
    }

    [System.Serializable]
    public class ResourceMine : ScriptableObject,IResourceMine
    {
        public string NameOfMine => _nameOfMine;

        public int ExtractionTime => _extractionTime;

        public int MaxMineAmount { get; }
      
        public Sprite Icon => _icon;
        public int CurrentMineValue => _currentMineValue;
        public int TirID => (int)_thisMineTier;
        public int TypeOfMine => (int)_typeOfMine;

        private string _nameOfMine;
        private int _extractionTime;
        private ResourceHolder _resourceHolderMine;
        private Sprite _icon;
        private int _currentMineValue;
        public Action<ResourceHolder> resurseMined;
        
        private TierNumber _thisMineTier;
        private TypeOfMine _typeOfMine;
        private int _extractionTime1;

        public ResourceMine (ResourceMine mine)
        {
            _nameOfMine = mine.NameOfMine;
            _extractionTime = mine.ExtractionTime;
            //_resourceHolderMine = new ResourceHolder(mine.ResourceHolderMine.ObjectInHolder, mine.ResourceHolderMine.MaxValue, mine.ResourceHolderMine.CurrentValue);
            _icon = mine.Icon;            
        }


        /// <summary>
        /// метод переопределения к-во ресурса в "шахте" для добычи 1 юнитом за раз
        /// </summary>
        /// <param name="value"></param>
        public void SetCurrentValueMine(float value)
        {
          /*  if (value<=_resourceHolderMine.MaxValue)
            {
                _resourceHolderMine.SetCurrentValueHolder(value);
            }
            else
            {
                Debug.Log("нельзя просто так взять, и заполнить шахту выше максимума!");
            }*/
        }
        /// <summary>
        /// метод добычи из шахты возвращает сразу "холдер" для транспортировки ресурса
        /// </summary>
        /// <returns></returns>
        public ResourceHolder MineResource()
        {
            ResourceHolder tempResHolder = new ResourceHolder(_resourceHolderMine.ResourceType, 1000, _currentMineValue);
            //_resourceHolderMine.GetFromHolder(tempResHolder);
            resurseMined?.Invoke(_resourceHolderMine);
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

        public ResourceMine(string name, int time, Sprite icon, int value, TierNumber tier, TypeOfMine typeOfMine)
        {
            _nameOfMine = name;
            _extractionTime = time;
            _icon = icon;
            _currentMineValue = value;
            _thisMineTier = tier;
            _typeOfMine = typeOfMine;
        }
    }
}
