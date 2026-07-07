using System;
using _Project.Scripts.Auth;
using _Project.Scripts.Core;
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
        private GameOverResultEvent gameOverResultEvent;
        private IScoreSystem scoreSystem;
        private IGridSystem gridSystem;

        public GameOverSystem(TetrominoGenerator tetrominoGenerator, GameOverResultEvent gameOverResultEvent, IScoreSystem scoreSystem, IGridSystem gridSystem)
        {
            this.tetrominoGenerator = tetrominoGenerator;
            this.gameOverResultEvent = gameOverResultEvent;
            this.scoreSystem = scoreSystem;
            this.gridSystem = gridSystem;
            gridSystem.OnGameOver += HandleGameOver;
            
        }

        private void HandleGameOver()
        {
            tetrominoGenerator.Stop();
            UINavigationManager.Instance.NavigateTo("9e392b27-fe0f-476a-975f-a5c2983282d9");
            ServiceLocator.Instance.GetService<ILeaderboardService>().GetPlayerLeaderboardPoints("Leaderboard",
                maxPoints =>
                {
                    var score  = scoreSystem.GetScore();
                    if (score > maxPoints)
                    {
                        ServiceLocator.Instance.GetService<ILeaderboardService>()
                            .UpdatePlayerStatics("Leaderboard", score);
                        gameOverResultEvent.Raise(new GameOverData(){isNewRecord = true, score = score});
                    }
                    else
                    {
                        gameOverResultEvent.Raise(new GameOverData(){ isNewRecord = false,  score = score});
                    }
                    
                });
            
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