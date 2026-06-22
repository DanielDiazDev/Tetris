using _Project.Scripts.SOAP.Events;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace _Project.Scripts.PlayFab
{
    public class PlayFabLogin : MonoBehaviour
    {
    
        [SerializeField] private GameObject namePanel;
        [SerializeField] private StringEvent saveNameEvent;

        private void OnEnable()
        {
            saveNameEvent.Subscribe(SaveName);
        }

        private void OnDestroy()
        {
            saveNameEvent.Unsubscribe(SaveName);
        }

        private void Start()
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
                CustomId = SystemInfo.deviceUniqueIdentifier,
                CreateAccount = true,
                InfoRequestParameters = new GetPlayerCombinedInfoRequestParams()
                {
                    GetPlayerProfile = true
                }
            };
            PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
        }

        private void OnLoginSuccess(LoginResult result)
        {
            string displayName = result.InfoResultPayload?.PlayerProfile?.DisplayName;

            if (string.IsNullOrEmpty(displayName))
            {
                namePanel.SetActive(true);
            }
            else
            {
                PlayerData.DisplayName = displayName;
                Debug.Log("Jugador logueado: " + result.InfoResultPayload.PlayerProfile.DisplayName);
            }
        }

        private void SaveName(string username)
        {
        
            var request = new UpdateUserTitleDisplayNameRequest()
            {
                DisplayName = username,
            };
            PlayFabClientAPI.UpdateUserTitleDisplayName(request,
                r =>
                {
                    PlayerData.DisplayName = r.DisplayName;
                },
                e => { Debug.LogError(e.GenerateErrorReport()); });
        }

        private void OnLoginFailure(PlayFabError error)
        {
            Debug.LogError(error.GenerateErrorReport());
        }
    }
}