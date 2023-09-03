using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace Code.TileSystem
{
    public class TileView : MonoBehaviour
    {
        [SerializeField]private string _id;
        public List<Dot> DotSpawns; 
        public Renderer Renderer;
        //public TileModel TileModel;
        public string ID => _id;

        public void Start()
        {
            
        }

        private void Awake()
        {
            _id = Guid.NewGuid().ToString();
            //TileModel = new TileModel();
            //TileModel.DotSpawns = _dotSpawns;
            //TileModel.TilePosition = transform.position;
        }

    }
}