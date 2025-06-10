using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuScript:MonoBehaviour
{
    [SerializeField] private GameObject MenuDesc;

    [SerializeField] private float scores;
    [SerializeField] private float pairs;
    [SerializeField] private float total;

    public void PauseButton() { 
        
        MenuDesc.SetActive(true);
        Time.timeScale = 0f;
    
    }

    public void ContinueButton()
    {
        MenuDesc.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OptionsButton() => Debug.LogWarning("NotEnplemetedSectionOptions");

    public void BackToMenuButton() {
        Time.timeScale = 1f;

        GameManagerScript.Instance.StopAllCoroutines();
        int activeCards = PoolManager.Instance.cardsList.Count - PoolManager.Instance.cardsPool.CountInactive;

        var poolCards = PoolManager.Instance.cardsList;
       for (int i = 0; i < activeCards; i++) { 

            poolCards[i].StartRotation(0f);
            poolCards[i].transform.position = Vector3.zero;
            PoolManager.Instance.ReleaseCard(poolCards[i]);
        }
        
        //PoolManager.Instance.ReleaseAllCards();
        SceneManager.LoadScene("SelectionMenu");
    }

    public void Restart(string name) {
        Time.timeScale = 1f;

        GameManagerScript.Instance.GameModeRestart();
        GameObject.Find(name).SetActive(false);
    
    }
}
