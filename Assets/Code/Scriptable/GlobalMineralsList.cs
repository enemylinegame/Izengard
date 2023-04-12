using System;
using System.Collections.Generic;
using ResourceSystem;
using UnityEngine;

namespace Code.Scriptable
{
    [CreateAssetMenu(fileName = "Global Minerals List", menuName = "Resources/Global Gatherables List", order = 1)]
    [Serializable]
    public class GlobalMineralsList : ScriptableObject
    {
        public List<MineralConfig> Minerals;
    }
}