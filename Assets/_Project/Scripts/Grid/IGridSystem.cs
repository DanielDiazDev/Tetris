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

        void CheckLines();
        bool IsWithin(Transform tetromino);
        event Action<int> OnAddScore;
        event Action OnCheckLevel;
    }
}