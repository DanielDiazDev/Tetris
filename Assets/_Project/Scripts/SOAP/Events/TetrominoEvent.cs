using Project.Scripts.Pieces;
using UnityEngine;

namespace _Project.Scripts.SOAP.Events
{
    [CreateAssetMenu(fileName = "TetrominoEvent", menuName = "ScriptableObjects/SOAP/GameEvent/TetrominoEvent")]
    public class TetrominoEvent : GameEvent<Tetromino>
    {
    }
}