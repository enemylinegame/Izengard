namespace StartupMenu
{
    public class SettingsMenuModel
    {
        private int _currentResolutionId;
        private int _currentGraphicsId;
        private int _currentShadowId;

        private bool _isFullScreenOn;
        private bool _isFVSyncOn;
        private bool _isBlurnOn;

        #region Public fields

        public int CurrentResolutionId
        {
            get => _currentResolutionId;
            private set
            {
                _currentResolutionId = value;
            }
        }

        public int CurrentGraphicsId
        {
            get => _currentGraphicsId;
            private set
            {
                _currentGraphicsId = value;
            }
        }

        public int CurrentShadowId
        {
            get => _currentShadowId;
            private set
            {
                _currentShadowId = value;
            }
        }

        public bool IsFullScreenOn
        {
            get => _isFullScreenOn;
            private set
            {
                _isFullScreenOn = value;
            }
        }

        public bool IsFVSyncOn
        {
            get => _isFVSyncOn;
            private set
            {
                _isFVSyncOn = value;
            }
        }

        public bool IsBlurnOn
        {
            get => _isBlurnOn;
            private set
            {
                _isBlurnOn = value;
            }
        }

        #endregion

        public void ChangeResolution(int newResolution) =>
            CurrentResolutionId = newResolution;

        public void ChangeGraphics(int newGraphicsId) =>
            CurrentGraphicsId = newGraphicsId;

        public void ChangeShadow(int newShadowId) =>
            CurrentShadowId = newShadowId;

        public void ChangeFullScreenMode(bool state) =>
           IsFullScreenOn = state;

        public void ChangeVSyncMode(bool state) =>
           IsFVSyncOn = state;
        
        public void ChangeBlurMode(bool state) =>
           IsBlurnOn = state;
    }
}
