using _Project.Scripts.SOAP.Events;
using UnityEngine;

namespace _Project.Scripts.Score
{
    public class LevelSystem : MonoBehaviour
    {
        public static LevelSystem Instance;
        [SerializeField] private IntEvent setLevelEvent;
        public int Level { get; private set; }

        private void Awake()
        {
            Instance = this;
            ResetLevel();
        }

        public void IncreaseLevel()
        {
            Level++;
            setLevelEvent.Raise(Level);
        }

        public void ResetLevel()
        {
            Level = 1;
            setLevelEvent.Raise(Level);
        }
    }
}