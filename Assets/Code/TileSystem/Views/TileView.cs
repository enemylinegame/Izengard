using System;
using System.Collections.Generic;
using Code.BuildingSystem;
using Code.BuldingsSystem;
using Code.BuldingsSystem.ScriptableObjects;
using Code.UI;
using CombatSystem;
using ResourceSystem;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Views.BuildBuildingsUI;

namespace Code.TileSystem
{
    public class TileView : MonoBehaviour
    {
        [SerializeField] private TileConfig _tileConfig;
        public Renderer Renderer;
        [SerializeField] private List<Dot> _dotSpawns;
        public TileModel TileModel;
        private void Awake()
        {
            TileModel = new TileModel();
            TileModel.TileConfig = _tileConfig;
            TileModel.DotSpawns = _dotSpawns;
            TileModel.Init();
        }

    }
}