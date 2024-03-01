using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Toolbox.Editor.Drawers
{
    public class RequiredAttributeDrawer: ToolboxSelfPropertyDrawer<RequiredAttribute>
    {
        protected override void OnGuiSafe(SerializedProperty property, GUIContent label, RequiredAttribute attribute)
        {
            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                if (property.objectReferenceValue == null)
                {
                    string errorMessage = property.name + " is required";
                    if (!string.IsNullOrEmpty(attribute.Message))
                    {
                        errorMessage = attribute.Message;
                    }
                    
                    EditorGUILayout.HelpBox(errorMessage, MessageType.Error);
                }
            }
            else
            {
                string warning = attribute.GetType().Name + " works only on reference types";
                EditorGUILayout.HelpBox(warning, MessageType.Warning);
            }
            
            EditorGUILayout.PropertyField(property, label);
        }


        public override bool IsPropertyValid(SerializedProperty property)
        {
            return property.propertyType == SerializedPropertyType.ObjectReference;
        }
    }
}