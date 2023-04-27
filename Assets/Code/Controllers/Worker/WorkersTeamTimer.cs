using System;


namespace Assets.Code.Controllers.Worker
{
    public class WorkersTeamTimer : IOnUpdate
    {
        public Action<int> OnTimeOut = delegate{};

        public void SetTimer(float timeInterval, int numOfWorkers)
        {
            _timeLeft = timeInterval;
            _countLeft = numOfWorkers;
        }

        public void OnUpdate(float deltaTime)
        {
            if (_countLeft <= 0)
                return;

            _timeLeft -= deltaTime;
            if (_timeLeft <= 0)
            {
                _countLeft--;
                OnTimeOut.Invoke(_countLeft);
                _timeLeft += _timeInterval;
            }
        }

        private float _timeLeft;
        private int _countLeft;
        private float _timeInterval;
    }
}