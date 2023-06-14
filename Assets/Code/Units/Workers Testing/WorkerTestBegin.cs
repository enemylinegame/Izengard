

using UnityEngine;

public class WorkerTestBegin : IWorkerTask
{
    WorkerTestBegin()
    {
    }

    public void Produce()
    {
        Debug.Log("WorkerTestBegin");
    }

}
