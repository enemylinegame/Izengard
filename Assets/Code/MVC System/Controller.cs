using System.Collections.Generic;

public class Controller
{
    public const string startMethod = "OnStart";
    public const string updateMethod = "OnUpdate";
    public const string fixedUpdateMethod = "OnFixedUpdate";
    public const string lateUpdate = "OnLateUpdate";
    
    private List<IOnStart> _onStarts = new List<IOnStart>();
    private List<IOnUpdate> _onUpdates = new List<IOnUpdate>();
    private List<IOnFixedUpdate> _onFixedUpdates = new List<IOnFixedUpdate>();
    private List<IOnLateUpdate> _onLateUpdates = new List<IOnLateUpdate>();
    private List<IOnDisable> _onDisables = new List<IOnDisable>();

    public Controller Add(IOnController controller)
    {
        if (controller is IOnStart onStart)
        {
            _onStarts.Add(onStart);
        }
            
        if (controller is IOnUpdate onUpdate)
        {
            _onUpdates.Add(onUpdate);
        }
            
        if (controller is IOnFixedUpdate onFixedUpdate)
        {
            _onFixedUpdates.Add(onFixedUpdate);
        }

        if (controller is IOnLateUpdate onLateUpdate)
        {
            _onLateUpdates.Add(onLateUpdate);
        }

        if (controller is IOnDisable onDisable)
        {
            _onDisables.Add(onDisable);
        }
            
        return this;
    }
    
    public void OnStart()
    {
        foreach (var ell in _onStarts)
        {
            if (ell.HasMethod(startMethod))
            {
                ell.OnStart();
            }
        }
    }

    public void OnUpdate(float deltaTime)
    {
        foreach (var ell in _onUpdates)
        {
            if (ell.HasMethod(updateMethod))
            {
                ell.OnUpdate(deltaTime);
            }
        }
    }

    public void OnFixedUpdate(float fixedDeltaTime)
    {
        foreach (var ell in _onFixedUpdates)
        {
            if (ell.HasMethod(fixedUpdateMethod))
            {
                ell.OnFixedUpdate(fixedDeltaTime);
            }
        }
    }

    public void OnLateUpdate(float deltaTime)
    {
        foreach (var ell in _onLateUpdates)
        {
            if (ell.HasMethod(lateUpdate))
            {
                ell.OnLateUpdate(deltaTime);
            }
        }
    }

    public void OnDisable()
    {
        foreach (var ell in _onDisables)
        {
            if (ell.HasMethod(startMethod))
            {
                ell.OnDisableItself();
            }
        }
    }
}