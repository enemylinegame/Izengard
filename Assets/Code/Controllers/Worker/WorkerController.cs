using System;
using UnityEngine;


namespace Controllers.Worker
{
    public sealed class WorkerController: IOnUpdate
    {
        private WorkerModel _model;
        private IWorkerView _view;

        public Action<int> OnMissionCompleted = delegate { };

        public WorkerController(WorkerModel workerModel, IWorkerView workerView)
        {
            _model = workerModel;
            _view = workerView;
        }

        private void InitTask(Vector3 target)
        {
            _view.Activate();
            _view.InitPlace(_model.StatrtingPlace);

            _model.PlaceOfWork = target;
            _view.GoToPlace(_model.PlaceOfWork);
            _model.State = WorkerStates.NONE;
        }

        public int GoToWorkAndReturn(Vector3 placeOfWork)
        {
            InitTask(placeOfWork);
            _model.State = WorkerStates.GO_TO_WORK;
            return _model.WorkerId;
        }

        public int GoToPlace(Vector3 place)
        {
            InitTask(place);
            _model.State = WorkerStates.GO_TO_PLACE;
            return _model.WorkerId;
        }

        private void ProduceWork()
        {
            _view.ProduceWork();
        }

        private void BringProducts()
        {
            _view.GoToPlace(_model.StatrtingPlace);
        }

        public void OnUpdate(float deltaTime)
        {
            if (WorkerStates.NONE == _model.State)
                return;

            if (WorkerStates.PRODUCE_WORK == _model.State)
            {
                _model.WorkTimeLeft -= deltaTime;
                if (_model.WorkTimeLeft < 0)
                {
                    _model.WorkTimeLeft = 0;
                    _model.State = WorkerStates.GO_TO_HOME;
                    BringProducts();
                }
            }
            else if (_view.IsOnThePlace())
            {
                if (WorkerStates.GO_TO_WORK == _model.State)
                {
                    ProduceWork();
                    _model.State = WorkerStates.PRODUCE_WORK;
                    _model.WorkTimeLeft = _model.TimeOfWork;
                }
                else if (WorkerStates.GO_TO_HOME == _model.State ||
                    WorkerStates.GO_TO_PLACE == _model.State)
                {
                    OnMissionCompleted.Invoke(_model.WorkerId);
                    _model.State = WorkerStates.NONE;
                    _view.Deactivate();
                }
            }
        }
    }
}