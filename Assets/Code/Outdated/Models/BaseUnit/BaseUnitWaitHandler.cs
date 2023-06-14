using Controllers.BaseUnit;


public sealed class BaseUnitWaitHandler : UnitHandler
{
   // private TimeController _timeController;
    private readonly BaseUnitController _baseUnitController;
    private float _time;

    public BaseUnitWaitHandler(float time, BaseUnitController baseUnitController)
    {
        _time = time;
        _baseUnitController = baseUnitController;
    }

    private void TimeIsUp()
    {
        base.Handle();
    }

    public override IUnitHandler Handle()
    {
        _baseUnitController.CurrentUnitHandler = GetCurrent();
        TimeRemainingExtensions.AddTimeRemaining(new TimeRemaining(TimeIsUp,_time,false));
        return this;
    }

}
