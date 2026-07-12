using System;
using _Project.Scripts.Grid;
using _Project.Scripts.SOAP.Events;
using UnityEngine;

namespace _Project.Scripts.Score
{
    public class LevelSystem : IDisposable
    {
        private IntEvent setLevelEvent;
        private IGridSystem gridSystem;
        private IScoreSystem scoreSystem;
        private FloatEvent setFallTimeEvent;
        private float fallTimeDecrement = 0.001f;
        public int Level { get; private set; }
        
        public LevelSystem(IGridSystem gridSystem, FloatEvent setFallTimeEvent, IScoreSystem scoreSystem, IntEvent setLevelEvent)
        {
            this.gridSystem = gridSystem;
            this.setFallTimeEvent = setFallTimeEvent;
            this.scoreSystem = scoreSystem;
            this.setLevelEvent = setLevelEvent;
            gridSystem.OnCheckLevel += CheckLevelProgression;
            
        }

        private void IncreaseLevel()
        {
            Level++;
            setLevelEvent.Raise(Level);
            setFallTimeEvent.Raise(fallTimeDecrement);
        }
        
        private void CheckLevelProgression()
        {
            int currentScore = scoreSystem.GetScore();
            int threshold = 200;

            if (currentScore >= (Level + 1) * threshold)
            {
                IncreaseLevel();
            }
        }


        public void ResetLevel()
        {
            Level = 1;
            setLevelEvent.Raise(Level);
        }

        public void SetLevel(int level)
        {
            Level = level;
            setLevelEvent.Raise(Level);
        }

        public void Dispose()
        {
            if (gridSystem != null)
            {
                gridSystem.OnCheckLevel -= CheckLevelProgression;
            }
        }
    }
}