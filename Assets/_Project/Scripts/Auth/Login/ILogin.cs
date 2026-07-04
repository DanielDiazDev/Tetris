using System;

namespace _Project.Scripts.Auth
{
    public interface ILogin
    {
        event Action onLoginComplete;
    }
}