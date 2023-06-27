namespace ResourceMarket
{
    public interface ICustomer
    {
        int Gold { get; }
        void AddGold(int amount);
        void RemoveGold(int amount);
    }
}
