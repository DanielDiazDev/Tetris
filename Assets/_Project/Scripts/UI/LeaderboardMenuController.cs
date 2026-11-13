using UnityEngine;

/// <summary>Switches between the main menu and leaderboard canvases.</summary>
public sealed class LeaderboardMenuController : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private GameObject leaderboardCanvas;

    private void Start()
    {
        if (PlayerPrefs.GetInt("OpenLeaderboardOnLoad", 0) == 1)
        {
            PlayerPrefs.DeleteKey("OpenLeaderboardOnLoad");
            OpenLeaderboard();
        }
    }

    public void OpenLeaderboard()
    {
        if (mainMenuCanvas != null) mainMenuCanvas.SetActive(false);
        if (leaderboardCanvas != null) leaderboardCanvas.SetActive(true);
    }

    public void BackToMainMenu()
    {
        if (leaderboardCanvas != null) leaderboardCanvas.SetActive(false);
        if (mainMenuCanvas != null) mainMenuCanvas.SetActive(true);
    }

}
