using System;
using _Project.Scripts.PlayFab.Interfaces;
using _Project.Scripts.SOAP.Events;
using _Project.Scripts.UINodeGraph.Runtime;

namespace _Project.Scripts.PlayFab
{
    public class DisplayNameHandler : IDisplayNameHandler, IDisposable
    {
        private Action onSaveName;
        private StringEvent saveNameEvent;
        private ITitleDataService titleDataService; 

        public DisplayNameHandler(StringEvent saveNameEvent, ITitleDataService titleDataService)
        {
            this.saveNameEvent = saveNameEvent;
            this.titleDataService = titleDataService;
            this.saveNameEvent.Subscribe(SaveName);
        }


        public void ActivateNamePanel(Action onComplete)
        {
            onSaveName = onComplete;
            UINavigationManager.Instance.NavigateTo("5855f12d-9182-46ac-9346-ea2ef2d6905e");
            
        }

        private void SaveName(string username)
        {
           titleDataService.UpdateTitleDataDisplayName(username, () =>
           {
               onSaveName?.Invoke();
           }); 
        }

        public void Dispose()
        {
            if (saveNameEvent != null)
            {
                saveNameEvent.Unsubscribe(SaveName);
                saveNameEvent = null;
            }
        }
    }
}
