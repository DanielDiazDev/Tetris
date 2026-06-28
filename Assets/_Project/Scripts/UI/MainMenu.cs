using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button startButton;

        private void Awake()
        {
            //startButton.onClick.AddListener(() => SceneManager.LoadScene("Game"));
        }
    }
}