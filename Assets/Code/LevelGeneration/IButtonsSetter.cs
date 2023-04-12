using System;
using UnityEngine;


namespace LevelGenerator.Interfaces
{
    public interface IButtonsSetter: IOnLateUpdate, IDisposable
    {
        void SetButtons(Vector2Int tileGridPosition);
    }
}