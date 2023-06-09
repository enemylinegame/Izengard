

using UnityEngine;

public class WorkerTestBegin : IWorkerWork
{
    WorkerTestBegin()
    { 
    }

    public void Produce()
    {
        Debug.Log("WorkerTestBegin");
    }

    public Resource GetResourceType()
    { 
    }

    public void Cancel()
    {
    }
}
