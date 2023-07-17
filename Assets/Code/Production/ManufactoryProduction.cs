using ResourceSystem;
using System;

public class ManufactoryProduction : IWorkerWork
{
    public ManufactoryProduction(GlobalStock stock, 
        Prescription prescription, 
        float productionEfficiency)
    {
        _globalStock = stock;
        _prescription = prescription;
        _productionEfficiency = productionEfficiency;
    }

    public void Produce(float deltaTime)
    {
        _produced += deltaTime * _productionEfficiency;

        float portion = UnityEngine.Mathf.Floor(_produced);
        if (portion > 0)
        {
            _produced -= portion;

            if (IsPrescriptCanBeDone(_prescription))
            {
                UtilizeResources();
                _globalStock.AddResourceToStock(_prescription.TargetResource,
                    (int)portion);
            }
        }
    }

    public bool IsPrescriptCanBeDone(Prescription prescription)
    {
        throw new NotImplementedException();
        return true;
    }

    public void UtilizeResources()
    {
        throw new NotImplementedException();
    }

    private float _produced;

    private float _productionEfficiency;
    private GlobalStock _globalStock;
    private Prescription _prescription;
}
