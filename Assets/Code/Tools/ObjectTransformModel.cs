using UnityEngine;

namespace Tools
{
    public class ObjectTransformModel
    {
        private readonly Transform _objectTransform;

        public ObjectTransformModel(Transform transform)
        {
            _objectTransform = transform;
        }

        public Vector3 GetPosition() =>
            _objectTransform.position;

        public void SetPosition(Vector3 position)
        {
            _objectTransform.position = position;
        }

        public Quaternion GetRotation() => 
            _objectTransform.rotation;

        public void SetRotation(Vector3 rotation)
{
            var newRotation = Quaternion.Euler(rotation);
            _objectTransform.rotation = newRotation;
        }
    }
}
