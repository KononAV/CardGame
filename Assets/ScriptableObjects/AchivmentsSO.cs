using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[CreateAssetMenu(fileName = "AchivmentsSO", menuName = "Scriptable Objects/AchivmentsSO")]
public class AchivmentsSO : ScriptableObject
{


    public static Dictionary<string, Action<object>> gameEvents;

    private static GameObject archivePref;
    private static Vector3 InstancePos;

    private static GameObject canvas;

    private static MonoBehaviour mono;
    private static DG.Tweening.Sequence animation;



    public static void InitAllAchivments()
    {
        archivePref = Resources.Load<GameObject>("Prefabs/Archive");
        InstancePos = InitArchivePos(SaveManager.Instance.safeArea);
        gameEvents = new Dictionary<string, Action<object>>
        {
            {"FirstStep",
                x=>{
                    Debug.Log("FirstStep");

                    DeleteFromDict("FirstStep");
                    GameEvents.TextureEvent("png/BaseCards/blue");

                    mono.StartCoroutine(UIArchive(name:"First Step", description:"enter the game", icon: null));

                } },
            {"FirstPlay",
                x=>{
                    Debug.Log("FirstPlay");

                    DeleteFromDict("FirstPlay");
                    GameEvents.TextureEvent("png/StarAnimals/croco");
                    mono.StartCoroutine(UIArchive(name:"First Game", description:"complete your first level", icon: null));

                } },

            {"FirstTenPairs",
                x=>{

                    int.TryParse(x.ToString(), out int a);
                    if(a<10)return;
                    DeleteFromDict("FirstTenPairs");
                    GameEvents.TextureEvent("png/PixelArt/kiwi");

                    mono.StartCoroutine(UIArchive(name:"First ten pairs", description:"do your first ten pairs", icon: null));


                } },


        };

        ResultDict();
    }

    public static void SetMono(MonoBehaviour monoBeh) => mono = monoBeh;

    private static Vector3 InitArchivePos(Rect safeArea)
    {
        float x = safeArea.x + safeArea.width * 0.5f;
        float y = safeArea.y + safeArea.height * 0.9f;

        return new Vector2(x, y);
    }

    private static void ResultDict()
    {
        string[] keys = SaveManager.Instance.saveProgressInstance.savedArchives.ToArray();
        foreach (var key in keys)
        {
            gameEvents.Remove(key);

        }
    }
    private static void DeleteFromDict(string key)
    {

        gameEvents.Remove(key);
        SaveManager.Instance.saveProgressInstance.savedArchives.Add(key);
    }

    public static IEnumerator UIArchive(string name = null, string description = null, Sprite icon = null)
    {
        canvas = GameObject.Find("Canvas").gameObject;

        var newArchive = Instantiate(archivePref, new Vector3(InstancePos.x, InstancePos.y+100, InstancePos.z), archivePref.transform.rotation);
        DontDestroyOnLoad(newArchive);

        newArchive.transform.SetParent(canvas.transform, true);
        newArchive.transform.localScale = Vector3.one;

        var Name = newArchive.transform.Find("ArchiveName")?.GetComponent<TextMeshProUGUI>();
        Name.text = name;

        var Description = newArchive.transform.Find("ArchiveDescription")?.GetComponent<TextMeshProUGUI>();
        Description.text = description;

        var Image = newArchive.transform.Find("ArchiveIcon")?.GetComponent<Image>();
        Image.sprite = icon;

        newArchive.TryGetComponent<CanvasGroup>(out var canvasGroup);
        canvasGroup.alpha = 0;
        
        
        if(newArchive!=null)
        ShowAnim(newArchive.transform, canvasGroup);

        yield return null;
        //mono.StartCoroutine(DestroyArchive(newArchive.GetComponent<CanvasGroup>()));
    }



    private static void ShowAnim(in Transform transform,CanvasGroup canvasGroup)
    {
        animation = DOTween.Sequence();
        animation.Append(transform?.DOMoveY(InstancePos.y, 2f).SetEase(Ease.Linear)).Join(canvasGroup?.DOFade(1, 5f)).OnComplete(()=> DestroyArchive(canvasGroup));
        //animation.Play();
        
    }

 

    private static void DestroyArchive(CanvasGroup canvasGroup)
    {
        animation = DOTween.Sequence();

        animation.Append(canvasGroup.DOFade(0, 5f)).OnComplete(()=>Destroy(canvasGroup.gameObject));
    }
    /*private static IEnumerator DestroyArchive(CanvasGroup canvasGroup)
    {
      
        while (canvasGroup != null&&canvasGroup.alpha > 0.1f)
        {
            
                canvasGroup.alpha -= 0.05f;
                yield return new WaitForSeconds(0.1f);
            
        }
        if (canvasGroup != null)

        Destroy(canvasGroup.gameObject);

    }*/
}



public static class GameEvents
{
   public static void TextureEvent(string path)
   {
        
        SaveManager.Instance.saveProgressInstance.AddTexture(path);
   }

}


