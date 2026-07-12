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

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            ServiceLocator.Instance.RegisterService<ISoundManager>(soundManager);
            currencyService = new PlayFabCurrencyService();
            ServiceLocator.Instance.RegisterService(currencyService);
            SceneManager.LoadScene("MainMenu");
        }

        private void OnDestroy()
        {
            ServiceLocator.Instance.UnregisterService<ISoundManager>();
        }
    }
}