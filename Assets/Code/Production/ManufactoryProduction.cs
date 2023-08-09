using ResourceSystem;
using System;
using System.Collections.Generic;
using System.Text;

public sealed class ManufactoryProduction : IWorkerWork
{
    private float _produced;
    private float _productionEfficiency;
    private GlobalStock _globalStock;
    private Recipe _prescription;
    private IPlayerNotifier _notifier;
    private bool _isProductionSuccess;

    private struct DeficitDescription
    {
        public ResourceType ResourceType;
        public int ResourceAccount;
    };

    private IList<DeficitDescription> _deficitResources;
    private IList<DeficitDescription> _lastDeficitResources;

    public ManufactoryProduction(GlobalStock stock,
        Recipe prescription,
        float productionEfficiency,
        IPlayerNotifier notifier)
    {
        _globalStock = stock;
        _prescription = prescription;
        _productionEfficiency = productionEfficiency;
        _notifier = notifier;

        _deficitResources = new List<DeficitDescription>();
        _lastDeficitResources = new List<DeficitDescription>();

        _isProductionSuccess = true;
    }

    public bool Produce(float deltaTime)
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
                _isProductionSuccess = true;
            }
            else
            {
                _produced = 0.0f;
                NotifyPlayer(_deficitResources);
                _isProductionSuccess = false;
            }
            ExchangeLists(ref _deficitResources, ref _lastDeficitResources);
            _deficitResources.Clear();
        }
        return _isProductionSuccess;
    }

    public bool IsProductionSuccess
    {
        get => _isProductionSuccess;
    }

    private void AppendDeficitFormat(StringBuilder message, 
        DeficitDescription deficitDescription)
    
    {
        message.AppendFormat("{0} missing {1}",
                deficitDescription.ResourceType, 
                deficitDescription.ResourceAccount);
    }

    private string CreateNotificationMessage(
        IList<DeficitDescription> deficitList)
    {
        if (0 == deficitList.Count)
            return string.Empty;

        StringBuilder message = new StringBuilder();
        message.Append("Not enough resources: ");
        int lastDeficintIndex = deficitList.Count - 1;
        for (int i = 0; i < lastDeficintIndex; ++i)
        {
            AppendDeficitFormat(message, deficitList[i]);
            message.Append(", ");
        }
        AppendDeficitFormat(message, deficitList[lastDeficintIndex]);
        message.Append(".");

        return message.ToString();
    }

    private void ExchangeLists<T>(ref IList<T> list1, 
        ref IList<T> list2)
    {
        IList<T> buffer = list1;
        list1 = list2;
        list2 = buffer;
    }
    private bool Equals(IList<DeficitDescription> list1,
        IList<DeficitDescription> list2)
    {
        if (list1.Count != list2.Count)
            return false;

        for (int i = 0; i < list1.Count; ++i)
            if (list1[i].ResourceType != list2[i].ResourceType ||
                list1[i].ResourceAccount != list2[i].ResourceAccount)
                return false;

        return true;
    }

    private void NotifyPlayer(IList<DeficitDescription> deficitList)
    {
        if (!Equals(deficitList, _lastDeficitResources))
            _notifier.Notify(CreateNotificationMessage(deficitList));
    }

    private void  FindDeficitResources(Recipe prescription, 
        IList<DeficitDescription> deficit)
    {
        deficit.Clear();
        for (int i = 0; i < prescription.Components.Length; ++i)
        {
            var prescriptionComponent = prescription.Components[i];

            int availableResourceAccount = 
                _globalStock.GetAvailableResourceAccount(
                    prescriptionComponent.ResourceType);

            if (availableResourceAccount < 
                prescriptionComponent.ResourceAmount)
            {
                deficit.Add(new DeficitDescription{ 
                    ResourceType = prescriptionComponent.ResourceType,
                    
                    ResourceAccount = prescriptionComponent.ResourceAmount - 
                        availableResourceAccount
                });
            }
        }
    }

    public void UtilizeResources(Recipe prescription)
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
