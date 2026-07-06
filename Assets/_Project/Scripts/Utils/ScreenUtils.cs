using UnityEngine;

namespace _Project.Scripts.Utils
{
    public class ScreenUtils
    {
        public static Vector3 ScreenToWorld(Camera cam, Vector3 screenPos)
        {
            screenPos.z = cam.nearClipPlane;
            return cam.ScreenToWorldPoint(screenPos);
        }
    }
}