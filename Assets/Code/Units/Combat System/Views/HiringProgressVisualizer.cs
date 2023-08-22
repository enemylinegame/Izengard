using Code.Units.HireDefendersSystem;
using UnityEngine.UI;

namespace CombatSystem.Views
{
    public class HiringProgressVisualizer
    {

        private const float UPDATE_INTERVAL = 0.1f;

        private Image _image;
        private IGetProgress _progress;
        private TimeRemaining _timer;
        private bool _isTiming;

        public HiringProgressVisualizer(Image image)
        {
            _image = image;
            _image.enabled = false;
            _timer = new TimeRemaining(OnUpdate, UPDATE_INTERVAL, true);
        }

        public void On(IGetProgress progress)
        {
            if (!_isTiming)
            {
                if (progress == null) return;

                _progress = progress;
                _image.enabled = true;
                TimersService.AddTimer(_timer);
                OnUpdate();
                _isTiming = true;
            }
        }

        public void Off()
        {
            if (_isTiming)
            {
                TimersService.RemoveTimer(_timer);
                _image.enabled = false;
                _isTiming = false;
            }
        }

        private void OnUpdate()
        {
            float current = _progress.GetCurrentProgress();
            float max = _progress.GetMaxProgress();

            if (max > 0.0f)
            {
                _image.fillAmount = 1.0f - current / max;
            }
        }
        
    }
}