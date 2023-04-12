using System.Collections.Generic;
using ResourceSystem;

public interface IProduceItem:IProduce
{

   public List<ItemProduct> ProducedItems { get; }     
   
}
