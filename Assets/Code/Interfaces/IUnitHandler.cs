public interface IUnitHandler
{
        IUnitHandler Handle();
        IUnitHandler SetNext(IUnitHandler nextHandler);
}
