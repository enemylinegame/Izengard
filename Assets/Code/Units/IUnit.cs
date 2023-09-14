namespace Units
{
    public interface IUnit
    {
        int CurrentHealth { get; }
        int CurrentArmor { get; }

        void IncreaseHealth(int amount);
        void DecreaseHealth(int amount);
    }
}
