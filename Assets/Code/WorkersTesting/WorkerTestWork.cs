using UnityEngine;

public class WorkerTestWork : IWorkerWork
{
    public void Produce()
    {
        Debug.Log("WorkerTestWork");
    }
}
