using _Project.Scripts.Prefs;
using _Project.Scripts.Sound;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityServiceLocator;

namespace _Project.Scripts.UI.Toggle
{
    /// <summary>Compact square-handle on/off control used by the Options panel.</summary>
    public sealed class OptionsSquareToggle : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private bool controlsMusic;

        private const string MusicKey = "MusicToggle";
        private const string SfxKey = "SfxToggle";
        private static readonly Color OnColor = Color.green;
        private static readonly Color OffColor = Color.red;
        private static readonly Color HandleColor = new Color(0.94f, 0.96f, 0.99f, 1f);

        [SerializeField] private RectTransform handle;
        [SerializeField]private Image trackImage;
        private bool isOn;

        private void Awake()
        {
            string key = controlsMusic ? MusicKey : SfxKey;
            isOn = PlayerPrefsManager.GetBool(key, true);
            RefreshVisuals();
            ApplyState(isOn);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            isOn = !isOn;
            PlayerPrefsManager.SetBool(controlsMusic ? MusicKey : SfxKey, isOn);
            RefreshVisuals();
            ApplyState(isOn);
        }

        private void ApplyState(bool value)
        {
            var soundManager = ServiceLocator.Global.Get<ISoundManager>();
            if (controlsMusic)
                soundManager.ToggleMusic(value);
            else
                soundManager.ToggleSfx(value);
        }
        private void RefreshVisuals()
        {
            handle.gameObject.GetComponent<Image>().color = isOn ? OnColor : OffColor;
            handle.anchoredPosition = new Vector2(isOn ? 28f : 4f, 0f);
        }
    }
}
