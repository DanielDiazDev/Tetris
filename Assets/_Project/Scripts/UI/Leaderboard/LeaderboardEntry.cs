using _Project.Scripts.PlayFab;
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
            placeText.color = (data.place + 1) switch
            {
                1 => Color.gold,
                2 => Color.green,
                3 => Color.darkTurquoise,
                _ => Color.antiqueWhite
            };
            nameText.text = data.leaderboardName;
            scoreText.text = data.score.ToString();
        }
    }
    
}