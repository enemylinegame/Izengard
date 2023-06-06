using UnityEngine;

namespace EquipmentSystem
{ 
    public class UnitView : MonoBehaviour,IHealthHolder
    {
        [SerializeField] private Inventory thisUnitInventory;
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _healthBarTransform;
        [SerializeField] private Transform _hireCameraPosition1;
        [SerializeField] private string NameOfCharacter;
        [SerializeField] private float _currentHealt;
        [SerializeField] private float _maxHealth;
        [SerializeField] private Sprite _icon;

        [SerializeField] private GameObject UnitPrefab;

        public float CurrentHealth => _currentHealt;
        public float MaxHealth => _maxHealth;

        public Sprite Icon => _icon;


        public string GetNameOfCharacter()
        {
            return NameOfCharacter;
        }
        public Transform Get_hireCameraPosition()
        {
            return _hireCameraPosition1;
        }
        public Inventory GetUnitInventory()
        {
            return thisUnitInventory;
        }
        public void RebindAnimation()
        {
            _animator.Rebind();
        }
        public void SetCurrentHealth(float value)
        {
            _currentHealt = value;
        }
        public void SetMaxHealth(float value)
        {
            _maxHealth = value;
        }
        public void SetIcon(Sprite icon)
        {
            _icon = icon;
        }
        public void AwakeInventory()
        {
            thisUnitInventory.SetUnitForSlots(this);

            thisUnitInventory.ArmorSlotsController.AwakeAllArmor();
            
            
        }
        
        public void Awake()
        {
            
            AwakeInventory();
        }
    }
}
