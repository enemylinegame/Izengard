using ResourceSystem;


public class MiningProduction : IWorkerTask
{
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

    private int _portionSize;
    private GlobalStock _globalStock;
    private ResourceType _resourceType;
}
