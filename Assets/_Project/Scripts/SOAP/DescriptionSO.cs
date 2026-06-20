using UnityEngine;

namespace _Project.Scripts.SOAP
{
    public abstract class DescriptionSO : ScriptableObject
    {
        [SerializeField,TextArea(1, 50)]protected string description;
    }
}