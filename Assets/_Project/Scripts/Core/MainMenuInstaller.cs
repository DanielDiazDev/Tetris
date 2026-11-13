using System;
using _Project.Scripts.Auth;
using _Project.Scripts.PlayfabServices;
using _Project.Scripts.SOAP.Events;
using _Project.Scripts.Sound;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityServiceLocator;

namespace _Project.Scripts.Core
{
    public class MainMenuInstaller : MonoBehaviour
    {
        [SerializeField] private StringEvent saveNameEvent;
        private PlayFabLogin playFabLogin;
        private IUserInitializer userInitializer;
        private IDisplayNameHandler displayNameHandler;
        private IUserDataService  userDataService;
        private ITitleDataService titleDataService;
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
            
            
            titleDataService = new PlayFabTitleDataService();
            userDataService = new PlayFabUserDataService();
            userInitializer = new UserInitializer(titleDataService,currencyService , userDataService);
            displayNameHandler = new DisplayNameHandler(saveNameEvent, titleDataService);
            playFabLogin = new PlayFabLogin(userInitializer, displayNameHandler);
           
            playFabLogin.onLoginComplete += OnLoginComplete;
            playFabLogin.TryLogin(SystemInfo.deviceUniqueIdentifier);
            serviceLocator.Get<ISoundManager>().Init();
            serviceLocator.Get<ISoundManager>().PlayMusic(MusicType.MainMenuMusic);
        }

        private void OnLoginComplete()
        {
            var serviceLocator = ServiceLocator.Global;
            var currencyService = serviceLocator.Get<ICurrencyService>();
            currencyService.CurrencyModel.Init();
            
        }

        private void Update()
        {
            if (Keyboard.current.fKey.wasPressedThisFrame)
            {
                ServiceLocator.Global.Get<ICurrencyService>().AddCurrency("SC", 80, result => {});
            }
        }

        private void OnDestroy()
        {
            displayNameHandler.Dispose();
            playFabLogin.onLoginComplete -= OnLoginComplete;
        //    currencyService.Dispose();
            
        }
    }
}
