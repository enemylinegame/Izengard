using System;
using LevelGenerator;
using UnityEngine;

namespace Code.Level_Generation
{
    public class ButtonSetterView : MonoBehaviour
    {
        public Material highlightMaterial;
        private Material originalMaterial;
        private Renderer renderer;
        public event Action onSpawnTileButtonClick;

        void Start()
        {
            renderer = GetComponent<Renderer>();
            originalMaterial = renderer.material;
        }

        void OnMouseEnter()
        {
            renderer.material = highlightMaterial;
        }

        void OnMouseExit()
        {
            renderer.material = originalMaterial;
        }

        void OnMouseDown()
        {
            onSpawnTileButtonClick?.Invoke();
        }
    }
}