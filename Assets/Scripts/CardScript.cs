using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class CardScript : MonoBehaviour
{
    [SerializeField] private CardStats _stats;
    [SerializeField] private List<Material> _materials;
    private CardStats stats;

    private void Awake()
    {
        stats = ScriptableObject.CreateInstance<CardStats>();  

    }
    
    public CardStats ShowStats()=> stats;
    public List<Material> ShowMaterial() => _materials;
    public void ChangeMaterial(int id)
    {
        if(id<_materials.Count)
        gameObject.GetComponent<Renderer>().material = _materials[id];
        else
        {
            throw new System.Exception("Material count Error");
        }
    }

    private void OnMouseDown()
    {

        Debug.Log(stats.ShowId());
        if (!GameManagerScript.Instance.IsMatch(this))
        {
            Debug.Log("Game Over");
        }
        ;
    }

}
