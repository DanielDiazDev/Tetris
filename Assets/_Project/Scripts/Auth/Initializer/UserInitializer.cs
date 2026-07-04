using System.Collections.Generic;
using _Project.Scripts.Auth.Configurations;
using _Project.Scripts.PlayfabServices;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace _Project.Scripts.Auth
{
    public interface IUserInitializer
    {
        void InitializeUser();
    }

    public class UserInitializer : IUserInitializer
    {
        private ITitleDataService titleDataService;
        private ICurrencyService currencyService;
        private IUserDataService userDataService;

        public UserInitializer(ITitleDataService titleDataService, ICurrencyService currencyService, IUserDataService userDataService)
        {
            this.titleDataService = titleDataService;
            this.currencyService = currencyService;
            this.userDataService = userDataService;
        }

        public void InitializeUser()
        {
           titleDataService.GetTitleData<InitialUserData>(new List<string>(){"InitialUserData"},
               data =>
               {
                    currencyService.AddCurrency("SC", data.InitialSoftCurrency,
                        resultSuccess => { });  
               });
           var dataToUpdate = new Dictionary<string, object>
           {
               { "TutorialMenu", new TutorialMenuConfiguration { IsCompleted = false } },
               { "TutorialGame", new TutorialGameConfiguration { IsCompleted = false } },
               { "IsInitialized", new UserInitialized { IsInitialized = true } }
           };
           userDataService.UpdateUserData(dataToUpdate, () => {}); //Animacion de que se actualizo 

        }
    }
}