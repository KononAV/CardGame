using UnityEngine;
using UnityEngine.SceneManagement;

public class FitstScreenScript : MonoBehaviour
{

    private void Start()
    {
        SaveManager.Instance.saveProgressInstance.LoadData();
        SaveManager.Instance.saveProgressInstance.Save(total:100);
    }
    public void StartFreePlay()

    {
        SaveManager.Instance.saveProgressInstance.ClearSave();                     /////////////////////////////////
                                                                                   /////////////////////////////////
        SaveManager.Instance.saveProgressInstance.AddTexture("png/PixelArt/kiwi"); /////////////////////////////////
        SaveManager.Instance.saveProgressInstance.AddTexture("png/BaseCards/blue");/////////////////////////////////

        SceneManager.LoadScene("SelectionMenu");
    }
}
