using ResourceSystem;
using Wave;

namespace Wave
{
    public class EnemyDestroyObserver
    {
        private readonly GlobalStock _globalStock;

        public EnemyDestroyObserver(GlobalStock globalStock)
        {
            _globalStock = globalStock;
        }


        public void EnemyDestroyed(Enemy enemy)
        {
            _globalStock.AddResourceToStock(ResourceType.Gold, enemy.Stats.Gold);
        }
        
    }
}