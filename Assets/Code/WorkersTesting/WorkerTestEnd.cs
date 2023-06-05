using UnityEngine;

public class WorkerTestEnd : IWorkerWork
{
    public void Produce()
    {
        Debug.Log("WorkerTestEnd");
    }
}
