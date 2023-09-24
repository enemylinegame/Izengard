using System;
using ResourceSystem.SupportClases;
using UnityEngine;

namespace ResourceSystem
{
    [System.Serializable]
    public class ResourceMine : ScriptableObject,IResourceMine
    {
        public string NameOfMine => _nameOfMine;
        public int ExtractionTime => _extractionTime;
        public int MaxMineAmount { get; }
      
        public Sprite Icon => _icon;
        public int CurrentMineValue => _currentMineValue;
        public TierNumber MineTier => _thisMineTier;
        public ResourceType ResourceType => _resourceType;
        public Action<ResourceHolder> OnMineResourceEnd;

        private string _nameOfMine;
        private int _extractionTime;
        private ResourceHolder _resourceHolderMine;
        private Sprite _icon;
        private int _currentMineValue;
  
        
        private TierNumber _thisMineTier;
        private ResourceType _resourceType;

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
            OnMineResourceEnd?.Invoke(_resourceHolderMine);
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

        public ResourceMine(string name, int time, Sprite icon, int value, TierNumber tier, ResourceType resourceType)
        {
            _nameOfMine = name;
            _extractionTime = time;
            _icon = icon;
            _currentMineValue = value;
            _thisMineTier = tier;
            _resourceType = resourceType;
        }
    }
}
