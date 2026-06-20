using UnityEngine;

namespace _Project.Scripts.SOAP.Events
{
    [CreateAssetMenu(fileName = "VoidEvent", menuName = "ScriptableObjects/SOAP/GameEvent/VoidEvent")]
    public class VoidEvent : GameEvent<Unit>
    {
        public void Raise() => Raise(Unit.Default);
    }

    public struct Unit
    {
        public static Unit Default => default;
    }
}