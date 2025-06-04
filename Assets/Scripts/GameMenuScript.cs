using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuScript:MonoBehaviour
{
    [SerializeField] private GameObject MenuDesc;

    public void PauseButton() => MenuDesc.SetActive(true);

    public void ContinueButton() => MenuDesc.SetActive(false);

    public void OptionsButton() => Debug.LogWarning("NotEnplemetedSectionOptions");

    public void BackToMenuButton() {
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

    public void Restart() { 
        
        GameManagerScript.Instance.GameModeRestart();
        MenuDesc.SetActive(false);
    
    }
}
