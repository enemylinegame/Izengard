namespace UnitSystem
{
    public interface IUnitAnimationView
    {
        bool IsMoving { set; }
        void Reset();
        void StartCast();
        void StartAttack();
        void StartDead();
        void TakeDamage();
    }
}