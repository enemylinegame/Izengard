using System;
using UnityEngine;


namespace Controllers.Worker
{
    public sealed class WorkerController: IOnUpdate
    {
        private WorkerModel _model;
        private IWorkerView _view;

        public Action<WorkerController> OnMissionCompleted = delegate { };

        public WorkerController(WorkerModel workerModel, IWorkerView workerView)
        {
            _model = workerModel;
            _view = workerView;
        }

        private void InitTask(Vector3 fromPlace, Vector3 target)
        {
            _view.Activate();
            _model.StatrtingPlace = fromPlace;

            _view.InitPlace(fromPlace);
            _view.GoToPlace(target);
            _model.State = WorkerStates.NONE;
        }

        public int GoToWorkAndReturn(Vector3 fromPlace, Vector3 placeOfWork)
        {
            InitTask(fromPlace, placeOfWork);
            _model.State = WorkerStates.GO_TO_WORK;
            return _model.WorkerId;
        }

        public int GoToPlace(Vector3 fromPlace, Vector3 toPlace)
        {
            InitTask(fromPlace, toPlace);
            _model.State = WorkerStates.GO_TO_PLACE;
            return _model.WorkerId;
        }

        public void CancelWork()
        {
            _model.State = WorkerStates.GO_TO_HOME;
            _view.GoToPlace(_model.StatrtingPlace);
        }

        public int WorkerId => _model.WorkerId;
        
        private void ProduceWork()
        {
            _view.ProduceWork();
        }

        private void BringProducts()
        {
            _view.DragToPlace(_model.StatrtingPlace);
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
                    OnMissionCompleted.Invoke(this);
                    _model.State = WorkerStates.NONE;
                    _view.Deactivate();
                }
            }
        }

        public void Pause()
        {
            _view.Pause();
        }

        public void Resume()
        {
            switch (_model.State)
            {
                case WorkerStates.GO_TO_HOME:
                    _view.ResumeDrag();
                    break;
                case WorkerStates.GO_TO_PLACE:
                    _view.ResumeWalk();
                    break;
                case WorkerStates.GO_TO_WORK:
                    _view.ResumeWalk();
                    break;
                case WorkerStates.PRODUCE_WORK:
                    _view.ResumeWork();
                    break;
            }
        }
    }
}