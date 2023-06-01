using Controllers.Worker;
using ResourceSystem;
using System;
using System.Collections.Generic;
using UnityEngine;


public class WorkersCraftTeam : IOnUpdate, IDisposable, IOnController
{
    public WorkersCraftTeam()
    {
        _model = new WorkersTeamModel();

        _workersAreGoingToWork = new Dictionary<int, WorkingWorker>();
        _workingWorkers = new Dictionary<int, WorkingWorker> ();
    }

    public int SendWorkerToWork(Vector3 startPalce, Vector3 targetPalce,
         WorkerController workerController, IWorkerWork work)
    {
        var workingWorker = new WorkingWorker() {
            Worker = workerController , Work = work 
        };

        _workersAreGoingToWork.Add(workerController.WorkerId, workingWorker);

        workerController.OnMissionCompleted += OnReadyToWork;
        workerController.GoToPlace(startPalce, targetPalce);
        return workerController.WorkerId;
    }

    public void Pause()
    {
        if (_model.IsPaused)
            return;

        _model.IsPaused = true;
        foreach (var kvp in _workersAreGoingToWork)
            kvp.Value.Worker.Pause();
    }

    public void Resume()
    {
        if (!_model.IsPaused)
            return;

        foreach (var kvp in _workersAreGoingToWork)
            kvp.Value.Worker.Resume();

        _model.IsPaused = false;
    }

    public WorkerController CancelWork(int workerId)
    {
        WorkingWorker workingWorker = null;
        if (_workersAreGoingToWork.TryGetValue(workerId,
            out workingWorker))
        {
            _workersAreGoingToWork.Remove(workingWorker.Worker.WorkerId);
            return workingWorker.Worker;
        }

        if (_workingWorkers.TryGetValue(workerId,
            out workingWorker))
        {
            _workingWorkers.Remove(workingWorker.Worker.WorkerId);
            return workingWorker.Worker;
        }
        return null;
    }

    public void OnUpdate(float deltaTime)
    {
        if (_model.IsPaused)
            return;

        foreach (var worker  in _workersAreGoingToWork)
            worker.Value.Worker.OnUpdate(deltaTime);

        foreach (var work in _workingWorkers)
            work.Value.Work.Produce();
    }

    private void OnReadyToWork(WorkerController workerController)
    {
        workerController.OnMissionCompleted -= OnReadyToWork;
        var workerId = workerController.WorkerId;

        if (!_workersAreGoingToWork.TryGetValue(
                workerId, out WorkingWorker workingWorker))
            return;

        _workersAreGoingToWork.Remove(workerId);
        _workingWorkers.Add(workerId, workingWorker);
    }

    public void Dispose()
    {
        foreach (var worker in _model.Workers)
            worker.Value.OnMissionCompleted -= OnReadyToWork;
    }

    class WorkingWorker
    {
        public WorkerController Worker;
        public IWorkerWork Work;
    }

    private WorkersTeamModel _model;

    private Dictionary<int, WorkingWorker> _workersAreGoingToWork;
    private Dictionary<int, WorkingWorker> _workingWorkers;

}
