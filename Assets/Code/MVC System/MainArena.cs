using System.Collections.Generic;
using Code.SceneConfigs;
using Configs;
using UnityEngine;

namespace Code.MVC_System
{
    public class MainArena : MonoBehaviour
    {
        [SerializeField] private ConfigsHolder _configsHolder;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private AudioSource _clickAudioSource;
        [SerializeField] private SceneObjectsHolder _sceneObjectsHolder;

        private Controller _controllers;

        private void Start()
        {
            _controllers = new Controller();

            new GameInitArena(_controllers, _configsHolder, _clickAudioSource, _canvas, _sceneObjectsHolder);

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
}