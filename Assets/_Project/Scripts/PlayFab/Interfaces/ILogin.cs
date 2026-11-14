using System;

namespace _Project.Scripts.PlayFab.Interfaces
{
    public interface ILogin
    {
        event Action onLoginComplete;
        void TryLogin(string deviceId);
        bool IsInitialized { get; }
    }
}