using Controllers.Worker;
using System.Collections.Generic;
using UnityEngine;

public class WorkerFactory
{
    private WorkersTeamConfig _config;
    private int _workerCounter;

    Transform _teamTransform;

    public WorkerFactory(WorkersTeamConfig config)
    {
        _config = config;
        _workerCounter = 0;

        _teamTransform =
           new GameObject("WorkerTeam").transform;
    }

    public WorkerController CreateWorker()
    {
        GameObject workerGameObject = GameObject.Instantiate(
                _config.WorkerPrefab, _teamTransform);

        IWorkerView workerView = 
            workerGameObject.GetComponent<IWorkerView>();
        workerView.Deactivate();

        WorkerModel workerModel = new WorkerModel(
                _config.TimeToProcessWork, _workerCounter++);

        return new WorkerController(workerModel, workerView);
    }
}
