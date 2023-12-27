﻿using Abstraction;
using System;

namespace BattleSystem.Obstacle
{
    public interface IObstacle : IDamageable, IKillable<int>
    {
        public int Id { get; }

        public IParametr<int> Health { get; }

        public IObstacleView View { get; }

        public void Enable();
        public void Disable();
    }
}
