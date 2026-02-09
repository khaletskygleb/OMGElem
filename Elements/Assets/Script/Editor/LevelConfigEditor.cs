using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelConfig))]
public class LevelConfigEditor : Editor
{
    private SerializedProperty widthProp;
    private SerializedProperty heightProp;
    private SerializedProperty cellsProp;
    private SerializedProperty databaseProp;

    private const int EMPTY = -1;
    private int brushIndex = EMPTY;

    void OnEnable()
    {
        widthProp = serializedObject.FindProperty("_width");
        heightProp = serializedObject.FindProperty("_height");
        cellsProp = serializedObject.FindProperty("_cells");
        databaseProp = serializedObject.FindProperty("_pieceDatabase");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Board Size", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(widthProp);
        EditorGUILayout.PropertyField(heightProp);

        int width = widthProp.intValue;
        int height = heightProp.intValue;

        // синхронизация массива
        if (cellsProp.arraySize != width * height)
        {
            cellsProp.arraySize = width * height;
            for (int i = 0; i < cellsProp.arraySize; i++)
                cellsProp.GetArrayElementAtIndex(i).intValue = EMPTY;
        }

        EditorGUILayout.Space(10);
        EditorGUILayout.PropertyField(databaseProp);

        if (databaseProp.objectReferenceValue == null)
        {
            EditorGUILayout.HelpBox("Assign ElementsDatabase", MessageType.Warning);
            serializedObject.ApplyModifiedProperties();
            return;
        }

        ElementsDatabase db = (ElementsDatabase)databaseProp.objectReferenceValue;
        DrawBrushSelector(db);
        EditorGUILayout.Space(10);
        DrawGrid(db, width, height);

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawBrushSelector(ElementsDatabase db)
    {
        EditorGUILayout.LabelField("Brush", EditorStyles.boldLabel);

        string[] options = new string[db.Elements.Length + 1];
        options[0] = "Empty";

        for (int i = 0; i < db.Elements.Length; i++)
            options[i + 1] = db.Elements[i].ElementName;

        brushIndex = EditorGUILayout.Popup("Paint", brushIndex + 1, options) - 1;
    }

    private void DrawGrid(ElementsDatabase db, int width, int height)
    {
        for (int y = height - 1; y >= 0; y--)
        {
            EditorGUILayout.BeginHorizontal();

            for (int x = 0; x < width; x++)
            {
                int index = y * width + x;
                SerializedProperty cell = cellsProp.GetArrayElementAtIndex(index);

                GUI.backgroundColor = GetColor(cell.intValue, db);

                if (GUILayout.Button("", GUILayout.Width(28), GUILayout.Height(28)))
                    PaintCell(cell, db);
            }

            EditorGUILayout.EndHorizontal();
        }

        GUI.backgroundColor = Color.white;
    }

    private void PaintCell(SerializedProperty cell, ElementsDatabase db)
    {
        if (brushIndex == EMPTY)
            cell.intValue = EMPTY;
        else if (brushIndex >= 0 && brushIndex < db.Elements.Length)
            cell.intValue = db.Elements[brushIndex].Index;
    }

    private Color GetColor(int value, ElementsDatabase db)
    {
        if (value == EMPTY) return Color.gray;

        var def = db.GetByIndex(value);
        return def != null ? def.EditorColor : Color.magenta;
    }
}
