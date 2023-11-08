using UnityEngine;

namespace Tools
{
    /// <summary>
    /// Only for test aoe range. 
    /// Should be deleted before merge in dev-master
    /// </summary>
    [RequireComponent(typeof(LineRenderer))]
    public class FieldOfViewDisplay : MonoBehaviour
    {
        [SerializeField] private float _viewRadius;
        [Range(0, 360)]
        [SerializeField] private float _viewAngle;
        [SerializeField] private Material _lineMaterial;

        private float _lineWidth = 0.01f;
        private LineRenderer lineRenderer;

        private void Start()
        {
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount = 4; 

            lineRenderer.startWidth = _lineWidth;
            lineRenderer.endWidth = _lineWidth;
            lineRenderer.material = _lineMaterial;
        }

        private void Update()
        {
            DrawFieldOfView();
        }

        private void DrawFieldOfView()
        {
            Vector3 origin = transform.position;
            Vector3 end1 = origin + Quaternion.Euler(0, _viewAngle / 2, 0) * transform.forward * _viewRadius;
            Vector3 end2 = origin + Quaternion.Euler(0, -_viewAngle / 2, 0) * transform.forward * _viewRadius;

            lineRenderer.SetPosition(0, origin);
            lineRenderer.SetPosition(1, end1);
            lineRenderer.SetPosition(2, end2);
            lineRenderer.SetPosition(3, origin);
        }

    }
}
