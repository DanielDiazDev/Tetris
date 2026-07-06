using System;
using UnityEngine;

public class GameOverUITest : MonoBehaviour
{
    //public static GameOverTest Instance;
    [SerializeField] private GameObject gameOverPanel;

    private void Awake()
    {
     //   Instance = this;
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
    }

    public void HideGameOver()
    {
        gameOverPanel.SetActive(false);
    }
}
