using System.IO;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SaveProgress", menuName = "Scriptable Objects/SaveProgress")]
public class SaveProgress : ScriptableObject
{
    [SerializeField] public float pairs;
    [SerializeField] public float total;
    [SerializeField] public string[] savedTextures;

    private static string SavePath => Path.Combine(Application.persistentDataPath, "save.json");

    [System.Serializable]
    private class SaveData
    {
        public float pairs;
        public float total;
        public string[] savedTextures;
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
        savedTextures = data.savedTextures ?? new string[0];
    }

    public void Save(float pairs=0f, float total=0f)
    {
        this.pairs += pairs;
        this.total += total;
        

        string json = JsonUtility.ToJson(this);
        File.WriteAllText(SavePath, json);
    }

    public void AddTexture(string path)
    {
        if (savedTextures == null)
        {
            savedTextures = new string[] { path };
            return;
        }
        if (savedTextures.Contains(path)) return;
        string[] newTexturesArray = new string[savedTextures.Length + 1];
        savedTextures.CopyTo(newTexturesArray, 0);
        newTexturesArray[^1] = path;
        savedTextures = newTexturesArray;
        Save();
    }

    public void ClearSave()
    {
        this.pairs = 0f;
        this.total = 0f;
        this.savedTextures = new string[0];
        Save();
    }
}
