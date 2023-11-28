using UnityEngine;

namespace BattleSystem.Obstacle
{
    [CreateAssetMenu(fileName = nameof(DefenWallConfig), menuName = "Building/" + nameof(DefenWallConfig))]
    public class DefenWallConfig : ScriptableObject
    {
        [SerializeField] private int _healtPoints;

        public int HealtPoints => _healtPoints;
    }
}
