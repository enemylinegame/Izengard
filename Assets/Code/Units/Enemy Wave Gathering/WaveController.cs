using CombatSystem;
using System;
using Code;
using Code.UI;
using UnityEngine;
using Wave;
using Wave.Interfaces;


public class WaveController : IOnController, IDisposable, IOnUpdate, IOnFixedUpdate, IDowntimeChecker
{
    private readonly GeneratorLevelController _levelGenerator;
    private readonly Transform _btnParents;
    
    private readonly IWaveGathering _waveGathering;
    private readonly ISendingEnemys _sendingEnemys;
    private readonly PhaseWaitingBase _combatPhaseWaiting;
    private readonly PhaseWaitingBase _peacefulPhaseWaiting;
    private readonly PhaseWaitingBase _preparatoryPhaseWaiting;
    private int _waveNumber;
    
    private readonly IPosibleSpawnPointsFinder _posibleSpawnPointsFinder;
    private IOnUpdate _currentPhase;

    private readonly IEnemyAIController _enemyAIController;
    private readonly IBulletsController _bulletsController;

    public bool IsDowntime { get; private set; }


    public WaveController(GeneratorLevelController levelGenerator, UIController controller, Transform btnParents, GameConfig gameConfig)
    {
        _levelGenerator = levelGenerator;
        _btnParents = btnParents;
        
        _enemyAIController = new EnemyAIController();
        _bulletsController = new BulletsController();
        
        _waveGathering = new WaveGatheringController(_levelGenerator, _enemyAIController, _bulletsController, gameConfig);
        _sendingEnemys = new SendingEnemies(_enemyAIController, _levelGenerator, gameConfig.BattlePhaseConfig.EnemySpawnSettings);
        _combatPhaseWaiting = new CombatPhaseWaiting(_sendingEnemys);
        _combatPhaseWaiting.PhaseEnded += OnCombatPhaseEnding;

        var timeCountShower = new TimerCountUI(controller.RightUI.Timer);
        _peacefulPhaseWaiting = new PeacefulPhaseWaiting(gameConfig.PhasesSettings.PeacefulPhaseDuration, timeCountShower.TimeCountShow, controller.CenterUI.BaseNotificationUI);
        _peacefulPhaseWaiting.PhaseEnded += OnPeacefulPhaseEnding;
        _preparatoryPhaseWaiting = new PreparatoryPhaseWaiting(gameConfig.PhasesSettings.PreparatoryPhaseDuration, timeCountShower.TimeCountShow, this, controller.CenterUI.BaseNotificationUI);
        _preparatoryPhaseWaiting.PhaseEnded += OnPreparatoryPhaseEnding;

        _posibleSpawnPointsFinder = new PosibleSpawnPointsFinder(levelGenerator.SpawnedTiles);
        _levelGenerator.SpawnResources += _posibleSpawnPointsFinder.OnNewTileInstantiated;

        _levelGenerator.SpawnResources += FreeSideForWave;

    }

    public void OnUpdate(float deltaTime)
    {
        _currentPhase?.OnUpdate(deltaTime);
        _sendingEnemys?.OnUpdate(deltaTime);
        _enemyAIController?.OnUpdate(deltaTime);
    }

    public void OnFixedUpdate(float fixedDeltaTime)
    {
        _sendingEnemys?.OnFixedUpdate(fixedDeltaTime);
        _bulletsController?.OnFixedUpdate(fixedDeltaTime);
    }

    private void OnCombatPhaseEnding()
    {
        // Debug.Log("боевая фаза закончилась");
        StartPeacefullPhase();
    }

    private void OnPeacefulPhaseEnding()
    {
        // Debug.Log("мирная фаза закончилась");
        StartPreparatoryPhase();
    }

    private void OnPreparatoryPhaseEnding()
    {
        // Debug.Log("подготовительная фаза закончилась");
        _levelGenerator.OnCombatPhaseStart?.Invoke();
        StartCombatPhase();
    }

    private void StartPeacefullPhase()
    {
        _currentPhase = _peacefulPhaseWaiting;

        _peacefulPhaseWaiting.StartPhase();
    }

    private void StartPreparatoryPhase()
    {
        _currentPhase = _preparatoryPhaseWaiting;

        _preparatoryPhaseWaiting.StartPhase();
        IsDowntime = true;
        _btnParents.gameObject.SetActive(true);
    }

    private void StartCombatPhase()
    {
        _currentPhase = _combatPhaseWaiting;

        _waveNumber++;
        var enemys = _waveGathering.GetEnemysList(_waveNumber, IsDowntime);
        _sendingEnemys.SendEnemys(enemys, _posibleSpawnPointsFinder.GetPosibleSpawnPoints());
        _combatPhaseWaiting.StartPhase();
        _btnParents.gameObject.SetActive(false);
    }

    private void FreeSideForWave(VoxelTile voxelTile)
    {
        if (voxelTile.NumZone <= 1) return;

        IsDowntime = false;
        if (_waveNumber == 0) StartCombatPhase();
    }

    public void Dispose()
    {
        _levelGenerator.SpawnResources -= FreeSideForWave;
        
        _waveGathering.Dispose();
        _combatPhaseWaiting.PhaseEnded -= OnCombatPhaseEnding;
        _peacefulPhaseWaiting.PhaseEnded -= OnPeacefulPhaseEnding;
        _preparatoryPhaseWaiting.PhaseEnded -= OnPreparatoryPhaseEnding;

        _levelGenerator.SpawnResources -= _posibleSpawnPointsFinder.OnNewTileInstantiated;
    }
}