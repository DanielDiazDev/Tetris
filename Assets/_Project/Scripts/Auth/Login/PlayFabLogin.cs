using System;
using System.Collections.Generic;
using _Project.Scripts.SOAP.Events;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace _Project.Scripts.Auth
{
    public class PlayFabLogin : ILogin
    {
        public event Action onLoginComplete;
        private IUserInitializer userInitializer;
        private IDisplayNameHandler displayNameHandler;

        public PlayFabLogin(IUserInitializer userInitializer, IDisplayNameHandler displayNameHandler)
        {
            this.userInitializer = userInitializer;
            this.displayNameHandler = displayNameHandler;
        }


        public void TryLogin(string customId)
        {
            if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
            {
                /*
            Please change the titleId below to your own titleId from PlayFab Game Manager.
            If you have already set the value in the Editor Extensions, this can be skipped.
            */
                PlayFabSettings.staticSettings.TitleId = "42";
            }
            var request = new LoginWithCustomIDRequest()
            {
                CustomId = customId, //SystemInfo.deviceUniqueIdentifier,
                CreateAccount = true
               
            };
            PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
        }

        private void OnLoginSuccess(LoginResult result)
        {
            PlayFabClientAPI.GetUserData(new GetUserDataRequest()
            {
                Keys = new List<string>(){"IsInitialized"}
            }, UserResultSuccess, UserResultFailure);
        }

        

        private void OnLoginFailure(PlayFabError error)
        {
            Debug.LogError(error.GenerateErrorReport());
        }
        private void UserResultSuccess(GetUserDataResult dataResult)
        {
            if (!dataResult.Data.ContainsKey("IsInitialized"))
            {
                //onLoginComplete?.Invoke();
                displayNameHandler.ActivateNamePanel(userInitializer.InitializeUser);
                //ActivateNamePanel(InitializeUser);
            }
        }
        private void UserResultFailure(PlayFabError error)
        {
            Debug.LogError(error.GenerateErrorReport());
        }
       

    }

   
}