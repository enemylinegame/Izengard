using UnityEngine;

namespace Abstraction
{
    public interface ITarget
    {
        string Id { get; }
        string Name { get; }

        Vector3 Position { get; }
    }
}
