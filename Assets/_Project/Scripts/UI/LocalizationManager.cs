using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace _Project.Scripts.UI
{
    public class LocalizationManager : MonoBehaviour
    {
        public static LocalizationManager Instance { get; private set; }

        private const string PlayerPrefsKey = "SelectedLanguage";

        [Header("Locale identifiers")]
        [Tooltip("Código del inglés en Localization Settings > Available Locales")]
        [SerializeField] private string englishLocale = "en";

        [Tooltip("Código del español en Localization Settings > Available Locales")]
        [SerializeField] private string spanishLocale = "es";

        [Tooltip("Idioma usado la primera vez que se abre el juego")]
        [SerializeField] private string defaultLocale = "en";

        public string CurrentLocaleCode { get; private set; }
        public string EnglishLocale => englishLocale;
        public string SpanishLocale => spanishLocale;

        public event Action<string> OnLanguageChanged;
        /// <summary>
        /// Se dispara una vez que LocalizationSettings ha terminado de cargar
        /// y el idioma inicial se ha aplicado. Úsalo en la UI para saber cuándo
        /// es seguro leer o cambiar el idioma.
        /// </summary>
        public event Action OnInitialized;

        private bool _isInitialized;
        /// <summary>
        /// Idioma que se pidió aplicar antes de que LocalizationSettings estuviera listo.
        /// Se aplica automáticamente al terminar Initialize().
        /// </summary>
        private string _pendingLocale;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Valor por defecto inmediato para que ningún lector obtenga null
            // antes de que termine la inicialización asíncrona.
            CurrentLocaleCode = defaultLocale;
        }

        private async void Start()
        {
            try
            {
                await Initialize();
            }
            catch (Exception ex)
            {
                Debug.LogError($"[LocalizationManager] Error inicializando la localización: {ex}");
            }
        }

        public async Task Initialize()
        {
            if (_isInitialized) return;

            // Esperar a que el paquete Localization termine de cargar tablas y locales
            await LocalizationSettings.InitializationOperation.Task;

            string saved = PlayerPrefs.GetString(PlayerPrefsKey, null);

            if (string.IsNullOrEmpty(saved))
            {
                // Primera vez: inglés por defecto y se guarda
                SetLanguage(defaultLocale, save: true);
            }
            else
            {
                // Restaurar idioma guardado (sin volver a guardar)
                SetLanguage(saved, save: false);
            }

            _isInitialized = true;

            // Aplicar cualquier SetLanguage que llegó antes de que estuviera listo
            if (!string.IsNullOrEmpty(_pendingLocale))
            {
                var pending = _pendingLocale;
                _pendingLocale = null;
                SetLanguage(pending, save: false);
            }

            OnInitialized?.Invoke();
        }

        public void SetLanguage(string localeCode, bool save = true)
        {
            if (!_isInitialized)
            {
                // En lugar de lanzar warning y salir, guardamos la petición para
                // aplicarla automáticamente tras Initialize(). Así la UI no se rompe
                // si llama a este método antes de que termine la carga asíncrona.
                _pendingLocale = localeCode;
                Debug.Log($"[LocalizationManager] Localization not initialized yet. Idioma '{localeCode}' en cola, se aplicará al terminar la inicialización.");
                return;
            }

            var locale = LocalizationSettings.AvailableLocales.GetLocale(localeCode);
            if (locale == null)
            {
                Debug.LogWarning($"[LocalizationManager] Locale '{localeCode}' not found in Available Locales.");
                return;
            }

            LocalizationSettings.SelectedLocale = locale;
            CurrentLocaleCode = localeCode;

            if (save)
                PlayerPrefs.SetString(PlayerPrefsKey, localeCode);

            OnLanguageChanged?.Invoke(localeCode);
        }
    }
}
