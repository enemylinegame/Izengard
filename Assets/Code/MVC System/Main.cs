using Configs;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField] private ConfigsHolder _configsHolder;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private AudioSource _clickAudioSource;
    
    private Controller _controllers;

    private void Start()
    {
        _controllers = new Controller();

        new GameInit(_controllers, _configsHolder, _clickAudioSource, _canvas);

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