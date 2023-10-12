using UnityEngine;

namespace Abstraction
{
    public class NoneTarget : ITarget
    {
        public int Id => -1;

        public Vector3 Position => Vector3.zero;
    }
}
