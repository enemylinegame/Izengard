using ResourceSystem;

public class ManufactoryProduction : IWorkerWork
{
    public ManufactoryProduction(GlobalStock stock, ResourceType resourceType, 
        float productionEfficiency)
    {
        _productionEfficiency = productionEfficiency;
        _globalStock = stock;
    }

    public void Produce(float deltaTime)
    {
        _produced += deltaTime * _productionEfficiency;

        int portion = (int)_produced;
        if (portion > 0)
            _produced -= portion;

        _globalStock.AddResourceToStock(_resourceType, portion);
    }


    private float _produced;

    private float _productionEfficiency;
    private GlobalStock _globalStock;
    private ResourceType _resourceType;
}
