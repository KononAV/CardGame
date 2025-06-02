using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class CardScript : MonoBehaviour, ICard
{
    [SerializeField] private CardStats _stats;
    private Material _material;
    public Material Material
    {
        get => _material; 
        set =>_material = value; 
    }
    private float duration = 0.2f;
    private float followSpeed = 2f;  
    private float followDelay = 0.02f;


    private CardStats stats;

    private void Awake()
    {
        stats = Instantiate(_stats);
        _material = stats.GetMaterial();

    }

    private void Start()
    {
        gameObject.GetComponent<Renderer>().material = _material;

        

    }

    public CardStats ShowStats()=> stats;
    public Material ShowMaterial() => _material;



    public void ChangeMaterial(int id,in Texture2D texture)
    {
        ShowStats().NewId(id);
        var renderer = GetComponent<Renderer>();
        MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
        renderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetTexture("_BaseMap", texture);
        renderer.SetPropertyBlock(propertyBlock);
        
        
    }



    /// <summary>
    /// for mouse only/ correct after all
    /// </summary>
    private void OnMouseDown()
    {
        if (GameManagerScript.Instance.isFool) return;
        StartRotation(-180);
        Debug.Log(stats.ShowId());
        if (!GameManagerScript.Instance.IsMatch(this))
        {
            Debug.Log("Game Over");
        }
        ;
    }
    /// <summary>
    /// for mouse only/ correct after all
    /// </summary>

     

    public IEnumerator MoveToFinalPos(float x)
    {
        while (true) 
        {
            if (x < transform.position.x)
            {
                transform.position = Vector3.Lerp(transform.position,new Vector3(x, transform.position.y, transform.position.z), followDelay*followSpeed);
            }
            yield return new WaitForSeconds(followDelay);
        }
    }

    private IEnumerator RotateRight(float targetZAngle)
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(0f, 180f, targetZAngle);

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRotation;
    }
    

    public void StartRotation(float targetZAngle)
    {
        StopAllCoroutines();
        StartCoroutine(RotateRight(targetZAngle));
    }

   
}
