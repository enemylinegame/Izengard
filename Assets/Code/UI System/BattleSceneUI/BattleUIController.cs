using System;
using UnitSystem;

namespace UI
{
    public class BattleUIController :  IOnController, IDisposable
    {
        private readonly BattlePanelUI _view;

        public BattlePanelUI View => _view;

        public event Action OnStartBattle;
        public event Action OnPauseBattle;
        public event Action OnResumeBattle;
        public event Action OnResetBattle;

        public event Action<IUnitData> OnSpawnNewUnit;

        public BattleUIController(UIViewFactory<BattlePanelUI> uIViewFactory)
        {
            _view = uIViewFactory.GetView(uIViewFactory.UIElementsConfig.BattlePanel);

            _view.StartButton.onClick.AddListener(() => OnStartBattle?.Invoke());
            _view.PauseButton.onClick.AddListener(OnPaused);
            _view.ResumeButton.onClick.AddListener(OnResumed);
            _view.ResetButton.onClick.AddListener(OnReseted);

            _view.UnitSettingsPanel.OnSpawn += SpawnUnits;

            _view.UnitSettingsPanel.InitPanel();
        }

        private void OnPaused()
        {
            OnPauseBattle?.Invoke();
        }

        private void OnResumed()
        {
            OnResumeBattle?.Invoke();
        }

        private void OnReseted()
        {
            _view.UnitSettingsPanel.ResetPanel();
            OnResetBattle?.Invoke();
        }

        private void SpawnUnits(int quantity)
        {
            var unitCreateData = _view.UnitSettingsPanel.Parametrs.GetData();

            for(int i=0; i < quantity; i++)
            {
                OnSpawnNewUnit?.Invoke(unitCreateData);
            }
        }

        public void BlockStartButton(bool state)
        {
            _view.StartButton.interactable = !state;
        }

        public void SwitchPauseUI(bool state)
        {
            _view.PauseButton.gameObject.SetActive(state);
            _view.ResumeButton.gameObject.SetActive(!state);
        }

        public void Dispose()
        {
            _view.StartButton.onClick.RemoveAllListeners();
            _view.PauseButton.onClick.RemoveAllListeners();
            _view.ResumeButton.onClick.RemoveAllListeners();
            _view.ResetButton.onClick.RemoveAllListeners();
        }
    }
}
