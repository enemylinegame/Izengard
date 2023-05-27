using System;
using System.Collections.Generic;
using System.Linq;
using Code.QuickOutline.Scripts;
using UnityEngine;
using Object = UnityEngine.Object;

public class OutlineController
{
  private static HashSet<Mesh> registeredMeshes = new HashSet<Mesh>();
  
  [Serializable]
  private class ListVector3 {
    public List<Vector3> data;
  }
  private Color _outlineColor;
  private float _outlineWidth;
  private Renderer[] renderers;
  private Material _outlineMaskMaterial;
  private Material _outlineFillMaterial;
  
  private bool precomputeOutline;

  [SerializeField, HideInInspector]
  private List<Mesh> bakeKeys = new List<Mesh>();

  [SerializeField, HideInInspector]
  private List<ListVector3> bakeValues = new List<ListVector3>();

  public OutlineController(OutLineSettings config)
  {
    _outlineMaskMaterial = config.OutlineMaskMaterial;
    _outlineFillMaterial = config.OutlineFillMaterial;
    _outlineColor = config.OutLineColor;
    _outlineWidth = config.OutlineWidth;
  }

  public void EnableOutLine(Renderer renderer) 
  {
    LoadSmoothNormals(renderer.GetComponent<MeshFilter>());
    _outlineFillMaterial.SetColor("_OutlineColor", _outlineColor);
    _outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
    _outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
    _outlineFillMaterial.SetFloat("_OutlineWidth", _outlineWidth);
    var materials = renderer.sharedMaterials.ToList();
  
    materials.Add(_outlineMaskMaterial);
    materials.Add(_outlineFillMaterial);
    
    renderer.materials = materials.ToArray();
  }
  
  public void DisableOutLine(Renderer renderer) 
  {
    var materials = renderer.sharedMaterials.ToList();

    _outlineFillMaterial.SetColor("_OutlineColor", Color.clear);

    materials.Remove(_outlineFillMaterial);
    materials.Remove(_outlineMaskMaterial);
      
    renderer.materials = materials.ToArray();
  }
  
  
  private void LoadSmoothNormals(MeshFilter meshFilter) 
  {

    // Skip if smooth normals have already been adopted
    if(!registeredMeshes.Add(meshFilter.sharedMesh))
    {

      // Retrieve or generate smooth normals
      var index = bakeKeys.IndexOf(meshFilter.sharedMesh);
      var smoothNormals = (index >= 0) ? bakeValues[index].data : SmoothNormals(meshFilter.sharedMesh);

      // Store smooth normals in UV3
      meshFilter.sharedMesh.SetUVs(3, smoothNormals);

      // Combine submeshes
      var renderer = meshFilter.GetComponent<Renderer>();

      if (renderer != null) 
      {
        CombineSubmeshes(meshFilter.sharedMesh, renderer.sharedMaterials);
      }
    }
  }

  List<Vector3> SmoothNormals(Mesh mesh) {

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

  void CombineSubmeshes(Mesh mesh, Material[] materials) {

    // Skip meshes with a single submesh
    if (mesh.subMeshCount == 1) {
      return;
    }

    // Skip if submesh count exceeds material count
    if (mesh.subMeshCount > materials.Length) {
      return;
    }

    // Append combined submesh
    mesh.subMeshCount++;
    mesh.SetTriangles(mesh.triangles, mesh.subMeshCount - 1);
  }
}




