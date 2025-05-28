using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class CardScript : MonoBehaviour
{
    [SerializeField] private CardStats _stats;
    [SerializeField] private Material _material;

    private CardStats stats;

    private void Awake()
    {
        stats = ScriptableObject.CreateInstance<CardStats>();  

    }

    private void Start()
    {
        gameObject.GetComponent<Renderer>().material = _material;


    }

    public CardStats ShowStats()=> stats;
    public Material ShowMaterial() => _material;



    public void ChangeMaterial(int id,Texture2D texture)
    {
        ShowStats().NewId(id);
        var renderer = GetComponent<Renderer>();
        MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
        renderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetTexture("_BaseMap", texture);
        renderer.SetPropertyBlock(propertyBlock);
        
        
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
