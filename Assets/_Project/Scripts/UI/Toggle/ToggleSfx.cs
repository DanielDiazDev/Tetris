using _Project.Scripts.Core;
using _Project.Scripts.Sound;
using UnityEngine;

namespace _Project.Scripts.UI.Toggle
{
    public class ToggleSfx : MonoBehaviour
    {
        public void Toggle(bool value)
        {
            ServiceLocator2.Instance.GetService<ISoundManager>().ToggleSfx(value);
        }
    }
}