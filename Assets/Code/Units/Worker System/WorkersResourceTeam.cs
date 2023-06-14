using Controllers.Worker;
using System;
using System.Collections.Generic;
using UnityEngine;


public class WorkersResourceTeam : IOnUpdate
{
    public Action<int> OnMissionCompleted = delegate{ };

    public WorkersResourceTeam(float smokeBreakTime)
    {
        _smokeBreakTime = smokeBreakTime;

        _model = new WorkersTeamModel();

        _smokingWorkers = new Dictionary<int, WorkerResourceWork>();
        _activeWorkers = new Dictionary<int, WorkerResourceWork>();
        _readyToWork = new List<int>();
        _readyToSmoke = new List<int>();
    }

    public int SendWorkerToMine(Vector3 startPalace, Vector3 targetPlace, 
        WorkerController workerController, 
        IWorkerPreparation preparation, IWorkerTask work)
    {
        if (null == workerController || null == work || 
                startPalace == targetPlace)
            return -1;

        _activeWorkers.Add(workerController.WorkerId, new WorkerResourceWork() 
            {Worker = workerController, 
            Preparation = preparation, Task = work, TimeToAvait = 0});

        workerController.OnMissionCompleted += OnMissionIsCompleted;

        workerController.GoToWorkAndReturn(startPalace, targetPlace);
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
        else if (_smokingWorkers.TryGetValue(workerId, out work))
        {
            _smokingWorkers.Remove(workerId);
        }
        else
            return null;

        if (null != work.Preparation)
            work.Preparation.AfterWork();

        var worker = work.Worker;
        work.Task = null;

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

        CheckSmokingWorkers(deltaTime);
        WorkersToSmokingArray();
    }

    private void CheckSmokingWorkers(float deltaTime)
    {
        foreach (var kvp in _smokingWorkers)
        {
            float timeToAwait = kvp.Value.TimeToAvait;
            timeToAwait -= deltaTime;
            if (timeToAwait < 0)
            {
                var worker = kvp.Value.Worker;
                int workerId = worker.WorkerId;
                
                _activeWorkers.Add(workerId, kvp.Value);
                worker.RepeatGoToWorkAndReturn();

                _readyToWork.Add(workerId);
            }
            kvp.Value.TimeToAvait = timeToAwait;
        }

        for (int i = 0; i < _readyToWork.Count; ++i)
        {
            _smokingWorkers.Remove(_readyToWork[i]);
        }
        _readyToWork.Clear();
    }

    private void WorkersToSmokingArray()
    {
        for (int i = 0; i < _readyToSmoke.Count; ++i)
        {
            int workerId = _readyToSmoke[i];
            if (_activeWorkers.TryGetValue(workerId, out WorkerResourceWork work))
            {
                _activeWorkers.Remove(workerId);
                _smokingWorkers.Add(workerId, work);
                work.Task.Produce();
                work.TimeToAvait = _smokeBreakTime;
            }
        }
        _readyToSmoke.Clear();
    }

    private void OnMissionIsCompleted(WorkerController workerController)
    {
        int workerId = workerController.WorkerId;
        _readyToSmoke.Add(workerId);
    }

    private void ClearWorkers(Dictionary<int, WorkerResourceWork> works, 
        List<WorkerController> workers)
    {
        foreach (var work in works)
        {
            work.Value.Worker.OnMissionCompleted -= OnMissionIsCompleted;
            work.Value.Task = null;
            work.Value.Preparation = null;

            workers.Add(work.Value.Worker);
            work.Value.Worker = null;
        }
        workers.Clear();
    }

    public void Dispose(List<WorkerController> workers)
    {
        ClearWorkers(_smokingWorkers, workers);
        ClearWorkers(_activeWorkers, workers);
    }

    private WorkersTeamModel _model;

    private class WorkerResourceWork
    {
        public float TimeToAvait;
        public WorkerController Worker;
        public IWorkerTask Task;
        public IWorkerPreparation Preparation;
    }

    private readonly float _smokeBreakTime;

    private Dictionary<int, WorkerResourceWork> _smokingWorkers;
    private Dictionary<int, WorkerResourceWork> _activeWorkers;
    List<int> _readyToWork;
    List<int> _readyToSmoke;
}
