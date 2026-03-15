using System;
using System.IO;
using CardMatch.Data;
using UnityEngine;

namespace CardMatch.Managers
{
    public class SaveDataManager
    {
        private static string SavePath => Application.persistentDataPath + "/save.json";

        public SaveData SaveData { private set; get; }

        public SaveDataManager()
        {
            SaveData = Load() ?? new SaveData();
        }

        public void OnLevelLoaded(LevelData levelData)
        {
            SaveData.levelId = levelData.levelId;

            Save();
        }

        private void Save()
        {
            if (SaveData == null)
                return;

            string json = JsonUtility.ToJson(SaveData, true);
            File.WriteAllText(SavePath, json);
        }

        private SaveData Load()
        {
            if (!File.Exists(SavePath)) return null;

            string json = File.ReadAllText(SavePath);
            try
            {
                return JsonUtility.FromJson<SaveData>(json);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return null;
            }
        }
    }
}