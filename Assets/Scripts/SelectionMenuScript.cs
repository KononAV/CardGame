using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectionMenuScript : MonoBehaviour
{
    [SerializeField]private Slider slider;

    [SerializeField] private MaterialScript materialSelection;
    [SerializeField] private SelectionMenuCard card;
    private SelectionMenuCard _card;
    private int mode;

    private bool isSwipe;
    //private bool isJocker;

    private void Awake()
    {
        _card = Instantiate(card,new Vector3(4,4.4f,-5.5f), card.transform.rotation);

        
    }

    private void Start()
    {
        Basic gameMode = SaveManager.Instance.gameMode;
        if (gameMode != null)
        {
            isSwipe = gameMode.isSwipe;

            slider.value = gameMode.SelectedCards;
        }
        
    }

    public void SelectedStyle(string imageSource) { 

        _card.GetComponent<Renderer>().material.SetTexture("_BaseMap", materialSelection.InintFirstCard(imageSource));
        //card.GetComponent<Renderer>().material.SetTexture("_BaseMap", materialSelection.InintFirstCard(imageSource));

        Resources.Load<Material>("Materials/MateralsForLevel/DemoMaterials/SpriteMaterial").SetTexture("_BaseMap", Resources.Load<Texture2D>(imageSource));

    }
        
    public void CardsCountSlider()
    {
        //isJocker = slider.value%2!=0? true : false;
        Debug.Log(slider.value);
        
    }

    public void ModeSelect(string mode)
    {
        this.mode = int.Parse(mode); 
        Debug.Log((Modes)this.mode);
    }

    public void OnIsSwipe()=> isSwipe = !isSwipe;





    public void StartButton()
    {
        
        SaveManager.Instance.gameMode = GameMode.GameModeSelector(mode);

       
        SaveManager.Instance.gameMode.isSwipe = isSwipe;
        SaveManager.Instance.saveMaterial = materialSelection.InitFolder();
        
        SaveManager.Instance.gameMode.SelectedCards = (int)slider.value;

        SceneManager.LoadScene("SampleScene");

    }

}
