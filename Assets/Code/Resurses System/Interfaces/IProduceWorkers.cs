using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuildingSystem
{     
    public interface IProduceWorkers 
    {
        public int CurrentWorkerValue { get; }
    }
}
