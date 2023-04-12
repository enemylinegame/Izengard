using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INameHolder 
{
    public string Name { get; }    
    public void SetName(string name);
}
