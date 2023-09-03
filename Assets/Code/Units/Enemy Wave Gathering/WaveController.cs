using CombatSystem;
using System;
using Code.BuildingSystem;
using Code.Game;
using Code.TileSystem;
using Code.UI;
using UnityEngine;
using Wave;
using Wave.Interfaces;


public class WaveController : IOnController, IDisposable, IOnUpdate, IOnFixedUpdate, IDowntimeChecker
{
    private readonly TileGenerator _levelTileGenerator;
    private readonly RightPanelController _rightPanel;
    
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


    public WaveController(TileGenerator levelTileGenerator, UIPanelsInitialization controller, 
    ConfigsHolder configsHolder, BulletsController bulletsController, EnemyDestroyObserver destroyObserver, BuildingFactory buildingFactory)
    {
        _levelTileGenerator = levelTileGenerator;
        _rightPanel = controller.RightPanelController;
        
        _enemyAIController = new EnemyAIController();
        _bulletsController = bulletsController;
        
        _waveGathering = new WaveGatheringController(buildingFactory, _enemyAIController, _bulletsController, configsHolder);
        _sendingEnemys = new SendingEnemies(_enemyAIController, _levelTileGenerator, 
            configsHolder.BattlePhaseConfig.EnemySpawnSettings, destroyObserver);
        _combatPhaseWaiting = new CombatPhaseWaiting(_sendingEnemys);
        _combatPhaseWaiting.PhaseEnded += OnCombatPhaseEnding;

        //var timeCountShower = new TimerCountUI(controller.RightPanel.Timer);
        _peacefulPhaseWaiting = new PeacefulPhaseWaiting(configsHolder.PhasesSettings.PeacefulPhaseDuration, controller.RightPanelController.TimeCountShow, controller.NotificationPanel);
        _peacefulPhaseWaiting.PhaseEnded += OnPeacefulPhaseEnding;
        _preparatoryPhaseWaiting = new PreparatoryPhaseWaiting(configsHolder.PhasesSettings.PreparatoryPhaseDuration, controller.RightPanelController.TimeCountShow, this, controller.NotificationPanel);
        _preparatoryPhaseWaiting.PhaseEnded += OnPreparatoryPhaseEnding;

        _posibleSpawnPointsFinder = new PosibleSpawnPointsFinder(levelTileGenerator.SpawnedTiles);
        _levelTileGenerator.SpawnResources += _posibleSpawnPointsFinder.OnNewTileInstantiated;

        _levelTileGenerator.SpawnResources += FreeSideForWave;

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
        _levelTileGenerator.OnCombatPhaseStart?.Invoke();
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
        _rightPanel.ActivateButtonParents();
    }

    private void StartCombatPhase()
    {
        _currentPhase = _combatPhaseWaiting;

        _waveNumber++;
        var enemys = _waveGathering.GetEnemysList(_waveNumber, IsDowntime);
        _sendingEnemys.SendEnemys(enemys, _posibleSpawnPointsFinder.GetPosibleSpawnPoints());
        _combatPhaseWaiting.StartPhase();
        _rightPanel.DeactivateButtonParents();
    }

    private void FreeSideForWave(VoxelTile voxelTile, TileModel model)
    {
        if (voxelTile.NumZone <= 1) return;

        IsDowntime = false;
        if (_waveNumber == 0) StartCombatPhase();
    }

    public void Dispose()
    {
        _levelTileGenerator.SpawnResources -= FreeSideForWave;
        
        _waveGathering.Dispose();
        _combatPhaseWaiting.PhaseEnded -= OnCombatPhaseEnding;
        _peacefulPhaseWaiting.PhaseEnded -= OnPeacefulPhaseEnding;
        _preparatoryPhaseWaiting.PhaseEnded -= OnPreparatoryPhaseEnding;

        _levelTileGenerator.SpawnResources -= _posibleSpawnPointsFinder.OnNewTileInstantiated;
    }
}