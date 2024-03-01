using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Toolbox.Editor
{
    using Editor = UnityEditor.Editor;
    using Object = UnityEngine.Object;

    /// <summary>
    /// Base Editor class for all Toolbox-related features.
    /// </summary>
    [CustomEditor(typeof(Object), true, isFallback = true)]
    [CanEditMultipleObjects]
    public class ToolboxEditor : Editor, IToolboxEditor
    {
        private IEnumerable<MethodInfo> _methods = Enumerable.Empty<MethodInfo>();

        private void OnEnable()
        {
            _methods = ReflectionUtility.GetAllMethods( 
                target, m => m.GetCustomAttributes(typeof(ButtonAttribute), true).Length > 0);
        }

        /// <summary>
        /// Inspector GUI re-draw call.
        /// </summary>
        public sealed override void OnInspectorGUI()
        {
            ToolboxEditorHandler.HandleToolboxEditor(this);
            
            DrawButtons();
        }

        private void DrawButtons()
        {
            if (_methods.Any())
            {
                foreach (var method in _methods)
                {
                    var buttonAttribute = method.GetCustomAttribute<ButtonAttribute>();
                    if (buttonAttribute == null)
                    {
                        continue;
                    }

                    string buttonName = buttonAttribute.ExtraLabel;

                    if (GUILayout.Button(new GUIContent
                        {
                            image = null,
                            text = buttonName,
                            tooltip = buttonAttribute.Tooltip
                        }))
                    {
                        method.Invoke(target, null);
                    }
                }
            }
        }

        /// <inheritdoc />
        [Obsolete("To draw properties in a different way override the Drawer property.")]
        public virtual void DrawCustomProperty(SerializedProperty property)
        { }

        /// <inheritdoc />
        public virtual void DrawCustomInspector()
        {
            Drawer.DrawEditor(serializedObject);
        }

        /// <inheritdoc />
        public void IgnoreProperty(SerializedProperty property)
        {
            Drawer.IgnoreProperty(property);
        }

        /// <inheritdoc />
        public void IgnoreProperty(string propertyPath)
        {
            Drawer.IgnoreProperty(propertyPath);
        }

        Editor IToolboxEditor.ContextEditor => this;
        /// <inheritdoc />
        public virtual IToolboxEditorDrawer Drawer { get; } = new ToolboxEditorDrawer();

#pragma warning disable 0067
        [Obsolete("ToolboxEditorHandler.OnBeginToolboxEditor")]
        public static event Action<Editor> OnBeginToolboxEditor;
        [Obsolete("ToolboxEditorHandler.OnBreakToolboxEditor")]
        public static event Action<Editor> OnBreakToolboxEditor;
        [Obsolete("ToolboxEditorHandler.OnCloseToolboxEditor")]
        public static event Action<Editor> OnCloseToolboxEditor;
#pragma warning restore 0067
    }
}