using System;
using System.Collections.Generic;

namespace _Project.Scripts.PlayFab.Interfaces
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
}