using _Project.Scripts.SOAP.Events;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CurrencyActivationUI : MonoBehaviour
{
    [SerializeField] private GameOverResultEvent gameOverResultEvent;
    [SerializeField] private GameObject currencyGO;
    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        gameOverResultEvent.Subscribe(ActivateView);
    }
    private void ActivateView(GameOverData _)
    {
        currencyGO.SetActive(true);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        gameOverResultEvent.Unsubscribe(ActivateView);
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == "MainMenu")
        {
            currencyGO.SetActive(true);
        }
        if (scene.name == "Game")
        {
            currencyGO.SetActive(false);
        }
    }
}