using UnityEngine;

namespace _Project.Scripts.SOAP.Events
{
    [CreateAssetMenu(fileName = "StringEvent", menuName = "ScriptableObjects/SOAP/GameEvent/StringEvent")]
    public class StringEvent : GameEvent<string>
    {
    }
}