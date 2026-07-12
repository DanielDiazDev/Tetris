using System;
using UnityEngine;

namespace _Project.Scripts.SOAP.Events
{
    [CreateAssetMenu(fileName = "GameOverResultEvent", menuName = "ScriptableObjects/SOAP/GameEvent/GameOverResultEvent")]
    public class GameOverResultEvent : GameEvent<GameOverData>
    {
    }

    [Serializable]
    public class GameOverData
    {
        public int score;
        public bool isNewRecord;
        public int level;
    }
}