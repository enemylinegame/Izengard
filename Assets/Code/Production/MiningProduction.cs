using ResourceSystem;


public sealed class MiningProduction : IWorkerTask
{
    private int _portionSize;
    private GlobalStock _globalStock;
    private ResourceType _resourceType;

    public MiningProduction(GlobalStock stock, ResourceType resourceType,
        int portionSize)
    {
        _globalStock = stock;
        _portionSize = portionSize;
        _resourceType = resourceType;
    }

    public void Produce()
    {
        _globalStock.AddResourceToStock(_resourceType, _portionSize);
    }

    public void Dispose()
    {
        _globalStock = null;
    }
}
