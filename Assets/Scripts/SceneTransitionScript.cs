using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public TextMeshProUGUI LoadingPercent;
    public Image LoadingImage;

    private static SceneTransition Instance;
    private static bool shouldPlayOpeningAnimation = false;

    private Animator Anim;
    private AsyncOperation loadingSceneOperation;

    public static void SwithToScene(string SceneNum)
    {
        Instance.GetComponent<SceneTransition>().enabled = true;
        Instance.StartCoroutine(Instance.LoadingScene(SceneNum));
    }


    IEnumerator LoadingScene(string SceneNum)
    {
        Anim.SetTrigger("sceneStart");
        yield return new WaitForSeconds(0.6f); 
        loadingSceneOperation = SceneManager.LoadSceneAsync(SceneNum);
        loadingSceneOperation.allowSceneActivation = false;
        while (loadingSceneOperation.progress < 0.9f)
        {
            yield return null;
        }
        shouldPlayOpeningAnimation = true;
        loadingSceneOperation.allowSceneActivation = true;
    }

  
    void Start()
    {
        GetComponent<SceneTransition>().enabled = false;
        Instance = this;

        Anim = GetComponent<Animator>();

        if (shouldPlayOpeningAnimation)
            Anim.SetTrigger("sceneClose");
    }

    void Update()
    {
        if (loadingSceneOperation != null)
        {
            LoadingPercent.text = "Loading...\t" + Mathf.RoundToInt(loadingSceneOperation.progress * 100) + "%";
            LoadingImage.fillAmount = loadingSceneOperation.progress;
        }
    }

    
}