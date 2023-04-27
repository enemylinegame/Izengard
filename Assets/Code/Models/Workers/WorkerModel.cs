using UnityEngine;

public class WorkerModel
{
    public Vector3 StatrtingPlace { get; set; }
    public Vector3 PlaceOfWork { get; set; }

    public WorkerStates State { get; set; }

    public float TimeOfWork { get; set; }
    public float WorkTimeLeft { get; set; }
}
