using System;
using System.Collections.Generic;

namespace _Project.Scripts.PlayFab.Interfaces
{
    public interface ITitleDataService
    {
        void GetTitleData<T>(List<string> keys, Action<T> onSuccess, Action<string> onError = null);

        void UpdateTitleDataDisplayName(string username, Action onSuccess = null,
            Action<string> onError = null);
    }
}