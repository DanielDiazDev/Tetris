using UnityEngine;

namespace _Project.Scripts.Editor.Attributes
{
    public class ShowIfAttribute : PropertyAttribute
    {
        public string conditionField;

        public ShowIfAttribute(string conditionField)
        {
            this.conditionField = conditionField;
        }
    }
    public class GuidAttribute : PropertyAttribute { }
}