namespace Tools
{
    public class HealthBarController : IOnController, IOnUpdate
    {
        private readonly HealthBarView _view;

        private float _maxHealth;
        private float _currentHealtRatio;
        private bool _isEnable;

        public HealthBarController(HealthBarView view,float maxHealth)
        {
            if (view == null)
            {
                _isEnable = false;
                return;
            }

            _view = view;
            _maxHealth = maxHealth;
            _currentHealtRatio = 1;

            _view.Init();
        }

        public void Enable()
        {
            _isEnable = true;

            _view.Show();
        }

        public void Disable()
        {
            _isEnable = false;

            _view.Hide();
        }

        public void ChangeHealthRatio(int healthValue)
        {
            _currentHealtRatio = healthValue / _maxHealth;
        }

        public void OnUpdate(float deltaTime)
        {
            if (_isEnable == false)
                return;

            _view.UpdateRotation();

            _view.UpdateHealthReduce(_currentHealtRatio);
        }
    }
}
