using System;
using System.Collections.Generic;
using System.Linq;
using Code.QuickOutline.Scripts;
using UnityEngine;
using Object = UnityEngine.Object;

namespace QuickOutline
{
    public class OutlineController
    {
        private static HashSet<Mesh> registeredMeshes = new HashSet<Mesh>();
        [Serializable]
  
        private class ListVector3 { public List<Vector3> data; }
        private Color _outlineColor;
        private float _outlineWidth;
        private Renderer[] renderers;
        private Material _outlineMaskMaterial;
        private Material _outlineFillMaterial;
        
        private List<Mesh> bakeKeys = new List<Mesh>();
        private List<ListVector3> bakeValues = new List<ListVector3>();

        public OutlineController(OutLineSettings config)
        {
            _outlineMaskMaterial = Object.Instantiate(config.OutlineMaskMaterial);
            _outlineFillMaterial = Object.Instantiate(config.OutlineFillMaterial);
            _outlineColor = config.OutLineColor;
            _outlineWidth = config.OutlineWidth;
    
            _outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
            _outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
            _outlineFillMaterial.SetFloat("_OutlineWidth", _outlineWidth);
        }

        public void EnableOutLine(Renderer renderer)
        {
            LoadSmoothNormals(renderer.GetComponentInChildren<MeshFilter>(), renderer);
            _outlineFillMaterial.SetColor("_OutlineColor", _outlineColor);
            var materials = renderer.sharedMaterials.ToList();
  
            materials.Add(_outlineMaskMaterial);
            materials.Add(_outlineFillMaterial);

            renderer.materials = materials.ToArray();
        }

        public void DisableOutLine(Renderer renderer)
        {
            LoadSmoothNormals(renderer.GetComponentInChildren<MeshFilter>(), renderer);
            var materials = renderer.sharedMaterials.ToList();

            _outlineFillMaterial.SetColor("_OutlineColor", Color.clear);

            materials.Remove(_outlineFillMaterial);
            materials.Remove(_outlineMaskMaterial);
    
            renderer.materials = materials.ToArray();
        }

        private void LoadSmoothNormals(MeshFilter meshFilter, Renderer renderer)
        {
            if(!registeredMeshes.Add(meshFilter.sharedMesh))
            {
                var index = bakeKeys.IndexOf(meshFilter.sharedMesh);
                var smoothNormals = (index >= 0) ? bakeValues[index].data : SmoothNormals(meshFilter.sharedMesh);
                
                meshFilter.sharedMesh.SetUVs(3, smoothNormals);

                if (renderer != null) 
                {
                    CombineSubmeshes(meshFilter.sharedMesh, renderer.sharedMaterials);
                }
            }
        }

        private List<Vector3> SmoothNormals(Mesh mesh)
        {
            // Group vertices by location
            var groups = mesh.vertices.Select((vertex, index) => new KeyValuePair<Vector3, int>(vertex, index)).GroupBy(pair => pair.Key);

            // Copy normals to a new list
            var smoothNormals = new List<Vector3>(mesh.normals);

            // Average normals for grouped vertices
            foreach (var group in groups) {

                // Skip single vertices
                if (group.Count() == 1) {
                    continue;
                }

                // Calculate the average normal
                var smoothNormal = Vector3.zero;

                foreach (var pair in group) {
                    smoothNormal += smoothNormals[pair.Value];
                }

                smoothNormal.Normalize();

                // Assign smooth normal to each vertex
                foreach (var pair in group) {
                    smoothNormals[pair.Value] = smoothNormal;
                }
            }

            return smoothNormals;
        }

        private void CombineSubmeshes(Mesh mesh, Material[] materials)
        {
            if (mesh.subMeshCount == 1) {
                return;
            }

            if (mesh.subMeshCount > materials.Length) {
                return;
            }

            mesh.subMeshCount++;
            mesh.SetTriangles(mesh.triangles, mesh.subMeshCount - 1);
        }
    }
}