using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        SaveManager.Instance.safeArea = Screen.safeArea;
        _card = Instantiate(card,MenuCardInit(FindAnyObjectByType<Camera>()), card.transform.rotation);
        
        InitStylesForCards();
        


    }

    private Vector3 MenuCardInit(Camera MainCamera)
    {
        float x = 0.8f; 
        float y = 0.7f;  
        float distanceFromCamera = 10f;

        Vector3 worldPos = MainCamera.ViewportToWorldPoint(new Vector3(x, y, distanceFromCamera));
        worldPos.z = -2f;

        return worldPos;
    }


    private void InitStylesForCards()
    {
        GameObject[] styles = GameObject.FindGameObjectsWithTag("StyleSelection");

        if (SaveManager.Instance.saveProgressInstance.savedTextures == null) return;
        HashSet<string> usedStyleNames = new HashSet<string>(
            SaveManager.Instance.saveProgressInstance.savedTextures
            .Select(x => Path.GetDirectoryName(x.Replace("png/", "")))
            .ToArray());


        foreach (GameObject style in styles)
        {
            if (style == null) continue;

            if (!usedStyleNames.Contains(style.name))
            {
                style.SetActive(false);
            }
        }


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


        SceneTransition.SwithToScene("SampleScene");

    }

}
