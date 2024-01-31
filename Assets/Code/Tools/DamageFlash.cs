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

        private List<Material> _materials = new();

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
        }


        public void Flash()
        {
            StopCoroutine(SwitchColor());

            StartCoroutine(SwitchColor());
        }

        private IEnumerator SwitchColor()
        {
            for (int i = 0; i < _materials.Count; i++)
            {
                var material = _materials[i];
                material.color = Color.red;
            }

            yield return new WaitForSeconds(_flashTime);

            for (int i = 0; i < _materials.Count; i++)
            {
                var material = _materials[i];
                material.color = Color.white;
            }
        }
    }
}
