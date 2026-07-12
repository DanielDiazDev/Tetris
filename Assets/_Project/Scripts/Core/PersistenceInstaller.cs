using _Project.Scripts.Auth;
using _Project.Scripts.PlayfabServices;
using _Project.Scripts.Sound;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Core
{
    public class PersistenceInstaller : MonoBehaviour
    {
        [SerializeField] private SoundManager soundManager;
        private ICurrencyService currencyService;
        private ILeaderboardService leaderboardService;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            ServiceLocator.Instance.RegisterService<ISoundManager>(soundManager);
            currencyService = new PlayFabCurrencyService();
            ServiceLocator.Instance.RegisterService(currencyService);
            leaderboardService = new PlayFabLeaderboardService();
            ServiceLocator.Instance.RegisterService(leaderboardService);
            SceneManager.LoadScene("MainMenu");
        }

        private void OnDestroy()
        {
            ServiceLocator.Instance.UnregisterService<ISoundManager>();
        }
    }
}