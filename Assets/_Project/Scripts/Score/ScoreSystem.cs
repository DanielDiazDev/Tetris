using System;
using _Project.Scripts.Grid;
using UnityEngine;

namespace _Project.Scripts.Score
{
    public class ScoreSystem : IScoreSystem, IDisposable
    {
        private ScoreModel scoreModel;
        private IGridSystem gridSystem;
        

        public ScoreSystem(ScoreModel scoreModel, IGridSystem gridSystem)
        {
            this.scoreModel = scoreModel;
            gridSystem.OnAddScore += AddScore;
            
        }
        public int GetScore() => scoreModel.GetScore();
        public void AddScore(int amount)
        {
            scoreModel.AddScore(amount);
        }

        public void Dispose()
        {
            if (gridSystem != null)
            {
                gridSystem.OnAddScore -= AddScore;
            }
        }
    }
}