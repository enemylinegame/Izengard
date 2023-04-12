using System.Collections.Generic;
using EquipmentSystem;
using ResurseSystem;
using UnityEngine;

public interface IProduceItem:IProduce
{

   public List<ItemProduct> ProducedItems { get; }     
   
}
