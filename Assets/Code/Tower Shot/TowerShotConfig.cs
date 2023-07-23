using UnityEngine;

namespace Code.TowerShot
{
    [CreateAssetMenu(fileName = nameof(TowerShotConfig))]
    public class TowerShotConfig : ScriptableObject
    {
        [Range(0, 50)][SerializeField] private float _fireRate; // скорострельность
        [Range(0, 50)][SerializeField] private float _rayOffset; // делаем поисковый луч, немного больше области поиска
        [Range(0, 1000)][SerializeField] private int _damage; // урон
        [Range(0, 50)][SerializeField] private float _bulletSpeed; // скорость пуль
        [Range(0, 100)][SerializeField] private int _countBullet; // количество пуль в pools

        public float FireRate => _fireRate;
        public float RayOffset
        {
            get => _rayOffset;
            set => _rayOffset = value;
        }

        public int Damage => _damage;
        public int CountBullet => _countBullet;
        public float BulletSpeed => _bulletSpeed;
    }
}