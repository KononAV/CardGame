using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SaveProgress", menuName = "Scriptable Objects/SaveProgress")]
public class SaveProgress : ScriptableObject
{
    [SerializeField] public float pairs;
    [SerializeField] public float total;
    [SerializeField] public HashSet<string> savedTextures;

    [SerializeField] public HashSet<string> savedArchives;

    private static string SavePath => Path.Combine(Application.persistentDataPath, "save.json");

    [System.Serializable]
    private class SaveData
    {
        public float pairs;
        public float total;
        public string[] savedTextures; 
        public string[] savedArchives;

    }

    public void LoadData()
    {
        if (!File.Exists(SavePath))
        {
            Debug.LogWarning("Save file not found: " + SavePath);
            return;
        }
        string jsonFromFile = File.ReadAllText(SavePath);
        SaveData loaded = JsonUtility.FromJson<SaveData>(jsonFromFile);
        CopyData(loaded);
    }

    private void CopyData(SaveData data)
    {
        pairs = data.pairs;
        total = data.total;
        savedTextures = (data.savedTextures ?? new string[0]).ToHashSet();
        savedArchives = (data.savedArchives ?? new string[0]).ToHashSet();
    }

    public void Save(float pairs = 0f, float total = 0f)
    {
        this.pairs += pairs;
        this.total += total;

        var data = new SaveData
        {
            pairs = this.pairs,
            total = this.total,
            savedTextures = this.savedTextures?.ToArray() ?? new string[0],
            savedArchives = this.savedArchives?.ToArray() ?? new string[0]
        };

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(SavePath, json);
    }

    public void AddTexture(string path)
    {
        if (savedTextures == null)
            savedTextures = new HashSet<string>();

        if (savedTextures.Add(path))
            Save();
    }

    public void ClearSave()
    {
        this.pairs = 0f;
        this.total = 0f;
        this.savedTextures = new HashSet<string>();
        this.savedArchives = new HashSet<string>();
        Save();
    }
}
