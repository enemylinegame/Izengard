using System;
using System.Collections.Generic;
using UnityEngine;


public class Damageable : MonoBehaviour, IHealthHolder
{
    [SerializeField] private Sprite _icon;
    [SerializeField] private int _maxCountAttackers = 5;

    public Sprite Icon => _icon;
    public float CurrentHealth { get; private set; }
    public float MaxHealth {get; private set;}
  
    public event Action DeathAction;
    public event Action<List<Damageable>> MeAttackedChenged;
    public bool IsDamagableDead { get; private set; }

    private List<Damageable> _listAttackedUnits = new List<Damageable>();

    public void Init(int maxHealth)
    {
        CurrentHealth = maxHealth;
        MaxHealth = maxHealth;
        IsDamagableDead = false;
    }

    public void Init(int maxHealth, int maxAttackers)
    {
        CurrentHealth = maxHealth;
        MaxHealth = maxHealth;
        IsDamagableDead = false;
        _maxCountAttackers = maxAttackers;
    }

    // private void OnCollisionEnter(Collision other) 
    // {
    //     if (other.collider.CompareTag("Bullet"))
    //     {
    //         MakeDamage(50);
    //     }
    // }

    public bool Attacked(Damageable damageable)
    {
        if (_listAttackedUnits.Count >= _maxCountAttackers) return false;
        else
        {
            if (!_listAttackedUnits.Contains(damageable))
            {
                _listAttackedUnits.Add(damageable);
                damageable.DeathAction += MeAttackedDead;
                MeAttackedChenged?.Invoke(_listAttackedUnits);
                return true;
            }
            return false;
        }

    }

    private void MeAttackedDead()
    {
        for (int i =0; i < _listAttackedUnits.Count; i++)
        {
            if (_listAttackedUnits[i].IsDamagableDead)
            {
                _listAttackedUnits[i].DeathAction-= MeAttackedDead;
                _listAttackedUnits.Remove(_listAttackedUnits[i]);
            }
        }
        MeAttackedChenged?.Invoke(_listAttackedUnits);
    }

    public void MakeDamage(int damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            IsDamagableDead = true;
            DeathAction?.Invoke();
            _listAttackedUnits.Clear();
            MeAttackedChenged?.Invoke(_listAttackedUnits);
        }
    }
}