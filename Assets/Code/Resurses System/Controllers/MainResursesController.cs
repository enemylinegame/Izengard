using System;
using ResurseSystem;
using System.Collections.Generic;
using UnityEngine;

namespace ResurseSystem
{ 
    
    public class MainResursesController : IOnController,IOnStart,IDisposable

    { 
        [SerializeField]
        private TopResUiVew _topUI; 
        [SerializeField]
        private GlobalResurseStock _globalResursesStock;
        
        
        

        public MainResursesController(GlobalResurseStock globalResurseStock, TopResUiVew topResUIView)
        {
            _globalResursesStock = globalResurseStock;
            _topUI = topResUIView;
        }

        public void Dispose()
        {
            
            _globalResursesStock.GlobalResChange -= UpdateTopUIValues;
            _globalResursesStock.GlobalGoldChange -= UpdateGoldValue;
        }

        public void OnStart()
        {
            _globalResursesStock.GlobalResChange +=UpdateTopUIValues;
            _globalResursesStock.GlobalGoldChange += UpdateGoldValue;
            UpdateTopUIValues(_globalResursesStock.GlobalResStock);
            UpdateGoldValue(_globalResursesStock.AllGoldHolder);
        }

        public void UpdateTopUIValues(ResurseStock stock)
        {
            foreach (ResurseHolder holder in stock.HoldersInStock)
            {
                _topUI.UpdateResursesCount(holder.ObjectInHolder.ResurseType, holder.CurrentValue);
            }
        }
        public void UpdateGoldValue(GoldHolder holder)
        {
            _topUI.UpdateResursesCount(holder.GoldObject.ResurseType, holder.CurrentValue);
        }
    }
}
