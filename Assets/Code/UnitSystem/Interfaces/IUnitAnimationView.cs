namespace UnitSystem
{
    public interface IUnitAnimationView
    {
        bool IsMoving { set; }
        void Reset();
        void StartCase();
        void StartAttack();
        void StartDead();
        void TakeDamage();
    }
}