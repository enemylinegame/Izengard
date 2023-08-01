using System;
using UnityEngine;


namespace Controllers.Worker
{
    public sealed class WorkerController: IOnUpdate, IDisposable
    {
        public WorkerModel Model { get; private set; }
        public IWorkerView View { get; private set; }

        public Action<WorkerController> OnMissionCompleted = delegate { };

        public WorkerController(WorkerModel workerModel, IWorkerView workerView)
        {
            Model = workerModel;
            View = workerView;
        }
        private void InitTask(Vector3 fromPlace, Vector3 target)
        {
            fromPlace.y = 0.0f;
            target.y = 0.0f;

            Model.StatrtingPlace = fromPlace;
            Model.TargetPlace = target;

            if (WorkerStates.NONE == Model.State)
            {
                View.Activate();
                View.InitPlace(fromPlace);
            }
            else
                View.ContinueWalkFromCurrentPlace();

            View.GoToPlace(target);
            Model.State = WorkerStates.NONE;
        }

        public int GoToWorkAndReturn(Vector3 fromPlace, Vector3 placeOfWork)
        {
            InitTask(fromPlace, placeOfWork);
            Model.State = WorkerStates.GO_TO_WORK;
            return Model.WorkerId;
        }

        public int RepeatGoToWorkAndReturn()
        {
            return GoToWorkAndReturn(
                Model.StatrtingPlace, Model.TargetPlace);
        }

        public int GoToPlace(Vector3 fromPlace, Vector3 toPlace)
        {
            InitTask(fromPlace, toPlace);
            Model.State = WorkerStates.GO_TO_PLACE;
            return Model.WorkerId;
        }

        public void CancelWork()
        {
            Model.State = WorkerStates.GO_TO_HOME;
            View.Activate();
            View.GoToPlace(Model.StatrtingPlace);
        }

        public int WorkerId => Model.WorkerId;
        
        private void ProduceWork()
        {
            View.ProduceWork();
            Model.State = WorkerStates.PRODUCE_WORK;
            Model.WorkTimeLeft = Model.TimeOfWork;
        }

        private void BringProducts()
        {
            Model.WorkTimeLeft = 0;
            Model.State = WorkerStates.GO_TO_HOME;
            View.DragToPlace(Model.StatrtingPlace);
        }

        private void MissionIsCompleted()
        {
            OnMissionCompleted.Invoke(this);
            Model.State = WorkerStates.NONE;
            View.Deactivate();
        }

        public void OnUpdate(float deltaTime)
        {
            if (WorkerStates.NONE == Model.State)
                return;

            if (WorkerStates.PRODUCE_WORK == Model.State)
            {
                Model.WorkTimeLeft -= deltaTime;
                if (Model.WorkTimeLeft < 0)
                {
                    BringProducts();
                }
            }
            else if (View.IsOnThePlace())
            {
                if (WorkerStates.GO_TO_WORK == Model.State)
                {
                    ProduceWork();
                }
                else if (WorkerStates.GO_TO_HOME == Model.State ||
                    WorkerStates.GO_TO_PLACE == Model.State)
                {
                    MissionIsCompleted();
                }
            }
        }

        public void Pause()
        {
            View.Pause();
        }

        public void Resume()
        {
            switch (Model.State)
            {
                case WorkerStates.GO_TO_HOME:
                    View.ResumeDrag();
                    break;
                case WorkerStates.GO_TO_PLACE:
                    View.ResumeWalk();
                    break;
                case WorkerStates.GO_TO_WORK:
                    View.ResumeWalk();
                    break;
                case WorkerStates.PRODUCE_WORK:
                    View.ResumeWork();
                    break;
            }
        }
        public void Dispose()
        {
            Model = null;
            View = null;
        }
    }
}