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

        public void Init(LeaderboardEntryData data)
        {
            placeText.text = data.place.ToString();
            nameText.text = data.leaderboardName;
            scoreText.text = data.score.ToString();
        }
    }
    
}