using UnityEngine;


public class HealthBar : MonoBehaviour {

    private MaterialPropertyBlock matBlock;
    private MeshRenderer _meshRenderer;
    private Camera _mainCamera;
    private IHealthHolder _damageable;

    private void Awake() {
        _meshRenderer = GetComponent<MeshRenderer>();
        matBlock = new MaterialPropertyBlock();
        _damageable = GetComponentInParent<Damageable>();
    }

    private void Start() {
        _mainCamera = Camera.main;
    }

    private void Update() {

        if (_damageable == null)
        {
            AlignCamera();
            return; 
        }
        if (_damageable.Health < _damageable.MaxHealth) {
            _meshRenderer.enabled = true;
            AlignCamera();
            UpdateParams();
        } else {
            _meshRenderer.enabled = false;
        }
    }

    private void UpdateParams() {
        _meshRenderer.GetPropertyBlock(matBlock);
        matBlock.SetFloat("_Fill", _damageable.Health / (float)_damageable.MaxHealth);
        _meshRenderer.SetPropertyBlock(matBlock);
    }

    private void AlignCamera() {
        if (_mainCamera != null) {
            var camXform = _mainCamera.transform;
            var forward = transform.position - camXform.position;
            forward.Normalize();
            var up = Vector3.Cross(forward, camXform.right);
            transform.rotation = Quaternion.LookRotation(forward, up);
        }
    }

}