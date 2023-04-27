using System.Collections.Generic;
using UnityEngine;

public class WorkersTeamModel
{

    public IList<WorkerModel> WorkerModels;

    public Vector3 StartPosition { get; set; }

    public float WorkersInterval;
    public Vector3 PlaceOfWork;
}
