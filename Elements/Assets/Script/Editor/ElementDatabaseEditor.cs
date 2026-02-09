using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ElementsDatabase))]
public class ElementsDatabaseEditor : Editor
{
    SerializedProperty piecesProp;

    void OnEnable()
    {
        piecesProp = serializedObject.FindProperty("_elements");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawDefaultInspector();

        if (GUILayout.Button("Auto Assign Indexes"))
        {
            for (int i = 0; i < piecesProp.arraySize; i++)
            {
                SerializedProperty elementProp = piecesProp.GetArrayElementAtIndex(i);
                if (elementProp.objectReferenceValue == null)
                    continue;

                SerializedObject elementSO = new SerializedObject(elementProp.objectReferenceValue);
                SerializedProperty indexProp = elementSO.FindProperty("_index"); 

                indexProp.intValue = i;
                elementSO.ApplyModifiedProperties();
            }
        }

        EditorGUILayout.Space(10);
        EditorGUILayout.HelpBox(
            "Indexes are auto-generated based on position in the array.\nDo not edit manually.",
            MessageType.Info);

        serializedObject.ApplyModifiedProperties();
    }
}
