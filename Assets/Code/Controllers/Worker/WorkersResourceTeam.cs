using Controllers.Worker;
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

        _smokingWorkers = new Dictionary<int, WorkerResourceWork>();
        _activeWorkers = new Dictionary<int, WorkerResourceWork>();
        _readyToWork = new List<int>();
        _readyToSmoke = new List<int>();
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
            _smokingWorkers.Remove(workerId);
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
                work.Work.Produce();
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
            work.Value.Work = null;

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
        public IWorkerWork Work;
    }

    private readonly float _smokeBreakTime;

    private Dictionary<int, WorkerResourceWork> _smokingWorkers;
    private Dictionary<int, WorkerResourceWork> _activeWorkers;
    List<int> _readyToWork;
    List<int> _readyToSmoke;
}
