using System.Collections.Generic;
using UnityEngine;

public class WorkerTeamView : MonoBehaviour, IWorkerTeamView
{
    public IList<IWorkerView> WorkersViews { get; set; }
}
