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
    public float CurrentHealth { get; set; }
    public float MaxHealth {get; set;}

    public Vector3 Position => transform.position;

    public int ThreatLevel => _threatLevel;

    public int NumberOfAttackers => _listAttackedUnits.Count;
    
    public event Action<float, float> OnHealthChanged; 
    public event Action OnDeath;
    public event Action<IDamageable> OnDamaged;

    public bool IsDead { get; private set; }

    private List<Damageable> _listAttackedUnits = new List<Damageable>();
    
    public event Action<List<Damageable>> MeAttackedChenged;
    
    public Animator animator;


  

    public void Init(int maxHealth, int maxAttackers = 1)
    {
        CurrentHealth = maxHealth;
        MaxHealth = maxHealth;
        IsDead = false;
        _maxCountAttackers = maxAttackers;
        animator = GetComponent<Animator>();
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

    public float AddHp(float value)
    {
        return CurrentHealth = value;
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
        animator.SetTrigger("TakeDamage"); //анимация получения урона 
        CurrentHealth -= damage;
        OnHealthChanged?.Invoke(MaxHealth,CurrentHealth);
        

        if (CurrentHealth <= 0)
        {
            animator.SetBool("EnemyDead", true); //анимация смерти (пока что не работает, так как враг деспаунится быстрее анимации)
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