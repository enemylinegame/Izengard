using System;
using UnitSystem;

namespace UI
{
    public class BattleUIController :  IOnController, IDisposable
    {
        private readonly BattleSceneUI _battleSceneUI;

        public event Action OnStartBattle;
        public event Action OnPauseBattle;
        public event Action OnResumeBattle;
        public event Action OnResetBattle;

        public event Action<IUnitData> OnSpawnNewUnit;

        public BattleUIController(BattleSceneUI battleSceneUI)
        {
            _battleSceneUI = battleSceneUI;

            _battleSceneUI.StartButton.onClick.AddListener(() => OnStartBattle?.Invoke());
            _battleSceneUI.PauseButton.onClick.AddListener(OnPaused);
            _battleSceneUI.ResumeButton.onClick.AddListener(OnResumed);
            _battleSceneUI.ResetButton.onClick.AddListener(OnReseted);

            _battleSceneUI.UnitSettingsPanel.OnSpawn += SpawnUnits;

            _battleSceneUI.UnitSettingsPanel.InitPanel();
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
            _battleSceneUI.UnitSettingsPanel.ResetPanel();
            OnResetBattle?.Invoke();
        }

        private void SpawnUnits(int quantity)
        {
            var unitCreateData = _battleSceneUI.UnitSettingsPanel.Parametrs.GetData();

            for(int i=0; i < quantity; i++)
            {
                OnSpawnNewUnit?.Invoke(unitCreateData);
            }
        }

        public void BlockStartButton(bool state)
        {
            _battleSceneUI.StartButton.interactable = !state;
        }

        public void SwitchPauseUI(bool state)
        {
            _battleSceneUI.PauseButton.gameObject.SetActive(state);
            _battleSceneUI.ResumeButton.gameObject.SetActive(!state);
        }

        public void Dispose()
        {
            _battleSceneUI.StartButton.onClick.RemoveAllListeners();
            _battleSceneUI.PauseButton.onClick.RemoveAllListeners();
            _battleSceneUI.ResumeButton.onClick.RemoveAllListeners();
            _battleSceneUI.ResetButton.onClick.RemoveAllListeners();
        }
    }
}
