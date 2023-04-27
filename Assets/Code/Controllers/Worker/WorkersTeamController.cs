using Controllers.Worker;
using System;
using System.Collections.Generic;
using UnityEngine;


public class WorkersTeamController: IOnUpdate, IDisposable
{
    public Action<int> OnMissionCompleted = delegate{ };

    public WorkersTeamController(
        WorkersTeamConfig config)
    {
        _workerFactory = new WorkerFactory(config);


        _workers = new Dictionary<int, WorkerController>();
        _completedWorkers = new List<int>();
    }

    public int SendWorkerToPlace(Vector3 startPalce, Vector3 targetPalce)
    {
        WorkerController workerController =
            _workerFactory.CreateWorker();

        _workers.Add(workerController.WorkerId, workerController);

        workerController.OnMissionCompleted += MissionIsCompeted;

        workerController.GoToPlace(startPalce, targetPalce);
        return workerController.WorkerId;
    }

    public int SendWorkerToWork(Vector3 startPalce, Vector3 targetPalce)
    {
        WorkerController workerController =
            _workerFactory.CreateWorker();

        _workers.Add(workerController.WorkerId, workerController);

        workerController.OnMissionCompleted += MissionIsCompeted;

        workerController.GoToWorkAndReturn(startPalce, targetPalce);
        return workerController.WorkerId;
    }

    public void CancelWork(int workerId)
    {
        if (!_workers.TryGetValue(workerId, out WorkerController workerController))
            return;

        workerController.CancelWork();
    }

    public void OnUpdate(float deltaTime)
    {
        foreach (var worker  in _workers)
            worker.Value.OnUpdate(deltaTime);

        for (int i = 0; i < _completedWorkers.Count; ++ i)
            _workers.Remove(_completedWorkers[i]);
    }

    private void MissionIsCompeted(WorkerController workerController)
    {
        workerController.OnMissionCompleted -= MissionIsCompeted;
        OnMissionCompleted.Invoke(workerController.WorkerId);

        _completedWorkers.Add(workerController.WorkerId);

        Debug.Log("Mission is completed!");
    }

    public void Dispose()
    {
        foreach (var worker in _workers)
            worker.Value.OnMissionCompleted -= MissionIsCompeted;
    }

    private WorkerFactory _workerFactory;

    private Dictionary<int, WorkerController> _workers;
    private List<int> _completedWorkers;
}
