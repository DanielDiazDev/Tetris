using _Project.Scripts.PlayFab.Interfaces;
using UnityEngine;
using UnityServiceLocator;

namespace _Project.Scripts.UI
{
    public class DeleteAccountButton : MonoBehaviour
    {
        public void Delete()
        {
            var login = ServiceLocator.Global.Get<ILogin>();
            login.DeleteAccount(
                onSuccess: () => Debug.Log("Account deleted"),
                onError:   err => Debug.LogError(err)
            );
        }
    }
}