using Controllers.Worker;
using ResourceSystem;
using System;
using System.Collections.Generic;
using UnityEngine;


public class WorkersCraftTeam : IOnUpdate, IOnController
{
    public WorkersCraftTeam()
    {
        _model = new WorkersTeamModel();

        _workersAreGoingToWork = new Dictionary<int, WorkerCraftWork>();
        _workingWorkers = new Dictionary<int, WorkerCraftWork> ();
    }

    public int SendWorkerToWork(Vector3 startPalce, Vector3 targetPalce,
         WorkerController workerController, 
            IWorkerWork beginOfWork, IWorkerWork work, IWorkerWork endOfWork)
    {
        var workingWorker = new WorkerCraftWork() { 
            Worker = workerController, 
            BeginOfWork = beginOfWork, Work = work, EndOfWork = endOfWork
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
        WorkerCraftWork workingWorker = null;
        if (_workersAreGoingToWork.TryGetValue(workerId,
            out workingWorker))
        {
            _workersAreGoingToWork.Remove(workingWorker.Worker.WorkerId);
        }
        else if (_workingWorkers.TryGetValue(workerId,
            out workingWorker))
        {
            _workingWorkers.Remove(workerId);
        }
        else
            return null;

        workingWorker.BeginOfWork = null;
        workingWorker.Work = null;
        workingWorker.EndOfWork = null;
        var worker = workingWorker.Worker;
        workingWorker.Worker = null;

        return worker;
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
                workerId, out WorkerCraftWork workingWorker))
            return;

        _workersAreGoingToWork.Remove(workerId);
        _workingWorkers.Add(workerId, workingWorker);
    }

    private void ClearWorks(Dictionary<int, WorkerCraftWork> works, List<WorkerController> workers)
    {
        
        foreach (var worker in works)
        {
            WorkerCraftWork work = worker.Value;
            work.Worker.OnMissionCompleted -= OnReadyToWork;
            work.BeginOfWork = null;
            work.Work = null;
            work.EndOfWork = null;

            workers.Add(work.Worker);
            work.Worker = null;
        }
        works.Clear();
    }

    public void Dispose(List<WorkerController> workers)
    {
        ClearWorks(_workersAreGoingToWork, workers);
        ClearWorks(_workingWorkers, workers);
    }

    class WorkerCraftWork
    {
        public WorkerController Worker;
        public IWorkerWork Work;
        public IWorkerWork BeginOfWork;
        public IWorkerWork EndOfWork;
    }

    private WorkersTeamModel _model;

    private Dictionary<int, WorkerCraftWork> _workersAreGoingToWork;
    private Dictionary<int, WorkerCraftWork> _workingWorkers;

}
