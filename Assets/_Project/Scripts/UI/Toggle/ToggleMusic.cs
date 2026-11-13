using _Project.Scripts.Core;
using _Project.Scripts.Prefs;
using _Project.Scripts.Sound;
using UnityEngine;
using UnityServiceLocator;

namespace _Project.Scripts.UI.Toggle
{
    public class ToggleMusic : MonoBehaviour
    {
        public void Toggle(bool value)
        {
            ServiceLocator.Global.Get<ISoundManager>().ToggleMusic(value);
        }
    }
}