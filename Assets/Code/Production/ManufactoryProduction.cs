using ResourceSystem;
using System;
using System.Collections.Generic;
using System.Text;

public sealed class ManufactoryProduction : IWorkerWork
{
    private float _produced;
    private float _productionEfficiency;
    private GlobalStock _globalStock;
    private Prescription _prescription;
    private IPlayerNotifier _notifier;

    private IList<ResourceType> _deficitResources;
    private int _dificitHashCode;

    public ManufactoryProduction(GlobalStock stock,
        Prescription prescription,
        float productionEfficiency,
        IPlayerNotifier notifier)
    {
        _globalStock = stock;
        _prescription = prescription;
        _productionEfficiency = productionEfficiency;
        _notifier = notifier;

        _deficitResources = new List<ResourceType>();
        _dificitHashCode = _deficitResources.GetHashCode();
    }

    public void Produce(float deltaTime)
    {
        _produced += deltaTime * _productionEfficiency;

        float portion = UnityEngine.Mathf.Floor(_produced);
        if (portion > 0)
        {
            _produced -= portion;

            FindDeficitResources(_prescription, _deficitResources);

            if (0 == _deficitResources.Count)
            {
                UtilizeResources(_prescription);
                _globalStock.AddResourceToStock(_prescription.TargetResource,
                    _prescription.ResultAmount * (int)portion);
            }
            else
            {
                NotifyPlayer(_deficitResources);
            }
            _dificitHashCode = _deficitResources.GetHashCode();
            _deficitResources.Clear();
        }
    }

    public string CreateNotifyMessage(IList<ResourceType> deficitList)
    {
        return "Not enough resource";
    }

    private void NotifyPlayer(IList<ResourceType> deficitList)
    {
        if (deficitList.GetHashCode() != _dificitHashCode)
            _notifier.Notify(CreateNotifyMessage(deficitList));
        
    }

    private void  FindDeficitResources(Prescription prescription, 
        IList<ResourceType> deficitResources)
    {
        deficitResources.Clear();
        for (int i = 0; i < prescription.Components.Length; ++i)
        {
            var prescriptionComponent = prescription.Components[i];
            if (!_globalStock.CheckResourceInStock(
                prescriptionComponent.ResourceType,
                prescriptionComponent.ResourceAmount))
            {
                if (!_deficitResources.Contains(prescriptionComponent.ResourceType))
                    _deficitResources.Add(prescriptionComponent.ResourceType);
            }
        }
    }

    public void UtilizeResources(Prescription prescription)
    {
        Array.ForEach(prescription.Components, (prescriptionComponent) =>
        {
            _globalStock.GetResourceFromStock(
                prescriptionComponent.ResourceType,
                prescriptionComponent.ResourceAmount);
        });
    }

    public void Dispose()
    {
        _globalStock = null;
        _notifier = null;
    }
}
