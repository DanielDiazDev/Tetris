using System;
using _Project.Scripts.SOAP.Events;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Scripts.Pieces
{
    public class TetrominoGenerator : MonoBehaviour
    {
        [SerializeField] private Tetromino[] tetrominoPrefabs;
        [SerializeField] private SpriteEvent nextSpriteEvent;
        private int currentIndex;
        private int nextIndex;


       

        private void Start()
        {
            nextIndex = Random.Range(0, tetrominoPrefabs.Length);
            Generate();
        }

        public void Generate()
        {
            currentIndex = nextIndex;
            Instantiate(tetrominoPrefabs[currentIndex], transform.position, Quaternion.identity);
            nextIndex = Random.Range(0, tetrominoPrefabs.Length);
            nextSpriteEvent.Raise(tetrominoPrefabs[nextIndex].sprite);
            
        }
    }
}