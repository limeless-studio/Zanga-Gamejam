using System;
using Toolbox.Editor;
using UnityEditor;
using UnityEngine;

namespace Inventory
{
    [CustomEditor(typeof(BaseInventory), true)]
    public class InventoryInspector : ToolboxEditor
    {
        private static readonly GUILayoutOption[] InfoTextSize = { GUILayout.Width(100f) };
        private static readonly GUILayoutOption[] DeleteButtonSize = { GUILayout.Width(70f) };

        public override void DrawCustomInspector()
        {
            base.DrawCustomInspector();
            
            var pInventory = (BaseInventory)target;

            // Show the list of items in the inventory
            EditorGUILayout.LabelField("Items");
            EditorGUILayout.BeginVertical("box");
            if (pInventory.IsEmpty())
            {
                EditorGUILayout.LabelField("No items in inventory");
            }
            else
            {
                try
                {
                    foreach (var item in pInventory.GetInventoryItems())
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(item.Value.Name, InfoTextSize);

                        EditorGUILayout.LabelField(item.Value.Count.ToString(), InfoTextSize);
                        
                        if (GUILayout.Button("X-(ONE)", DeleteButtonSize))
                            pInventory.RemoveItem(item.Value, 1);
                        if (GUILayout.Button("X-(ALL)", DeleteButtonSize))
                            pInventory.RemoveItem(item.Value);

                        EditorGUILayout.EndHorizontal();
                    }
                }
                catch (Exception e)
                {
                    // ignored
                    Debug.Log($"Error: {e}");
                }
            }

            EditorGUILayout.EndVertical();


        }
    }
}