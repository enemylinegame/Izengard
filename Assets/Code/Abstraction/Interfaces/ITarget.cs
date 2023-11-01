using UnityEngine;

namespace Abstraction
{
    public interface ITarget
    {
        int Id { get; }

        Vector3 Position { get; }
    }
}
