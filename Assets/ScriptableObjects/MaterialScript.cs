using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.WSA;
using System.IO;

[CreateAssetMenu(fileName = "MaterialScript", menuName = "Scriptable Objects/MaterialScript")]
public class MaterialScript : ScriptableObject
{
    public Texture2D[] images { get;private set; }

    public Texture2D firstImage { get; private set; }
    private string path;

    public Texture2D[] InitFolder()
    {
        if (path == "") path = "png/BaseCards/blue";

        Debug.Log(path + "patj");
        images = Resources.LoadAll<Texture2D>(SplitSource(path));
            foreach (var image in images)
            {

                Debug.Log(image.name);
            }
        
        return images;
    }

    public Texture2D InintFirstCard(string imageSourse)
    {
        path = imageSourse;
        firstImage = Resources.Load<Texture2D>(path);
        return Resources.Load<Texture2D>(path);
    }

    private string SplitSource(string image) => Path.GetDirectoryName(image);


}
