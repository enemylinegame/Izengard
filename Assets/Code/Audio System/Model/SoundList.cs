using System.Collections.Generic;
using UnityEngine;

namespace Audio_System
{
    [CreateAssetMenu(fileName = nameof(SoundList),
     menuName = "Audio Data/" + nameof(SoundList))]
    public class SoundList : ScriptableObject
    {
        [SerializeField] private List<SoundConfig> _sounds;
        public List<SoundConfig> Sounds => _sounds;
    }
}
