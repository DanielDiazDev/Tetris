using System;
using System.Collections.Generic;
using _Project.Scripts.Auth.Configurations;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace _Project.Scripts.PlayfabServices
{
    public interface ITitleDataService
    {
        void GetTitleData<T>(List<string> keys, Action<T> onSuccess, Action<string> onError = null);

        void UpdateTitleDataDisplayName(string username, Action onSuccess = null,
            Action<string> onError = null);
    }
    public class PlayFabTitleDataService  : ITitleDataService
    {
        public void GetTitleData<T>(List<string> keys, Action<T> onSuccess, Action<string> onError = null)
        {
            GetTitleDataRequest request = new GetTitleDataRequest()
            {
                Keys = keys, 
            };
            PlayFabClientAPI.GetTitleData(request, dataResult =>
            {
                foreach (var key in keys)
                {
                    if (dataResult.Data.TryGetValue(key, out var json))
                    {
                        var result =  JsonUtility.FromJson<T>(json);//Adapter
                        onSuccess(result);
                    }
                }
                
            }, error => onError?.Invoke(error.GenerateErrorReport()));
            
        }

        public void UpdateTitleDataDisplayName(string username, Action onSuccess = null,
            Action<string> onError = null)
        {
            var request = new UpdateUserTitleDisplayNameRequest()
            {
                DisplayName = username,
            };
            //  PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), result => result.AccountInfo.Username);
            //Ponerlo en su clase
            PlayFabClientAPI.UpdateUserTitleDisplayName(request,
                r =>
                {
                    onSuccess?.Invoke();
                    //onSaveName();
                },
                e => { Debug.LogError(e.GenerateErrorReport()); });
        }
    }
}