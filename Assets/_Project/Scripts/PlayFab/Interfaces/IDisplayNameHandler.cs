using System;

namespace _Project.Scripts.PlayFab.Interfaces
{
    public interface IDisplayNameHandler
    {
        void ActivateNamePanel(Action onComplete);
        void Dispose();
    }
}