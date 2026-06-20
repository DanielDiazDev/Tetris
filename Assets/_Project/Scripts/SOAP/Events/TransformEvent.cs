using UnityEngine;

namespace _Project.Scripts.SOAP.Events
{
    [CreateAssetMenu(fileName = "TransformEvent", menuName = "ScriptableObjects/SOAP/GameEvent/TransformEvent")]
    public class TransformEvent : GameEvent<Transform>
    {
    }
}