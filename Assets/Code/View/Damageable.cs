using System;
using System.Threading.Tasks;
using Code.TowerShot;
using UnityEngine;


public class Damageable : MonoBehaviour, IHealthHolder
{
    [SerializeField] private Sprite _icon;
    public Sprite Icon => _icon;
    public float Health { get; private set; }
    public float MaxHealth {get; private set;}
    public event Action DeathAction;
    public bool IsDamagableDead { get; private set; }


    public void Init(int maxHealth)
    {
        Health = maxHealth;
        MaxHealth = maxHealth;
        IsDamagableDead = false;
    }

    // private void OnCollisionEnter(Collision other) 
    // {
    //     if (other.collider.CompareTag("Bullet"))
    //     {
    //         MakeDamage(50);
    //     }
    // }


    public void MakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            IsDamagableDead = true;
            DeathAction?.Invoke();
            
        }
    }
}