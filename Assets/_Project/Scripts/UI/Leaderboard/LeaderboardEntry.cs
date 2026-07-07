using _Project.Scripts.Auth;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public class LeaderboardEntry : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI placeText;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI scoreText;

        public void Init(PlayerLeaderboardEntryData data)
        {
            placeText.text = $"{data.place + 1}";
            nameText.text = data.leaderboardName;
            scoreText.text = data.score.ToString();
        }
    }
    
}