using System;
using _Project.Scripts.PlayfabServices;
using _Project.Scripts.SOAP.Events;
using _Project.Scripts.UINodeGraph.Runtime;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace _Project.Scripts.Auth
{
    public interface IDisplayNameHandler
    {
        void ActivateNamePanel(Action onComplete);
        void Dispose();
    }

    public class DisplayNameHandler : IDisplayNameHandler, IDisposable
    {
        private Action onSaveName;
        private StringEvent saveNameEvent;
        private ITitleDataService titleDataService; 

        public DisplayNameHandler(StringEvent saveNameEvent, ITitleDataService titleDataService)
        {
            this.saveNameEvent = saveNameEvent;
            this.titleDataService = titleDataService;
            this.saveNameEvent.Subscribe(SaveName);
        }


        public void ActivateNamePanel(Action onComplete)
        {
            onSaveName = onComplete;
            UINavigationManager.Instance.NavigateTo("0a2a516d-dd4a-4e50-8069-8e2ecc2ffa59");
            
        }

        private void SaveName(string username)
        {
           titleDataService.UpdateTitleDataDisplayName(username, () =>
           {
               onSaveName?.Invoke();
           }); 
            // var request = new UpdateUserTitleDisplayNameRequest()
            // {
            //     DisplayName = username,
            // };
            // //  PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), result => result.AccountInfo.Username);
            // //Ponerlo en su clase
            // PlayFabClientAPI.UpdateUserTitleDisplayName(request,
            //     r =>
            //     {
            //         onSaveName();
            //     },
            //     e => { Debug.LogError(e.GenerateErrorReport()); });
            
        }

        public void Dispose()
        {
            if (saveNameEvent != null)
            {
                saveNameEvent.Unsubscribe(SaveName);
                saveNameEvent = null;
            }
        }
    }
}