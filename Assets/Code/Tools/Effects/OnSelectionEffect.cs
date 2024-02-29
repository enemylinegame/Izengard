using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
    public class OnSelectionEffect : MonoBehaviour
    {
        [SerializeField]
        private MeshRenderType meshRenderType;
        [SerializeField]
        private Material _selctionMaterial;

        private List<Renderer> _renderers = new();

        private List<Material> _oldMaterials = new();

        private void Awake()
        {
            switch (meshRenderType)
            {
                case MeshRenderType.MeshRender:
                    {
                        var renderers = GetComponentsInChildren<MeshRenderer>();

                        foreach (var renderer in renderers)
                        {
                            _renderers.Add(renderer);
                        }

                        break;
                    }
                case MeshRenderType.SkinMeshRender:
                    {
                        var renderers = GetComponentsInChildren<SkinnedMeshRenderer>();

                        foreach (var renderer in renderers)
                        {
                            _renderers.Add(renderer);
                        }

                        break;
                    }
            }
        }

        public void Display()
        {
            _oldMaterials.Clear();

            for (int i = 0; i < _renderers.Count; i++)
            {
                _oldMaterials.Add(_renderers[i].material);

                _renderers[i].material = _selctionMaterial;
            }
        }

        public void Hide()
        {
            if (_oldMaterials.Count == 0)
                return;

            for (int i = 0; i < _renderers.Count; i++)
            {
                _renderers[i].material = _oldMaterials[i];
            }
        }
    }
}
