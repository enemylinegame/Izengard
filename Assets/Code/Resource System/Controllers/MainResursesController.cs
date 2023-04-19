using System;
using UnityEngine;

namespace ResourceSystem
{ 
    
    public class MainResursesController : IOnController,IOnStart,IDisposable

    { 
        [SerializeField]
        private TopResUiVew _topUI; 
        [SerializeField]
        private GlobalResourceStock _globalResorcesStock;
        
        
        

        public MainResursesController(GlobalResourceStock globalResorceStock, TopResUiVew topResUIView)
        {
            _globalResorcesStock = globalResorceStock;
            _topUI = topResUIView;
        }

        public void Dispose()
        {
            
            _globalResorcesStock.GlobalResChange -= UpdateTopUIValues;
            _globalResorcesStock.GlobalGoldChange -= UpdateGoldValue;
        }

        public void OnStart()
        {
            _globalResorcesStock.GlobalResChange +=UpdateTopUIValues;
            _globalResorcesStock.GlobalGoldChange += UpdateGoldValue;
            UpdateTopUIValues(_globalResorcesStock.GlobalResStock);
            UpdateGoldValue(_globalResorcesStock.AllGoldHolder);
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
