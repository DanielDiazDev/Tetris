using System;
using UnityEngine;

namespace _Project.Scripts.Grid
{
    public interface IGridSystem
    {
        event Action OnGameOver;
        void AddToGrid(Transform tetromino);
        int X  { get; }
        int Y { get; }
        void TriggerGameOver();
        void CheckLines();
        bool IsWithin(Transform tetromino);
        event Action<int> OnAddScore;
        event Action OnCheckLevel;

        Vector3 GridToWorld(int x, int y);
        Vector3 GridToWorldCenter(int x, int y);
        Vector2Int WorldToGrid(Vector3 worldPosition);
    }
}