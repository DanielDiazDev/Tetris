using System;
using _Project.Scripts.Auth;
using _Project.Scripts.PlayfabServices;
using _Project.Scripts.SOAP.Events;
using _Project.Scripts.Sound;
using UnityEngine;
using UnityEngine.InputSystem;

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
        private void Awake()
        {
            var currencyService = ServiceLocator.Instance.GetService<ICurrencyService>();
            titleDataService = new PlayFabTitleDataService();
            userDataService = new PlayFabUserDataService();
            userInitializer = new UserInitializer(titleDataService,currencyService , userDataService);
            displayNameHandler = new DisplayNameHandler(saveNameEvent, titleDataService);
            playFabLogin = new PlayFabLogin(userInitializer, displayNameHandler);
           
            playFabLogin.onLoginComplete += OnLoginComplete;
            playFabLogin.TryLogin(SystemInfo.deviceUniqueIdentifier);
            ServiceLocator.Instance.GetService<ISoundManager>().PlayMusic(MusicType.MainMenuMusic);
        }

        private void OnLoginComplete()
        {
            var currencyService = ServiceLocator.Instance.GetService<ICurrencyService>();
            currencyService.CurrencyModel.Init();
            
        }

        private void Update()
        {
            if (Keyboard.current.fKey.wasPressedThisFrame)
            {
                ServiceLocator.Instance.GetService<ICurrencyService>().AddCurrency("SC", 80, result => {});
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