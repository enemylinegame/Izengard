using UnityEngine;

namespace BattleSystem.Buildings.Interfaces
{
    public interface IWarBuildingView
    {
        Vector3 Position { get; }
        void Init(int id);
        void Show();
        void Hide();
        void ChangeHealth(int hpValue);
    }
}