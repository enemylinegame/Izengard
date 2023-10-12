using Abstraction;
using UnityEngine;

public class MainTowerView : MonoBehaviour, ITarget
{
    [SerializeField] private int _id = 0;
    [SerializeField] private Vector3 _position;
    
    public int Id => _id;
    public Vector3 Position => _position;
}