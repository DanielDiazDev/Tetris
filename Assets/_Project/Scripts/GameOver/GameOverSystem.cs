using System;
using _Project.Scripts.Grid;
using _Project.Scripts.PlayFab.Interfaces;
using _Project.Scripts.Score;
using _Project.Scripts.SOAP.Events;
using _Project.Scripts.UINodeGraph.Runtime;
using Project.Scripts.Pieces;
using UnityEngine;
using UnityServiceLocator;

namespace _Project.Scripts.GameOver
{
    public class GameOverSystem : IDisposable
    {
        private TetrominoGenerator tetrominoGenerator;
        private GameOverResultEvent gameOverResultEvent;
        private IScoreSystem scoreSystem;
        private IGridSystem gridSystem;
        private LevelSystem levelSystem;

        public GameOverSystem(TetrominoGenerator tetrominoGenerator, GameOverResultEvent gameOverResultEvent, IScoreSystem scoreSystem, IGridSystem gridSystem, LevelSystem levelSystem)
        {
            this.tetrominoGenerator = tetrominoGenerator;
            this.gameOverResultEvent = gameOverResultEvent;
            this.scoreSystem = scoreSystem;
            this.gridSystem = gridSystem;
            this.levelSystem = levelSystem;
            gridSystem.OnGameOver += HandleGameOver;
            
        }

        private void HandleGameOver()
        {
            tetrominoGenerator.Stop();
            UINavigationManager.Instance.NavigateTo("88800111-8d56-4c8d-b558-d842fd2c9583");
            ServiceLocator.Global.Get<ICurrencyService>().AddCurrency("SC", 1);
            
            ServiceLocator.Global.Get<ILeaderboardService>().GetPlayerLeaderboardPoints("Leaderboard",
                maxPoints =>
                {
                    var score  = scoreSystem.GetScore();
                    if (score > maxPoints)
                    {
                        ServiceLocator.Global.Get<ILeaderboardService>()
                            .UpdatePlayerStatics("Leaderboard", score);
                        gameOverResultEvent.Raise(new GameOverData(){isNewRecord = true, score = score, level = levelSystem.Level});
                        ServiceLocator.Global.Get<ICurrencyService>().AddCurrency("SC", 20);
                    }
                    else
                    {
                        gameOverResultEvent.Raise(new GameOverData(){ isNewRecord = false,  score = score, level = levelSystem.Level});
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
