using ResourceSystem;

public interface IProduce
{
    public ResourceCost NeeddedResourcesForProduce { get; }    
    public float ProducingTime { get; }
    public float CurrentProduceTime { get; }
    public int ProducedValue { get; }
    public bool autoProduce { get; }

    public void StartProduce(float time);
    public void CheckResurseForProduce();
    public void SetAutoProduceFlag();
    public void GetResurseForProduce();
}
