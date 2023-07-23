using Controllers.Worker;
using System.Collections.Generic;
using System.Linq;


public class WorkersFreeTeam: IOnUpdate
{
    public WorkersFreeTeam(WorkerFactory workerFactory)
    {
        _model = new WorkersTeamModel();
        _workers = new Dictionary<int, WorkerController>();
        _completedWorkers = new List<int>();
        _workerFactory = workerFactory;
    }

    public void FreeWorker(
         WorkerController workerController)
    {
        if (null == workerController)
            return;

        workerController.OnMissionCompleted += OnWorkerFree;
        _workers.Add(workerController.WorkerId, workerController);
    }

    public void Pause()
    {
        if (_model.IsPaused)
            return;

        _model.IsPaused = true;
        foreach (var kvp in _workers)
            kvp.Value.Pause();
    }

    public void Resume()
    {
        if (!_model.IsPaused)
            return;

        foreach (var kvp in _workers)
            kvp.Value.Resume();

        _model.IsPaused = false;
    }

    private void OnWorkerFree(WorkerController workerController)
    {
        _completedWorkers.Add(workerController.WorkerId);
    }

    public WorkerController GetWorker()
    {
        if (0 == _workers.Count)
        {
            return null;
        }
        var pair = _workers.ElementAt(0);

        var worker = pair.Value;
        _workers.Remove(worker.WorkerId);

        return worker;
    }

    public void Dispose()
    {
        foreach (var kvp in _workers)
        {
            var workerController = kvp.Value;

            workerController.OnMissionCompleted -= OnWorkerFree;
            _workerFactory.ReleaseWorker(workerController.View);
            workerController.Dispose();
        }

        _workers.Clear();
    }

    public void OnUpdate(float deltaTime)
    {
        foreach (var worker in _workers)
            worker.Value.OnUpdate(deltaTime);
        
        CheckCompletedWorkers();
    }

    private void CheckCompletedWorkers()
    {
        for (int i = 0; i < _completedWorkers.Count; ++i)
        {
            int workerId = _completedWorkers[i];
            if (_workers.TryGetValue(workerId, out WorkerController worker))
            {
                _workers.Remove(workerId);
                worker.OnMissionCompleted -= OnWorkerFree;
                _workerFactory.ReleaseWorker(worker.View);
                worker.Dispose();
            }
        }
        _completedWorkers.Clear();
    }

    private WorkersTeamModel _model;

    private Dictionary<int, WorkerController> _workers;
    private List<int> _completedWorkers;

    private WorkerFactory _workerFactory;
}
