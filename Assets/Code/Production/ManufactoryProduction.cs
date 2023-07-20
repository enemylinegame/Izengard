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
    private IList<ResourceType> _lastDeficitResources;

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
        _lastDeficitResources = new List<ResourceType>();
    }

    public void Produce(float deltaTime)
    {
        _produced += deltaTime * _productionEfficiency;

        if (_produced > _prescription.ResultAmount)
        {
            _produced -= _prescription.ResultAmount;

            FindDeficitResources(_prescription, _deficitResources);

            if (0 == _deficitResources.Count)
            {
                UtilizeResources(_prescription);
                _globalStock.AddResourceToStock(_prescription.TargetResource,
                    _prescription.ResultAmount);
            }
            else
            {
                _produced = 0;
                NotifyPlayer(_deficitResources);
            }
            ExchangeLists(ref _deficitResources, ref _lastDeficitResources);
            _deficitResources.Clear();
        }
    }

    public string CreateNotifyMessage(IList<ResourceType> deficitList)
    {
        if (0 == deficitList.Count)
            return string.Empty;

        StringBuilder message = new StringBuilder();
        message.Append("Not enough resource: ");
        int lastDeficintIndex = deficitList.Count - 1;
        for (int i = 0; i < lastDeficintIndex; ++i)
        {
            message.AppendFormat("{0}, ", deficitList[i]);
        }
        message.AppendFormat("{0}.", deficitList[lastDeficintIndex]);
        return message.ToString();
    }

    private void ExchangeLists(ref IList<ResourceType> list1, 
        ref IList<ResourceType> list2)
    {
        IList<ResourceType> buffer = list1;
        list1 = list2;
        list2 = buffer;
    }
    private bool Equals(IList<ResourceType> list1,
        IList<ResourceType> list2)
    {
        if (list1.Count != list2.Count)
            return false;

        for (int i = 0; i < list1.Count; ++i)
            if (list1[i] != list2[i])
                return false;

        return true;
    }

    private void NotifyPlayer(IList<ResourceType> deficitList)
    {
        if (!Equals(deficitList, _lastDeficitResources))
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
                if (!deficitResources.Contains(prescriptionComponent.ResourceType))
                    deficitResources.Add(prescriptionComponent.ResourceType);
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
