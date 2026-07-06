using _Project.Scripts.Grid;
using UnityEngine;

namespace Project.Scripts.Pieces
{
    public class GhostTetrominoController
    {
        private GameObject ghostTetromino;
        private IGridSystem gridSystem;
        public GameObject GhostTetromino =>  ghostTetromino;

        public GhostTetrominoController(IGridSystem gridSystem)
        {
            this.gridSystem = gridSystem;
        }

        public void CreateGhost(GameObject tetromino)
        {
            if (ghostTetromino != null)
            {
                Object.Destroy(ghostTetromino.gameObject);
                ghostTetromino = null;
            }
            ghostTetromino = Object.Instantiate(tetromino, tetromino.transform.position, Quaternion.identity);
            foreach (var sr in ghostTetromino.GetComponentsInChildren<SpriteRenderer>())
            {
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.3f); // Transparente
            }

            var tetrominoScript = ghostTetromino.GetComponent<Tetromino>();
            Object.Destroy(tetrominoScript);
        }
        public void UpdateGhost(GameObject tetromino)
        {
            ghostTetromino.transform.position = tetromino.transform.position;
            ghostTetromino.transform.rotation = tetromino.transform.rotation;
        
            while (gridSystem.IsWithin(ghostTetromino.transform))
            {
                ghostTetromino.transform.position += Vector3.down;
            }
            ghostTetromino.transform.position -= Vector3.down;
            SnapParent();
        }
        private void SnapParent()
        {
            ghostTetromino.transform.position = gridSystem.GridToWorldCenter(
                gridSystem.WorldToGrid(ghostTetromino.transform.position).x,
                gridSystem.WorldToGrid(ghostTetromino.transform.position).y
            );
        }
    }
}