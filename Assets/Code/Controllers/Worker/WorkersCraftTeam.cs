using Controllers.Worker;
using ResourceSystem;
using System;
using System.Collections.Generic;
using UnityEngine;


public class WorkersCraftTeam : IOnUpdate, IDisposable, IOnController
{
    public Action<int> OnMissionCompleted = delegate{ };

    public WorkersCraftTeam()
    {
        
        _model = new WorkersTeamModel();

        _model.Workers = new Dictionary<int, WorkerController>();
        _model.CompletedWorkers = new List<int>();
    }

    public int SendWorkerToWork(Vector3 startPalce, Vector3 targetPalce,
         WorkerController workerController)
    {

        _model.Workers.Add(workerController.WorkerId, workerController);

        workerController.OnMissionCompleted += MissionIsCompleted;

        workerController.GoToPlace(startPalce, targetPalce);
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

        workerController.CancelWork();
        return workerController;
    }

    public void OnUpdate(float deltaTime)
    {
        if (_model.IsPaused)
            return;

        foreach (var worker  in _model.Workers)
            worker.Value.OnUpdate(deltaTime);

        ClearCompletedWorkers();
    }

    private void ClearCompletedWorkers()
    {
        for (int i = 0; i < _model.CompletedWorkers.Count; ++i)
        {
            int workerId = _model.CompletedWorkers[i];
            if (_model.Workers.TryGetValue(workerId, out WorkerController worker))
            {
                _model.Workers.Remove(workerId);

                _workerFactory.ReleaseWorker(worker.View);

                worker.Dispose();
            }
        }

        _model.CompletedWorkers.Clear();
    }

    private void MissionIsCompleted(WorkerController workerController)
    {
        workerController.OnMissionCompleted -= MissionIsCompleted;
        OnMissionCompleted.Invoke(workerController.WorkerId);

        _model.CompletedWorkers.Add(workerController.WorkerId);

    }

    public void Dispose()
    {
        foreach (var worker in _model.Workers)
            worker.Value.OnMissionCompleted -= MissionIsCompleted;
    }

    private WorkersTeamModel _model;
}
