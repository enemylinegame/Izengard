using UnityEngine;

namespace Abstraction
{
    public interface IMovable
    {
        void MoveTo(Vector3 position);
        void Stop();
    }
}
