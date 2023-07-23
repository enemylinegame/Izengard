using System;
using System.Collections.Generic;
using CombatSystem.Interfaces;
using UnityEngine;


public class Damageable : MonoBehaviour, IHealthHolder, IDamageable
{
    [SerializeField] private Sprite _icon;
    [SerializeField] private int _maxCountAttackers = 5;
    [SerializeField] private int _threatLevel = 0;

    
    public Sprite Icon => _icon;
    public float CurrentHealth { get; private set; }
    public float MaxHealth {get; private set;}

    public Vector3 Position => transform.position;

    public int ThreatLevel => _threatLevel;

    public int NumberOfAttackers => _listAttackedUnits.Count;
    
    public event Action<float, float> OnHealthChanged; 
    public event Action OnDeath;
    public event Action<IDamageable> OnDamaged; 

    public bool IsDead { get; private set; }

    private List<Damageable> _listAttackedUnits = new List<Damageable>();
    
    public event Action<List<Damageable>> MeAttackedChenged;

    public void Init(int maxHealth, int maxAttackers = 1)
    {
        CurrentHealth = maxHealth;
        MaxHealth = maxHealth;
        IsDead = false;
        _maxCountAttackers = maxAttackers;
    }

    public bool Attacked(Damageable damageable)
    {
        if (_listAttackedUnits.Count >= _maxCountAttackers) return false;
        else
        {
            if (!_listAttackedUnits.Contains(damageable))
            {
                _listAttackedUnits.Add(damageable);
                damageable.OnDeath += MeAttackedDead;
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
            if (_listAttackedUnits[i].IsDead)
            {
                _listAttackedUnits[i].OnDeath-= MeAttackedDead;
                _listAttackedUnits.Remove(_listAttackedUnits[i]);
            }
        }
        MeAttackedChenged?.Invoke(_listAttackedUnits);
    }

    public void MakeDamage(int damage)
    {
        CurrentHealth -= damage;
        OnHealthChanged?.Invoke(MaxHealth,CurrentHealth);
        if (CurrentHealth <= 0)
        {
            IsDead = true;
            _listAttackedUnits.Clear();
            OnDeath?.Invoke();
            
        }
    }

    public void MakeDamage(int damage, IDamageable damageDealer)
    {
        CurrentHealth -= damage;
        OnHealthChanged?.Invoke(MaxHealth,CurrentHealth);
        if (CurrentHealth <= 0)
        {
            IsDead = true;
            _listAttackedUnits.Clear();
            OnDeath?.Invoke();
        }
        else
        {
            OnDamaged?.Invoke(damageDealer);
        }
    }

}