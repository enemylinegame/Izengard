using EquipmentSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ResourceSystem
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "Global Resource Stock", menuName = "Resources/Global Resource Stock", order = 1)]
    public class GlobalResorceStock :ScriptableObject
    {
        public ResurseStock GlobalResStock=> _globalResursesStock;
        public GoldHolder AllGoldHolder => _goldHolder;
        public ItemStock AllItemPlayer => _globalItemStock;

        private List<ResurseStock> AllResursesStockPlayer;
        [SerializeField]
        private ResurseStock _globalResursesStock;
        [SerializeField] 
        private GoldHolder _goldHolder;
        [SerializeField]
        private ItemStock _globalItemStock;
        public Action<ResurseStock> GlobalResChange;
        public Action<GoldHolder> GlobalGoldChange;
        public Action<ItemStock> GLobalItemStockChange;

        
        public GlobalResorceStock( ResurseStock resurseStock,GoldHolder holder,ItemStock itemStock)
        {
            _globalResursesStock = new ResurseStock (resurseStock);
            _goldHolder = holder;
            _globalItemStock = new ItemStock (itemStock);
            AllResursesStockPlayer = new List<ResurseStock>();           
            ResetGlobalRes();
        }
        public GlobalResorceStock(GlobalResorceStock globalResorceStock)
        {
            _globalResursesStock = new ResurseStock (globalResorceStock.GlobalResStock);
            AllResursesStockPlayer = new List<ResurseStock>(globalResorceStock.AllResursesStockPlayer);
            _goldHolder = globalResorceStock.AllGoldHolder;
            _globalItemStock = new ItemStock(globalResorceStock._globalItemStock);
            ResetGlobalRes();

        }
        /// <summary>
        /// Метод получения ресурсов из глобального стока для производства ресурса
        /// </summary>
        /// <param name="product">ресурс требующий ресурсов для производства</param>
        public void GetResurseForProduceFromGlobalStock(ResurseProduct product)
        {
            product.PaidCostForProduceProduct(_globalResursesStock);
            GlobalResChange?.Invoke(GlobalResStock);
        }
        /// <summary>
        /// Метод получения ресурсов из глобального стока для производства предмета
        /// </summary>
        /// <param name="product">предмет требующий ресурсов</param>
        public void GetResurseForProduceFromGlobalStock(ItemProduct product)
        {
            product.PaidCostForProduceProduct(_globalResursesStock);
            GlobalResChange?.Invoke(GlobalResStock);
        }
        public void GetResurseForProduceFromGlobalStock(ResourceCost cost)
        {
            cost.GetNeededResource(_globalResursesStock);
            GlobalResChange?.Invoke(GlobalResStock);
        }
        /// <summary>
        /// Метод получения оплаты за предмет из глобального стока
        /// </summary>
        /// <param name="cost">стоимость требующая оплаты</param>
        /// <returns></returns>
        public bool PriceGoldFromGlobalStock(GoldCost cost)
        {
            bool result = false;
            if (cost.Cost <= AllGoldHolder.CurrentValue)
            {
                _goldHolder.GetGold(cost);
                GlobalGoldChange?.Invoke(_goldHolder);
                result = true;
            }
            else Debug.Log("Не хватает золота!");
            return result;
        }
        /// <summary>
        /// Метод добавления золота в глобальный сток
        /// </summary>
        /// <param name="cost">сумма золота для добавления</param>
        public void AddGoldToGLobalStock(GoldCost cost)
        {
            _goldHolder.AddGold(cost);
        }
        /// <summary>
        /// Метод добавления ресурсов в глобальный сток
        /// </summary>
        /// <param name="holder">держатель ресурса добавления</param>
        public void AddResurseToGlobalResurseStock(ResourceHolder holder)
        {
            _globalResursesStock.AddInStock(holder);
            GlobalResChange?.Invoke(GlobalResStock);
        }
        /// <summary>
        /// Add item in global item stock
        /// </summary>
        /// <param name="holder"></param>
        public void AddItemToGlobalResurseStock(ItemСarrierHolder holder)
        {
            _globalItemStock.AddInStock(holder);
        }
        /// <summary>
        /// Return pay for product in global stock
        /// </summary>
        /// <param name="cost"></param>
        public void ReturnPayForGlobalResurseStock(ResourceCost cost)
        {
            Debug.LogError("Commented by V.Chirkov");
          /*  foreach(ResourceHolder holder in cost.ResourcePrice)
            {
                AddResurseToGlobalResurseStock(holder);
            }*/
        }
        /// <summary>
        /// Метод слияния стока с глобальным стоком (Увеличивает максимальный уровень ресурсов в глобальном стоке)
        /// </summary>
        /// <param name="stock">увеличивающий лимит сток</param>
        public void CompileStockWithGlobalResStock(ResurseStock stock)
        {
            _globalResursesStock.CompileStocks(stock);
            GlobalResChange?.Invoke(_globalResursesStock);
        }
        /// <summary>
        /// Compile item stock with global item stock
        /// </summary>
        /// <param name="stock"></param>
        public void ChangeMaxValueOfGlobalResurseStock(ItemStock stock)
        {
            _globalItemStock.CompileStocks(stock);
            GLobalItemStockChange?.Invoke(_globalItemStock);
        }
        /// <summary>
        /// Метод добавления объекта в сток без взимания платы с проверкой типа объекта.
        /// </summary>
        /// <param name="obj">объект для добавления в сток</param>
        /// <param name="value">количество объекта</param>
        public void AddObjectInStock(ScriptableObject obj,float value)
        {
            if (obj is ItemModel)
            {
                _globalItemStock.AddInStock((ItemModel)obj, value);
                GLobalItemStockChange?.Invoke(_globalItemStock);
            }
            if (obj is ResurseCraft)
            {
                _globalResursesStock.AddInStock((ResurseCraft)obj, value);
                GlobalResChange?.Invoke(_globalResursesStock);
            }
        }
        public void AddProductInStock (ResurseProduct product)
        {
            _globalResursesStock.AddInStock(product.ObjectProduct, product.ProduceValue);
        }
        public void AddProductInStock(ItemProduct product)
        {
            _globalItemStock.AddInStock(product.ObjectProduct, product.ProduceValue);
        }
        
        /// <summary>
        /// Сброс параметров глобального стока
        /// </summary>
        public void ResetGlobalRes()
        {
            _globalResursesStock.ResetStockHoldersValue();
            _goldHolder.SetCurrentGold(0);
            _globalItemStock.ResetStockHoldersValue();
            GlobalResChange?.Invoke(_globalResursesStock);
            GlobalGoldChange?.Invoke(_goldHolder);
        }
        
    }
}
