﻿using System;
using UnitSystem.Enum;
using UnityEngine;

namespace UnitSystem.Data
{
    [Serializable]
    public class UnitMainStatsData : IUnitStatsData
    {
        [SerializeField] private UnitType _type;
        [SerializeField] private int _healthPoints = 100;
        [SerializeField] private float _size = 1f;
        [SerializeField] private float _speed = 1f;
        [SerializeField] private float _detectionRange = 5f;

        public UnitType Type => _type;
        public int HealthPoints => _healthPoints;
        public float Size => _size;
        public float Speed => _speed;
        public float DetectionRange => _detectionRange;

    }
}
