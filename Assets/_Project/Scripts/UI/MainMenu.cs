using System;
using _Project.Scripts.Auth;
using _Project.Scripts.PlayfabServices;
using _Project.Scripts.SOAP.Events;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private StringEvent saveNameEvent;
        private PlayFabLogin playFabLogin;
        private IUserInitializer userInitializer;
        private IDisplayNameHandler displayNameHandler;
        private IUserDataService  userDataService;
        private ITitleDataService titleDataService;
        private ICurrencyService currencyService;

        private void Awake()
        {
            titleDataService = new PlayFabTitleDataService();
            currencyService = new PlayFabCurrencyService();
            userDataService = new PlayFabUserDataService();
            userInitializer = new UserInitializer(titleDataService, currencyService, userDataService);
            displayNameHandler = new DisplayNameHandler(saveNameEvent, titleDataService);
            playFabLogin = new PlayFabLogin(userInitializer, displayNameHandler);
            playFabLogin.TryLogin("Test9");
        }

        private void Update()
        {
            if (Keyboard.current.fKey.wasPressedThisFrame)
            {
                currencyService.AddCurrency("SC", 80, result => {});
            }
        }
    }
}