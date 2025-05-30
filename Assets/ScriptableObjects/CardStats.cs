using UnityEngine;

[CreateAssetMenu(fileName = "CardStats", menuName = "Scriptable CardStats")]
public class CardStats : ScriptableObject
{
    [SerializeField] private int _id;
    [SerializeField] private string _name;
    [SerializeField] private Material _material;


    public int ShowId()=>_id;
    public string ShowName() => _name;
    public Material GetMaterial() => _material;

    public void NewId(int id)=>_id=id;
    public void NewName(string name) => _name = name;

}
