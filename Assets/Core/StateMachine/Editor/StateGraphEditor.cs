using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace Core.StateMachine {
    [CustomNodeGraphEditor(typeof(StateGraph))]
    public class StateGraphEditor : NodeGraphEditor {
        
        bool showBlackboard = false;
        
        public override void OnOpen()
        {
            window.titleContent.text = "Test";
            base.OnOpen();
        }

        public override void OnGUI()
        {
            base.OnGUI();
            
            // show toolbar with 3 sections (left, middle, right) with high z-index
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            // Show blackboard button
            if (GUILayout.Button("Blackboard", EditorStyles.toolbarButton)) showBlackboard = !showBlackboard;
            GUILayout.FlexibleSpace();
            GUILayout.Label("State Machine", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            GUILayout.Space(10);
            
            if (showBlackboard)
            {
                // Draw blackboard
                DrawBlackboard();
            }
            
        }
        
        void DrawBlackboard()
        {
            // Draggable blackboard
            GUILayout.BeginArea(new Rect(0, 20, 300, 350), EditorStyles.helpBox);
            // bg color
            EditorGUI.DrawRect(new Rect(0, 0, 300, 350), new Color(0.1f, 0.1f, 0.1f));
            // space
            GUILayout.Space(10);
            // Title
            GUILayout.Label("Blackboard", EditorStyles.boldLabel);
            // Draw Line
            EditorGUI.DrawRect(new Rect(0, 30, 300, 1), new Color(0.3f, 0.3f, 0.3f));
            // Draw blackboard fields
            GUILayout.BeginVertical();
            
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
    }
}