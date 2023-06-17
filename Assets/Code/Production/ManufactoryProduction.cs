using ResourceSystem;

public class ManufactoryProduction : IWorkerWork
{
    public ManufactoryProduction(GlobalStock stock, ResourceType resourceType, 
        float productionEfficiency)
    {
        _globalStock = stock;
        _resourceType = resourceType;
        _productionEfficiency = productionEfficiency;
    }

    public void Produce(float deltaTime)
    {
        _produced += deltaTime * _productionEfficiency;

        float portion = UnityEngine.Mathf.Floor(_produced);
        if (portion > 0)
        {
            _produced -= portion;
            _globalStock.AddResourceToStock(_resourceType, (int)portion);
        }
    }


    private float _produced;

    private float _productionEfficiency;
    private GlobalStock _globalStock;
    private ResourceType _resourceType;
}
