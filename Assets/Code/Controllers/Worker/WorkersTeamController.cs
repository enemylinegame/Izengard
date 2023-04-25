
using Controllers.Worker;
using System.Collections.Generic;
using UnityEngine;

public class WorkersTeamController
{
    private IList<WorkerController> _controllers;
    private WorkersTeamModel _model;
    private IWorkerTeamView _view;


    public WorkersTeamController(WorkersTeamConfig config, Vector3 initPosition,
        IWorkerTeamView teamView)
    {
        _view = teamView;

        _model = new WorkersTeamModel();
        _model.StartPosition = initPosition;
        _model.WorkerModels = new List<WorkerModel>();

        _controllers = new List<WorkerController>();

        CreateWorkers(config);
    }

    public void SendTeamToPlace(Vector3 place)
    { 

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
                WorkTimeLeft  = 0 
            };

            _model.WorkerModels.Add(workerModel);
            _controllers.Add(new WorkerController(workerModel, workerView));
        }
    }


}
