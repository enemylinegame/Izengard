using UnityEngine;

public class WorkerModel
{
    public WorkerModel(float timeToProcessWork, int workerId)
    {
        StatrtingPlace = Vector3.zero;
        State = WorkerStates.NONE;
        TimeOfWork = timeToProcessWork;
        WorkTimeLeft = 0;
        WorkerId = workerId;
    }

    public Vector3 StatrtingPlace { get; set; }

    public WorkerStates State { get; set; }

    public float TimeOfWork { get; set; }
    public float WorkTimeLeft { get; set; }

    public int WorkerId { get; set; }
}
