using Controllers.Worker;
using UnityEngine;

public class WorkerFactory
{
    private WorkersTeamConfig _config;
    private int _workerCounter;

    Transform _teamTransform;

    public WorkerFactory(WorkersTeamConfig config)
    {
        _config = config;
        _workerCounter = 0;

        _teamTransform =
           new GameObject("WorkerTeam").transform;
    }

    public WorkerController CreateWorker()
    {
        GameObject workerGameObject = GameObject.Instantiate(
                _config.WorkerPrefab, _teamTransform);

        IWorkerView workerView = 
            workerGameObject.GetComponent<IWorkerView>();
        workerView.Deactivate();

        WorkerModel workerModel = new WorkerModel(
                _config.TimeToProcessWork, _workerCounter++);

        return new WorkerController(workerModel, workerView);
    }

    internal void ReleaseWorker(IWorkerView view)
    {
        if (view is MonoBehaviour worker)
        {
            GameObject.Destroy(worker.gameObject);
        }
    }
}
