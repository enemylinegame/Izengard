using UnityEngine;

public class WorkerTestEnd : IWorkerWork
{
    public void Produce(float deltaTime)
    {
        Debug.Log("WorkerTestEnd");
    }
}
