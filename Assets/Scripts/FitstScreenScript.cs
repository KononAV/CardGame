using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEditor.DeviceSimulation;
using System.Collections;
using TMPro;
using UnityEngine.UI;


public class FitstScreenScript : MonoBehaviour
{

    [SerializeField] GameObject _startText;
    [SerializeField] RawImage _startImage;
    //[SerializeField] SceneTra neTransition;
    private Sequence _startAnimation;


    private void Start()
    {
        StartImageInit();

        StartCoroutine(StartAnimation(_startText.transform, _startText.GetComponent<TextMeshProUGUI>()));
        SaveManager.Instance.saveProgressInstance.LoadData();SaveManager.Instance.saveProgressInstance.ClearSave(); 
        SaveManager.Instance.saveProgressInstance.Save(total:100);

        AchivmentsSO.InitAllAchivments();
        AchivmentsSO.gameEvents.TryGetValue("FirstStep", out Action<object> action);
        
        action?.Invoke(null);

        //AchivmentsSO.UIArchive();
    }

    private void StartImageInit()
    {
        RectTransform rt = _startImage.GetComponent<RectTransform>();
        rt.anchorMin =  rt.offsetMin = rt.offsetMax =  Vector2.zero;
        rt.anchorMax = Vector2.one;

    }


    private IEnumerator StartAnimation(Transform transform, TextMeshProUGUI text) {
        _startAnimation = DOTween.Sequence();
         _startAnimation.Append(transform.DOScale(0.7f, 5f)).Join(text.DOFade(0.1f, 5f)).SetLoops(-1,LoopType.Yoyo).Play();
        yield break; 
    }
   
    public void StartFreePlay()
    {
        StopAllCoroutines();
        DOTween.KillAll(true);



        //SceneManager.LoadScene("SelectionMenu");
    }

    private void Update()
    {
        if (Input.touchCount == 1)
        {
            SceneTransition.SwithToScene("SelectionMenu");
            StartFreePlay();
        }
    }




}
