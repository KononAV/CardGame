using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectionMenuScript : MonoBehaviour
{
    [SerializeField]private Slider slider;

    [SerializeField] private MaterialScript materialSelection;
    [SerializeField] private SelectionMenuCard card;
    private SelectionMenuCard _card;

    private void Start()
    {
        _card = Instantiate(card,new Vector3(4,4.4f,-5.5f), card.transform.rotation);
    }


    public void SelectedStyle(string imageSource)=>_card.GetComponent<Renderer>().material.SetTexture("_BaseMap", materialSelection.InintFirstCard(imageSource));
        
    public void CardsCountSlider()
    {
        Debug.Log(slider.value);
        SaveManager.Instance.gameMode.CardsInGame = (int)slider.value;
    }

    public void ModeSelect(string mode)
    {
        SaveManager.Instance.gameMode = GameMode.GameModeSelector(int.Parse(mode));
        Debug.Log(SaveManager.Instance.gameMode.ToString());
    }


    public void StartButton()
    {
        SaveManager.Instance.saveMaterial = materialSelection.InitFolder();
        SceneManager.LoadScene("SampleScene");
    }

}
