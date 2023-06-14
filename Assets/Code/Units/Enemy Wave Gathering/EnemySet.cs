using System.Collections.Generic;
using UnityEngine;


namespace Wave
{
    [CreateAssetMenu(fileName = nameof(EnemySet), menuName = "Wave/" + nameof(EnemySet))]
    public class EnemySet : ScriptableObject
    {
        [SerializeField] private List<EnemySettings> enemies;
        public IReadOnlyList<EnemySettings> Enemies => enemies;
    }
}