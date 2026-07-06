using System;
using _Project.Scripts.Grid;
using _Project.Scripts.SOAP.Events;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Scripts.Pieces
{
    public class TetrominoGenerator : MonoBehaviour
    {
        [SerializeField] private Tetromino[] tetrominoPrefabs;
        [SerializeField] private SpriteEvent nextSpriteEvent;
        [SerializeField] private TetrominoEvent tetrominoLockedEvent;
        private int currentIndex;
        private int nextIndex;
        private IGridSystem gridSystem;
        private bool canGenerate = true;
       // private GhostTetrominoController ghostTetrominoController;

        private void OnEnable()
        {
            tetrominoLockedEvent.Subscribe(HandleTetrominoLocked);
        }

        private void OnDestroy()
        {
            tetrominoLockedEvent.Unsubscribe(HandleTetrominoLocked);
        }

        private void HandleTetrominoLocked(Tetromino tetromino)
        {
            gridSystem.AddToGrid(tetromino.transform);
            gridSystem.CheckLines();
            Generate();
        }

        public void Init(IGridSystem gridSystem)
        {
            this.gridSystem = gridSystem;
            nextIndex = Random.Range(0, tetrominoPrefabs.Length);
        //    ghostTetrominoController = new  GhostTetrominoController(gridSystem);
            Generate();
        }

        public void Stop()
        {
            canGenerate = false;
        }

        public void Generate()
        {
            if (!canGenerate) return; 
    
            currentIndex = nextIndex;
            var worldPosToGrid = gridSystem.WorldToGrid(transform.position);
            var tetromino = Instantiate(
                tetrominoPrefabs[currentIndex], 
                gridSystem.GridToWorldCenter(worldPosToGrid.x, worldPosToGrid.y), 
                Quaternion.identity
            ).GetComponent<Tetromino>();
            var ghostTetrominoController = new GhostTetrominoController(gridSystem);
            tetromino.Init(gridSystem, ghostTetrominoController);
            if (!gridSystem.IsWithin(tetromino.transform))
            {
                Stop();
                Destroy(ghostTetrominoController.GhostTetromino.gameObject);
                gridSystem.TriggerGameOver();
                return;
            }
            nextIndex = Random.Range(0, tetrominoPrefabs.Length);
            nextSpriteEvent.Raise(tetrominoPrefabs[nextIndex].sprite);
        }

        
        

       
        
    }
}