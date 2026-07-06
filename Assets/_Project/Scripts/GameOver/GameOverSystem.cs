using System;
using _Project.Scripts.Grid;
using _Project.Scripts.Score;
using _Project.Scripts.SOAP.Events;
using _Project.Scripts.UINodeGraph.Runtime;
using Project.Scripts.Pieces;

namespace _Project.Scripts.GameOver
{
    public class GameOverSystem : IDisposable
    {
        private TetrominoGenerator tetrominoGenerator;
        private IntEvent finalScoreEvent;
        private IScoreSystem scoreSystem;
        private IGridSystem gridSystem;

        public GameOverSystem(TetrominoGenerator tetrominoGenerator, IntEvent finalScoreEvent, IScoreSystem scoreSystem, IGridSystem gridSystem)
        {
            this.tetrominoGenerator = tetrominoGenerator;
            this.finalScoreEvent = finalScoreEvent;
            this.scoreSystem = scoreSystem;
            this.gridSystem = gridSystem;
            gridSystem.OnGameOver += HandleGameOver;
            
        }

        private void HandleGameOver()
        {
            tetrominoGenerator.Stop();
            UINavigationManager.Instance.NavigateTo("9e392b27-fe0f-476a-975f-a5c2983282d9");
            finalScoreEvent.Raise(scoreSystem.GetScore());
        }

        public void Dispose()
        {
            if (gridSystem != null)
            {
                gridSystem.OnGameOver -= HandleGameOver;
            }
        }
    }
}