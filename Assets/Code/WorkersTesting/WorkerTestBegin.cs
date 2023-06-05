

using UnityEngine;

public class WorkerTestBegin : IWorkerWork
{
    public void Produce()
    {
        Debug.Log("WorkerTestBegin");
    }
}
