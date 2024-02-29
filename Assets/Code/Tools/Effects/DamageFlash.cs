using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace Tools
{
    public class DamageFlash : MonoBehaviour
    {
        [SerializeField]
        private MeshRenderType meshRenderType;
        [SerializeField]
        private float _flashTime = 0.15f;
        [SerializeField]
        private Color _flashColor = Color.red;

        private List<Material> _materials = new();
        private Color[] _cashedColors;

        private void Awake()
        {
            switch (meshRenderType)
            {
                case MeshRenderType.MeshRender: 
                    {
                        var meshes = GetComponentsInChildren<MeshRenderer>();

                        foreach(var mesh in meshes)
                        {
                            _materials.Add(mesh.material);
                        }

                        break;
                    }
                case MeshRenderType.SkinMeshRender:
                    {
                        var meshes = GetComponentsInChildren<SkinnedMeshRenderer>();

                        foreach (var mesh in meshes)
                        {
                            _materials.Add(mesh.material);
                        }
                        break;
                    }
            }

            _cashedColors = new Color[_materials.Count];
            
            for (int i = 0; i < _materials.Count; i++)
            {
                var material = _materials[i];

                _cashedColors[i] = material.color;
            }
        }


        public void Flash()
        {
            ResetColor();

            StopCoroutine(SwitchColor());

            StartCoroutine(SwitchColor());
        }

        private void ResetColor()
        {
            for (int i = 0; i < _materials.Count; i++)
            {
                var material = _materials[i];
                material.color = _cashedColors[i];
            }
        }

        private IEnumerator SwitchColor()
        {

            for (int i = 0; i < _materials.Count; i++)
            {
                var material = _materials[i];

                material.color = _flashColor;
            }

            yield return new WaitForSeconds(_flashTime);

            ResetColor();
        }
    }
}
