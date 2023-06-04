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

        _awaitedWorkers = new Dictionary<int, WorkerResourceWork>();
        _activeWorkers = new Dictionary<int, WorkerResourceWork>();
        _readyAfterAwaiteWorkers = new List<int>();
    }

    public int SendWorkerToMine(Vector3 startPalce, Vector3 targetPalce, 
        WorkerController workerController, IWorkerWork work)
    {
        if (null == workerController || null == work || 
                startPalce == targetPalce)
            return -1;

        _activeWorkers.Add(workerController.WorkerId, new WorkerResourceWork() 
            {Worker = workerController, Work = work, TimeToAvait = 0});

        workerController.OnMissionCompleted += OnMissionIsCompleted;

        workerController.GoToWorkAndReturn(startPalce, targetPalce);
        return workerController.WorkerId;
    }

    public void Pause()
    {
        if (_model.IsPaused)
            return;

        _model.IsPaused = true;
        foreach (var kvp in _activeWorkers)
            kvp.Value.Worker.Pause();
    }

    public void Resume()
    {
        if (!_model.IsPaused)
            return;

        foreach (var kvp in _activeWorkers)
            kvp.Value.Worker.Resume();

        _model.IsPaused = false;
    }

    public WorkerController CancelWork(int workerId)
    {
        WorkerResourceWork work = null;
        if (_activeWorkers.TryGetValue(workerId, out work))
        {
            _activeWorkers.Remove(workerId);
        }
        else if (!_activeWorkers.TryGetValue(workerId, out work))
        {
            _awaitedWorkers.Remove(workerId);
        }
        else
            return null;

        work.Work = null;

        var worker = work.Worker;
        worker.OnMissionCompleted -= OnMissionIsCompleted;
        worker.CancelWork();

        return worker;
    }

    public void OnUpdate(float deltaTime)
    {
        if (_model.IsPaused)
            return;

        foreach (var worker  in _activeWorkers)
            worker.Value.Worker.OnUpdate(deltaTime);

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
                    workerId, out WorkerResourceWork awaitedWorker))
                continue;

            _activeWorkers.Add(workerId, awaitedWorker);
            awaitedWorker.Worker.RepeatGoToWorkAndReturn();
            _awaitedWorkers.Remove(workerId);
        }

        _readyAfterAwaiteWorkers.Clear();
    }

    private void OnMissionIsCompleted(WorkerController workerController)
    {
        int workerId = workerController.WorkerId;
        if (_activeWorkers.TryGetValue(workerId, out WorkerResourceWork work))
        {
            _awaitedWorkers.Add(workerId, work);
            _activeWorkers.Remove(workerId);
            work.Work.Produce();
        }
    }

    private void ClearWorkers(Dictionary<int, WorkerResourceWork> works, 
        List<WorkerController> workers)
    {
        foreach (var work in works)
        {
            work.Value.Worker.OnMissionCompleted -= OnMissionIsCompleted;
            work.Value.Work = null;

            workers.Add(work.Value.Worker);
            work.Value.Worker = null;
        }
        workers.Clear();
    }

    public void Dispose(List<WorkerController> workers)
    {
        ClearWorkers(_awaitedWorkers, workers);
        ClearWorkers(_activeWorkers, workers);
    }

    private WorkersTeamModel _model;

    private class WorkerResourceWork
    {
        public float TimeToAvait;
        public WorkerController Worker;
        public IWorkerWork Work;
    }

    private readonly float _smokeBreakTime;

    private Dictionary<int, WorkerResourceWork> _awaitedWorkers;
    private Dictionary<int, WorkerResourceWork> _activeWorkers;
    List<int> _readyAfterAwaiteWorkers;
}
