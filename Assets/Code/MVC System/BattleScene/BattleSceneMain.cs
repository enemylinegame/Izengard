using Code.MVC_System;
using Code.SceneConfigs;
using Configs;
using NewBuildingSystem;
using UnityEngine;

public class BattleSceneMain : MonoBehaviour
{

    [SerializeField] private Canvas _canvas;
    [SerializeField] private ConfigsHolder _configsHolder;
    [SerializeField] private SceneObjectsHolder _sceneObjectsHolder;
    [SerializeField] private GameObject _spawnerPrefab;
    [SerializeField] private GameObject _plane;
    [SerializeField] private Grid _grid;
    [SerializeField] private Map _map;

    private Controller _controllers;

    private void Start()
    {
        _controllers = new Controller();

        new BattleSceneGameInit(
            _controllers, 
            _configsHolder, 
            _canvas, 
            _sceneObjectsHolder, 
            _spawnerPrefab, 
            _plane, 
            _grid,
            _map);

        _controllers.OnStart();
    }

    private void Update()
    {
        _controllers.OnUpdate(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        _controllers.OnFixedUpdate(Time.fixedDeltaTime);
    }

    private void LateUpdate()
    {
        _controllers.OnLateUpdate(Time.deltaTime);
    }
}