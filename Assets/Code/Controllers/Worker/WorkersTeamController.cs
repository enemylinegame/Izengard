using Controllers.Worker;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class WorkersTeamController: IOnUpdate, IDisposable, IOnController
{
    public Action<int> OnMissionCompleted = delegate{ };

    public WorkersTeamController(WorkersTeamConfig config)
    {
        _workerFactory = new WorkerFactory(config);

        const float smokeBreakTime = 6.0f;
        _workersResourceTeam = new WorkersResourceTeam(smokeBreakTime);

        _workersCraftTeam = new WorkersCraftTeam();
        _freeWorkers = new Dictionary<int, WorkerController>();
    }

    public int SendWorkerToWork(Vector3 startPalce, Vector3 craftPalce)
    {
        WorkerController workerController = GetWorker();

        _workersCraftTeam.SendWorkerToWork(
            startPalce, craftPalce, workerController);

        return workerController.WorkerId;
    }

    public int SendWorkerToMine(Vector3 startPalce, Vector3 targetPalce)
    {
        WorkerController workerController = GetWorker();

        _workersResourceTeam.SendWorkerToMine(
            startPalce, targetPalce, workerController);

        return workerController.WorkerId;
    }

    public void Pause()
    {
        if (_model.IsPaused)
            return;

        _model.IsPaused = true;
        
        _workersCraftTeam.Pause();
        _workersResourceTeam.Pause();
    }

    public void Resume()
    {
        if (!_model.IsPaused)
            return;

        _model.IsPaused = false;

        _workersCraftTeam.Resume();
        _workersResourceTeam.Resume();
    }

    public void CancelWork(int workerId)
    {
        var worker = _workersResourceTeam.CancelWork(workerId);
        if (null == worker)
        {
            worker = _workersCraftTeam.CancelWork(workerId);
            if (null == worker)
                return;
        }

        worker.OnMissionCompleted += WorkerIsFree;
        _freeWorkers.Add(worker.WorkerId, worker);
    }

    public void OnUpdate(float deltaTime)
    {
        if (_model.IsPaused)
            return;

        _workersResourceTeam.OnUpdate(deltaTime);
        _workersCraftTeam.OnUpdate(deltaTime);

        foreach (var kvp in _freeWorkers)
            kvp.Value.OnUpdate(deltaTime);
    }

    private WorkerController GetWorker()
    {
        if (_freeWorkers.Count > 0)
        {
            var pair = _freeWorkers.ElementAt(0);

            int workerId = pair.Key;
            var worker = pair.Value;
            _freeWorkers.Remove(workerId);

            return worker;
        }
        return _workerFactory.CreateWorker();
    }

    private void WorkerIsFree(WorkerController workerController)
    {
        _freeWorkers.Remove(workerController.WorkerId);
        workerController.OnMissionCompleted -= WorkerIsFree;

        _workerFactory.ReleaseWorker(workerController.View);
        workerController.Dispose();
    }

    public void Dispose()
    {
        foreach (var worker in _model.Workers)
            worker.Value.OnMissionCompleted -= WorkerIsFree;
    }

    private WorkerFactory _workerFactory;
    private WorkersTeamModel _model;

    WorkersResourceTeam _workersResourceTeam;
    WorkersCraftTeam _workersCraftTeam;

    Dictionary<int, WorkerController> _freeWorkers;
}
