namespace Abstraction
{
    public class DamageStructure : IDamage
    {
        private float _baseDamage;
        private float _fireDamage;
        private float _coldDamage;
        
        public float BaseDamage
        {
            get => _baseDamage;
            set => _baseDamage = value;
        }
        public float FireDamage
        {
            get => _fireDamage;
            set => _fireDamage = value;
        }
        public float ColdDamage
        {
            get => _coldDamage;
            set => _coldDamage = value;
        }

        public DamageStructure() { }
    }
}
