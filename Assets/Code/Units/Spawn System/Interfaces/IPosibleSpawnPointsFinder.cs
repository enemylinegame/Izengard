using System.Collections.Generic;
using UnityEngine;

namespace SpawnSystem.Interfaces
{
    interface IPosibleSpawnPointsFinder
    {
        List<Vector3> GetPosibleSpawnPoints();
    }
}
