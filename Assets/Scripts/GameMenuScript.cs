using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameMenuScript:MonoBehaviour
{
    [SerializeField] private GameObject MenuDesc;
    [SerializeField] private RectTransform BlureDesc;

    [SerializeField] private float scores;
    [SerializeField] private float pairs;
    [SerializeField] private float total;

    private Sequence _blureAnimation;

    private void Start()
    {
        if (_blureAnimation == null) {BlureAnim(in BlureDesc); }
        
        
    }




    public void BlureAnim(in RectTransform transform)
    { 
        _blureAnimation = DOTween.Sequence();
        var massiveConreners = new Vector3[4];
        transform.GetWorldCorners(massiveConreners);
        _blureAnimation.Append(transform.DOMove(massiveConreners[2], 50f).SetEase(Ease.Linear)).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
    }
    public void PauseButton() { 
        
        MenuDesc.SetActive(true);
        Time.timeScale = 0f;
        _blureAnimation?.Play();


    }

    public void ContinueButton(string name)
    {
             
        GameObject.Find(name).SetActive(false);
        Time.timeScale = 1f;
    }

    public void OptionsButton() => Debug.LogWarning("NotEnplemetedSectionOptions");

    public void BackToMenuButton() {
        Time.timeScale = 1f;
        DOTween.KillAll();

        GameManagerScript.Instance.StopAllCoroutines();

        GameManagerScript.Instance.MoveToDec(PoolManager.Instance.cardsList.ToArray(), PoolManager.Instance.cardsList.Count - PoolManager.Instance.cardsPool.CountInactive);


        //PoolManager.Instance.ReleaseAllCards();
        SceneTransition.SwithToScene("SelectionMenu");
    }

    public void Restart(string name) {
        
        ContinueButton(name);
        GameManagerScript.Instance.GameModeRestart();
       
        
    
    }
}
