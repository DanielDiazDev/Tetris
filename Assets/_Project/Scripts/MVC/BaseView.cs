using System;
using UnityEngine;

namespace _Project.Scripts.MVC
{
    public abstract class BaseView : MonoBehaviour
    {
        #region Events
        public event Action OnInitialized;
        public event Action OnShow;
        public event Action OnHide;
        #endregion

        #region Unity
        private void Awake()
        {
            Initialize();
        }

        private void OnEnable() => ShowView();
        private void OnDisable() => HideView();
        #endregion

        protected virtual void Initialize() => OnInitialized?.Invoke();
        protected virtual void ShowView() => OnShow?.Invoke();
        protected virtual void HideView() => OnHide?.Invoke();
    }

    public abstract class GeneralView<TModel, TController> : BaseView
        where TModel : BaseModel
    {
        protected TController controller;
    }
}