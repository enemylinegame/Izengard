using Interfaces;
using System.Collections.Generic;


namespace Wave.Interfaces
{
    public interface IEnemySorter
    {
        List<IPoolController<IEnemyController>> SortEnemyList(List<IPoolController<IEnemyController>> list);
    }
}