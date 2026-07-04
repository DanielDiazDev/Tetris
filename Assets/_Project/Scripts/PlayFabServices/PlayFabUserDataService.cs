using System;
using System.Collections.Generic;
using _Project.Scripts.Auth.Configurations;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace _Project.Scripts.PlayfabServices
{
    public interface IUserDataService
    {
        void UpdateUserData<T>(Dictionary<string, T> dataMap,
            Action onSuccess = null,
            Action<string> onError = null);

        void GetUserData<T>(string key,
            Action<T> onSuccess = null,
            Action<string> onError = null);
    }

    public class PlayFabUserDataService : IUserDataService
    {
        public void UpdateUserData<T>(Dictionary<string, T> dataMap,
            Action onSuccess = null,
            Action<string> onError = null)
        {
            var serializedData = new Dictionary<string, string>();
            foreach (var kvp in dataMap)
            {
                serializedData[kvp.Key] = JsonUtility.ToJson(kvp.Value);
            }

            var request = new UpdateUserDataRequest { Data = serializedData };

            PlayFabClientAPI.UpdateUserData(request,
                result => onSuccess?.Invoke(), 
                error => onError?.Invoke(error.GenerateErrorReport()));
        }

        public void GetUserData<T>(string key,
            Action<T> onSuccess = null,
            Action<string> onError = null)
        {
            PlayFabClientAPI.GetUserData(new GetUserDataRequest(),
                result =>
                {
                    if (result.Data != null && result.Data.TryGetValue(key, out var record))
                    {
                        var deserialized = JsonUtility.FromJson<T>(record.Value);
                        onSuccess?.Invoke(deserialized);
                    }
                    else
                    {
                        onError?.Invoke($"Key {key} not found");
                    }
                },
                error => onError?.Invoke(error.GenerateErrorReport()));
        }
       
        
    }
    
}