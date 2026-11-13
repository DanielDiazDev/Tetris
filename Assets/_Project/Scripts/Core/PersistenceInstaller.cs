using _Project.Scripts.Auth;
using _Project.Scripts.PlayfabServices;
using _Project.Scripts.Sound;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityServiceLocator;

namespace _Project.Scripts.Core
{
    public class PersistenceInstaller : MonoBehaviour
    {
        [SerializeField] private SoundManager soundManager;
        private ICurrencyService currencyService;
        private ILeaderboardService leaderboardService;

        private void Awake()
        {
            
            var serviceLocator = ServiceLocator.Global;
            
            serviceLocator.Register<ISoundManager>(soundManager);
            currencyService = new PlayFabCurrencyService();
            serviceLocator.Register(currencyService);
            leaderboardService = new PlayFabLeaderboardService();
            serviceLocator.Register(leaderboardService);
            SceneManager.LoadScene("MainMenu");
        }
        
        
    }
}