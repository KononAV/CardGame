using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FitstScreenScript : MonoBehaviour
{

    private void Start()
    {
        
        SaveManager.Instance.saveProgressInstance.LoadData();SaveManager.Instance.saveProgressInstance.ClearSave(); 
        SaveManager.Instance.saveProgressInstance.Save(total:100);

        AchivmentsSO.InitAllAchivments();
        AchivmentsSO.gameEvents.TryGetValue("FirstStep", out Action<object> action);
        action?.Invoke(null);

        //AchivmentsSO.UIArchive();
    }
    public void StartFreePlay()
    {
                   
        SceneManager.LoadScene("SelectionMenu");
    }
}
