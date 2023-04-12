public class UnitHandler: IUnitHandler
{
    private IUnitHandler _nextHandler;

    public virtual IUnitHandler Handle()
    {
        if (_nextHandler != null)
        {
            _nextHandler.Handle();
        }
        return _nextHandler;
    }

    public IUnitHandler SetNext(IUnitHandler nextHandler)
    {
        _nextHandler = nextHandler;
        return nextHandler;

    }

    public IUnitHandler GetCurrent()
    {
        return this;
    }
}
