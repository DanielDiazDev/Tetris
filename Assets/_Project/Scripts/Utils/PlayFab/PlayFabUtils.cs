using _Project.Scripts.Auth.Configurations;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace _Project.Scripts.Utils.PlayFab
{
    public static class PlayFabUtils
    {
        
    }
}

// PlayFabClientAPI.GetTitleData(request, dataResult =>
// {
//     var data = dataResult.Data["InitialUserData"];
//     var initialUserData = JsonUtility.FromJson<InitialUserData>(data);
//                 
//     PlayFabClientAPI.AddUserVirtualCurrency(new AddUserVirtualCurrencyRequest()
//         {
//             Amount = initialUserData.InitialSoftCurrency,
//             VirtualCurrency = "SC"
//         },
//         result =>{}, error => {});
//     Debug.Log(initialUserData.InitialSoftCurrency);
//     PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
//     {
//         Data = new Dictionary<string, string>()
//         {
//             {
//                 "TutorialMenu",
//                 JsonUtility.ToJson(new TutorialMenuConfiguration()
//                     { IsCompleted = initialUserData.TutorialMenuCompleted })
//             },
//             {
//                 "TutorialGame",
//                 JsonUtility.ToJson(new TutorialGameConfiguration()
//                     { IsCompleted = initialUserData.TutorialGameCompleted })
//             },
//             {
//                 "IsInitialized",
//                 JsonUtility.ToJson(new UserInitialized()
//                     { IsInitialized = true })
//             },
//         }
//     }, result =>
//     {
//                     
//     }, error => { });
// }, error => {} );