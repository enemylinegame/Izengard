using UnityEngine;

public class WorkerTestWork : IWorkerWork
{
    public void Produce(float deltaTime)
    {
        Debug.Log("WorkerTestWork");
    }
}
