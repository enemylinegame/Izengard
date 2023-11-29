using Code.MVC_System;
using Code.SceneConfigs;
using Configs;
using UnityEngine;

public class BattleSceneMain : MonoBehaviour
{

    [SerializeField] private Canvas _canvas;
    [SerializeField] private ConfigsHolder _configsHolder;
    [SerializeField] private SceneObjectsHolder _sceneObjectsHolder;

    private Controller _controllers;

    private void Start()
    {
        _controllers = new Controller();

        new BattleSceneGameInit(_controllers, _configsHolder, _canvas, _sceneObjectsHolder);

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