using System;
using UnityEngine;

namespace _Project.Scripts.Score
{
    public class ScoreSystem : MonoBehaviour
    {
        public static  ScoreSystem Instance;
        private ScoreModel scoreModel;

        private void Awake()
        {
            Instance = this;
        }

        public int GetScore() => scoreModel.score.Value;
        public void SetScoreModel(ScoreModel scoreModel)
        {
            if (this.scoreModel)
            {
                this.scoreModel = null;
            }
            this.scoreModel = scoreModel;
        }
        
        public void AddScore(int amount)
        {
            scoreModel.score.Value += amount;
        }

        public void ResetScore()
        {
           // scoreModel.score.Value = 0;
        }

        public void DecreaseScore(int amount)
        {
            scoreModel.score.Value -= amount;
        }
    }
}