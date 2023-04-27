using Assets.Code.Controllers.Worker;
using Controllers.Worker;
using System;
using System.Collections.Generic;
using UnityEngine;


public class WorkersTeamController: IOnUpdate, IDisposable
{
    public WorkersTeamController(
        WorkersTeamConfig config, Vector3 initPosition,
        IWorkerTeamView teamView)
    {
        _view = teamView;

        _model = new WorkersTeamModel();
        _model.StartPosition = initPosition;
        _model.WorkerModels = new List<WorkerModel>();
        _model.WorkersInterval = config.WorkersIntervalSec;

        _controllers = new List<WorkerController>();

        CreateWorkers(config);

        _timer = new WorkersTeamTimer();
        _timer.OnTimeOut += SendNextWorker;
    }

    public void OnUpdate(float deltaTime)
    {
        _timer.OnUpdate(deltaTime);

        for (int i = 0; i < _controllers.Count; ++i)
            _controllers[i].OnUpdate(deltaTime);
    }

    public void SendTeamToPlace(Vector3 place)
    {
        _model.PlaceOfWork = place;
        _timer.SetTimer( _model.WorkersInterval, _controllers.Count);
    }

    private void CreateWorkers(WorkersTeamConfig config)
    {
        int workerAmount = _view.WorkersViews.Count;

        for (int i = 0; i < workerAmount; ++i)
        {
            IWorkerView workerView = _view.WorkersViews[i];

            WorkerModel workerModel = new WorkerModel() {  
                StatrtingPlace = _model.StartPosition,
                PlaceOfWork = _model.StartPosition,
                State = WorkerStates.AT_HOME,
                TimeOfWork = config.TimeToProcessWork,
                WorkTimeLeft  = 0,
                WorkerId = i
            };

            _model.WorkerModels.Add(workerModel);
            _controllers.Add(new WorkerController(workerModel, workerView));
        }
    }

    private void SendNextWorker(int workerId)
    {
        _controllers[workerId].OnMissionCompleted += MissionIsCompeted;
        _controllers[workerId].GoToWork(_model.PlaceOfWork);
    }

    private void MissionIsCompeted(int workerId)
    {
        _controllers[workerId].OnMissionCompleted -= MissionIsCompeted;

        Debug.Log("Mission is completed!");
    }

    public void Dispose()
    {
        _timer.OnTimeOut -= SendNextWorker;
    }

    private IList<WorkerController> _controllers;
    private WorkersTeamModel _model;
    private IWorkerTeamView _view;

    private WorkersTeamTimer _timer;
}
