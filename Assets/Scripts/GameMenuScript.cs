using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuScript:MonoBehaviour
{
    [SerializeField] private GameObject MenuDesc;

    public void PauseButton() => MenuDesc.SetActive(true);

    public void ContinueButton() => MenuDesc.SetActive(false);

    public void OptionsButton() => Debug.LogWarning("NotEnplemetedSectionOptions");

    public void BackToMenuButton() => Debug.LogWarning("NotEnplemetedSectionBackMenu");

    public void Restart() => SceneManager.LoadScene("SampleScene");
}
