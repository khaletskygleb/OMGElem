using UnityEngine;
using UnityEditor;
using System.IO;

public static class SaveSystemEditor
{
    [MenuItem("ElementGame Tools/Clear Save Data")]
    public static void ClearSaveData()
    {
        string path = Application.persistentDataPath + "/save.json";

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log($"Save file deleted: {path}");
        }
        else
        {
            Debug.Log("No save file found.");
        }
    }
}
