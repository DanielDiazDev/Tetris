using UnityEngine;

namespace _Project.Scripts.Prefs
{
    public static class PlayerPrefsManager
    {
        public static void SetInt(string key, int value) => PlayerPrefs.SetInt(key, value);
        public static void SetFloat(string key, float value) => PlayerPrefs.SetFloat(key, value);
        public static void SetString(string key, string value) => PlayerPrefs.SetString(key, value);
        
        public static void SetBool(string key, bool value) => PlayerPrefs.SetInt(key, value ? 1 : 0);

        public static void SetObject<T>(string key, T value)
        {
            string json = JsonUtility.ToJson(value);
            PlayerPrefs.SetString(key, json);
        }
        public static int GetInt(string key, int defaultValue = 0) => PlayerPrefs.GetInt(key, defaultValue);
        public static float GetFloat(string key, float defaultValue = 0f) => PlayerPrefs.GetFloat(key, defaultValue);
        public static string GetString(string key, string defaultValue = "") => PlayerPrefs.GetString(key, defaultValue);
        public static bool GetBool(string key, bool defaultValue = false) => PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;

        public static T GetObject<T>(string key)
        {
            string json = PlayerPrefs.GetString(key, "");
            if (string.IsNullOrEmpty(json)) return default;
            return JsonUtility.FromJson<T>(json);
        }
        public static bool HasKey(string key) => PlayerPrefs.HasKey(key);
        public static void DeleteKey(string key) => PlayerPrefs.DeleteKey(key);
        public static void ClearAll() => PlayerPrefs.DeleteAll();
    }
}