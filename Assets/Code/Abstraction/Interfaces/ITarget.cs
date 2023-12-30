using UnityEngine;

namespace Abstraction
{
    public interface ITarget
    {
        string Id { get; }

        Vector3 Position { get; }
    }
}
