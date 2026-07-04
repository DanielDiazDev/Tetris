using System;
using UnityEngine;

namespace _Project.Scripts.Score
{
    public class ScoreSystem : IScoreSystem
    {
        private ScoreModel scoreModel;

        public ScoreSystem(ScoreModel scoreModel)
        {
            this.scoreModel = scoreModel;
        }
        public int GetScore() => scoreModel.GetScore();
        public void AddScore(int amount)
        {
            scoreModel.AddScore(amount);
        }
    }
}