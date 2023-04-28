using Controllers.Worker;
using System.Collections.Generic;

public class WorkersTeamModel
{
    public Dictionary<int, WorkerController> Workers;
    public List<int> CompletedWorkers;
}
