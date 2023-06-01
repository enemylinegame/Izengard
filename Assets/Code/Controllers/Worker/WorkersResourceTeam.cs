using Controllers.Worker;
using ResourceSystem;
using System;
using System.Collections.Generic;
using UnityEngine;


public class WorkersResourceTeam : IOnUpdate, IOnController
{
    public Action<int> OnMissionCompleted = delegate{ };

    public WorkersResourceTeam(float smokeBreakTime)
    {
        _smokeBreakTime = smokeBreakTime;

        _model = new WorkersTeamModel();

        _model.Workers = new Dictionary<int, WorkerController>();
        _model.CompletedWorkers = new List<int>();

        _awaitedWorkers = new Dictionary<int, AwaitedWorker>();
        _readyAfterAwaiteWorkers = new List<int>();
    }

    public int SendWorkerToMine(Vector3 startPalce, Vector3 targetPalce, 
        WorkerController workerController)
    {
        _model.Workers.Add(workerController.WorkerId, workerController);

        workerController.OnMissionCompleted += MissionIsCompleted;

        workerController.GoToWorkAndReturn(startPalce, targetPalce);
        return workerController.WorkerId;
    }

    public void Pause()
    {
        if (_model.IsPaused)
            return;

        _model.IsPaused = true;
        foreach (var kvp in _model.Workers)
            kvp.Value.Pause();
    }

    public void Resume()
    {
        if (!_model.IsPaused)
            return;

        foreach (var kvp in _model.Workers)
            kvp.Value.Resume();

        _model.IsPaused = false;
    }

    public WorkerController CancelWork(int workerId)
    {
        if (!_model.Workers.TryGetValue(workerId, 
            out WorkerController workerController))
            return null;

        workerController.OnMissionCompleted -= MissionIsCompleted;
        workerController.CancelWork();
        return workerController;
    }

    public void OnUpdate(float deltaTime)
    {
        if (_model.IsPaused)
            return;

        foreach (var worker  in _model.Workers)
            worker.Value.OnUpdate(deltaTime);

        CheckAwaitedWorkers(deltaTime);
        StartWorkersAfterAwait();
    }

    private void CheckAwaitedWorkers(float deltaTime)
    {
        foreach (var kvp in _awaitedWorkers)
        {
            float timeToAwait = kvp.Value.TimeToAvait;
            timeToAwait -= deltaTime;
            if (timeToAwait < 0)
            {
                _readyAfterAwaiteWorkers.Add(kvp.Key);
            }
            kvp.Value.TimeToAvait = timeToAwait;
        }
    }

    private void StartWorkersAfterAwait()
    {
        for (int i = 0; i < _readyAfterAwaiteWorkers.Count; ++i)
        {
            int workerId = _readyAfterAwaiteWorkers[i];

            if (!_awaitedWorkers.TryGetValue(
                    workerId, out AwaitedWorker awaitedWorker))
                continue;

            var workerController = awaitedWorker.Worker;
            _model.Workers.Add(workerId, workerController);
            workerController.RepeatGoToWorkAndReturn();
            _awaitedWorkers.Remove(workerId);
        }

        _readyAfterAwaiteWorkers.Clear();
    }

    private void MissionIsCompleted(WorkerController workerController)
    {
        int workerId = workerController.WorkerId;
        _awaitedWorkers.Add(workerId, 
            new AwaitedWorker() 
                { TimeToAvait = _smokeBreakTime, Worker = workerController });

        _model.Workers.Remove(workerId);
    }

    private WorkersTeamModel _model;

    private class AwaitedWorker
    {
        public float TimeToAvait;
        public WorkerController Worker;
    }

    private readonly float _smokeBreakTime = 5.0f;
    private Dictionary<int, AwaitedWorker> _awaitedWorkers;

    List<int> _readyAfterAwaiteWorkers;
}
