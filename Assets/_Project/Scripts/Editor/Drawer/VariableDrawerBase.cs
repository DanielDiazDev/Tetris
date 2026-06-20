using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace _Project.Scripts.SOAP.Drawer
{
    
    public abstract class VariableDrawerBase<T> : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();
            var objectField = new ObjectField(property.displayName)
            {
                objectType = typeof(T)
            };
            objectField.BindProperty(property);

            var valueLabel = new Label();
            valueLabel.style.paddingLeft = 20;
            
            container.Add(objectField);
            container.Add(valueLabel);
            
            objectField.RegisterValueChangedCallback(evt =>
            {
                
                if (evt.newValue is T variable)
                {
                    valueLabel.text = FormatValue(variable);
                   // variable.OnValueChanged += newValue => valueLabel.text = FormatValue(newValue);
                }
                else
                {
                    valueLabel.text = string.Empty;
                }
            } );
            
            if (property.objectReferenceValue is T currentVariable)
            {
                valueLabel.text = FormatValue(currentVariable);
              //  currentVariable.OnValueChanged += newValue => valueLabel.text = FormatValue(newValue);
            }
            
            return container;


        }
        protected abstract string FormatValue(T value);
    }
}