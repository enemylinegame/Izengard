using System;
using UnityEngine;

namespace ResourceSystem
{ 
    
    public class MainResourcesController : IOnController,IOnStart,IDisposable

    { 
        [SerializeField]
        private TopResUiVew _topUI; 
        [SerializeField]
        private GlobalResourceStock _globalResourcesStock;
        
        
        

        public MainResourcesController(GlobalResourceStock globalResourceStock, TopResUiVew topResUIView)
        {
            _globalResourcesStock = globalResourceStock;
            _topUI = topResUIView;
        }

        public void Dispose()
        {
            
            _globalResourcesStock.GlobalResChange -= UpdateTopUIValues;
            _globalResourcesStock.GlobalGoldChange -= UpdateGoldValue;
        }

        public void OnStart()
        {
            _globalResourcesStock.GlobalResChange +=UpdateTopUIValues;
            _globalResourcesStock.GlobalGoldChange += UpdateGoldValue;
            UpdateTopUIValues(_globalResourcesStock.GlobalResStock);
            UpdateGoldValue(_globalResourcesStock.AllGoldHolder);
        }

        public void UpdateTopUIValues(ResourceStock stock)
        {
            foreach (ResourceHolder holder in stock.HoldersInStock)
            {
                _topUI.UpdateResursesCount(holder.ObjectInHolder.ResourceType, holder.CurrentValue);
            }
        }
        public void UpdateGoldValue(GoldHolder holder)
        {
            _topUI.UpdateResursesCount(holder.GoldObject.ResourceType, holder.CurrentValue);
        }
    }
}
