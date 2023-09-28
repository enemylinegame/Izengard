using Configs;
using Tools;
using UnityEngine;

public class GameInit
{
    public GameInit(
        Controller controller, 
        ConfigsHolder configs, 
        AudioSource clickAudioSource, 
        Canvas canvas)
    {
        var timeRemainingService = new TimeRemainingController();
            
        controller.Add(timeRemainingService);
    }
}