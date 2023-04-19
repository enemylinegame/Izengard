using System;
using UnityEngine;
using Wave;


namespace Code.MovementOfWorkers.Animations
{
    public class WorkerProductionAnimation : IWorkerAnimation
    {
        private const float DASH_DISTANCE = 0.1f;
        private const float DASH_SPEED = 1;

        public event Action ActionMoment;
        public event Action AnimationComplete;

        private readonly Enemy _unit;
        private Vector3 _startPosition;
        private float _currentDashPosition;
        private bool _isStartMoving;


        public WorkerProductionAnimation(Enemy unit)
        {
            _unit = unit;
        }

        public void OnFixedUpdate(float deltaTime)
        {
            if (_isStartMoving)
            {
                _currentDashPosition += DASH_SPEED * deltaTime;
                _unit.Prefab.transform.Translate(Vector3.forward * DASH_SPEED * deltaTime);
                if (_currentDashPosition > DASH_DISTANCE)
                {
                    _isStartMoving = false;
                    ActionMoment?.Invoke();
                }
            }
            else
            {
                _currentDashPosition -= DASH_SPEED * deltaTime;
                if (_currentDashPosition < 0)
                {
                    _unit.Prefab.transform.localPosition = _startPosition;
                    AnimationComplete?.Invoke();
                }
                _unit.Prefab.transform.Translate(Vector3.back * DASH_SPEED * deltaTime);
            }
        }

        public void PlayAnimation()
        {
            _startPosition = _unit.Prefab.transform.localPosition;
            _currentDashPosition = 0;
            _isStartMoving = true;
        }
    }
}