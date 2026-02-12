using UnityEngine;
using System.IO;

namespace ElementGame.Save
{
    public class SaveSystem
    {
        private string SavePath => Application.persistentDataPath + "/save.json";

        public void SaveGame(int levelIndex, Core.Board board)
        {
            SaveData data = new SaveData();
            data.levelIndex = levelIndex;
            data.width = board.Width;
            data.height = board.Height;
            data.cells = new int[data.width * data.height];

            for (int x = 0; x < data.width; x++)
            {
                for (int y = 0; y < data.height; y++)
                {
                    int index = y * data.width + x;
                    Core.Cell cell = board.GetCell(new Vector2Int(x, y));

                    data.cells[index] = cell.IsEmpty ? -1 : cell.Element.TypeIndex;
                }
            }

            string json = JsonUtility.ToJson(data);
            File.WriteAllText(SavePath, json);
        }

        public bool HasSave() => File.Exists(SavePath);

        public SaveData LoadGame()
        {
            if (!HasSave()) return null;

            string json = File.ReadAllText(SavePath);
            return JsonUtility.FromJson<SaveData>(json);
        }

        public void ClearSave()
        {
            if (HasSave()) File.Delete(SavePath);
        }
    }
}