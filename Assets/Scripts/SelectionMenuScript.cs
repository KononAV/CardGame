using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectionMenuScript : MonoBehaviour
{
    [SerializeField]private Slider slider;

    [SerializeField] private MaterialScript materialSelection;
    [SerializeField] private SelectionMenuCard card;
    private SelectionMenuCard _card;
    private int mode;

    private bool isSwipe;

    private void Awake()
    {
        _card = Instantiate(card,new Vector3(4,4.4f,-5.5f), card.transform.rotation);
    }


    public void SelectedStyle(string imageSource)=>_card.GetComponent<Renderer>().material.SetTexture("_BaseMap", materialSelection.InintFirstCard(imageSource));
        
    public void CardsCountSlider()
    {
        Debug.Log(slider.value);
        
    }

    public void ModeSelect(string mode)
    {
        this.mode = int.Parse(mode); 
        Debug.Log((Modes)this.mode);
    }

    public void OnIsSwipe()
    {
        isSwipe = !isSwipe;
        Debug.Log(isSwipe);
    }



    public void StartButton()
    {
        SaveManager.Instance.gameMode = GameMode.GameModeSelector(mode);
        SaveManager.Instance.gameMode.isSwipe = isSwipe;
        SaveManager.Instance.saveMaterial = materialSelection.InitFolder();

        SaveManager.Instance.gameMode.SelectedCards = (int)slider.value;

        SceneManager.LoadScene("SampleScene");

    }

}
