using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Scripts.Pieces
{
    public class TetrominoGenerator : MonoBehaviour
    {
        public GameObject[] tetrominoPrefabs;

        private void Start()
        {
            Generate();
        }

        public void Generate()
        {
            Instantiate(tetrominoPrefabs[Random.Range(0, tetrominoPrefabs.Length)], transform.position, Quaternion.identity);
        }
    }
}