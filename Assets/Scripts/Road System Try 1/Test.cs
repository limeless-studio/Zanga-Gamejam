using UnityEngine;
using UnityEditor;

public class Test : MonoBehaviour
{
    public bool boolField;
    public int intField;
}


[CustomEditor(typeof(Test))]
public class TestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Test script = ((Test)target);
        
        EditorGUILayout.BeginHorizontal();
        
        EditorGUILayout.LabelField("Test");
        script.intField = EditorGUILayout.IntField(script.intField);
        script.boolField = EditorGUILayout.Toggle(script.boolField);
        
        EditorGUILayout.EndHorizontal();
        
        //base.OnInspectorGUI();
    }
}
