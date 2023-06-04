using Controllers.Worker;
using System.Collections.Generic;
using System.Linq;


public class WorkersFreeTeam
{
    public WorkersFreeTeam(WorkerFactory workerFactory)
    {
        _model = new WorkersTeamModel();
        _workers = new Dictionary<int, WorkerController>();
        _workerFactory = workerFactory;
    }

    public void CancelWorker(
         WorkerController workerController)
    {
        workerController.CancelWork();
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
        workerController.OnMissionCompleted -= OnWorkerFree;
        var workerId = workerController.WorkerId;
        _workerFactory.ReleaseWorker(workerController.View);
        workerController.Dispose();
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

    private WorkersTeamModel _model;

    private Dictionary<int, WorkerController> _workers;
    private WorkerFactory _workerFactory;
}
