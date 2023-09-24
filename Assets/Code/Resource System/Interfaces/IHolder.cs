using UnityEngine;


public interface IHolder<T> where T:ScriptableObject
{
    public T ObjectInHolder { get; }
    public float CurrentValue { get; }
    public float MaxValue { get; }

    public void ChangeObjectInHolder(T obj);
    //public IHolder<T> AddInHolder(IHolder<T> holder);
    //public IHolder<T> GetFromHolder(IHolder<T> holder);
    public void SetCurrentValueHolder(float value);
    public void SetMaxValueHolder(float value);
    


}
