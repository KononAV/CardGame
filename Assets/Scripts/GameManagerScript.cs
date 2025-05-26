using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] private CardScript card;

    [SerializeField] private GameObject startPosition;

    public GameMode gameMode;

    private Basic CurrentGameMode;

    [SerializeField]private Camera MainCamera;
    [SerializeField] private GameObject GameBoard;
    
    private List<CardScript> currentCardId = new();

    public static GameManagerScript Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(Instance);
            return;
        }

        Instance = this;    
        DontDestroyOnLoad(gameObject);
    }

     void Start()
    {
        
        gameMode = new();
        CurrentGameMode = gameMode.GameModeSelector();

        if (MainCamera)
        {
            Vector3 cameraCenter =  MainCamera.GetComponent<Camera>().transform.position;
            Debug.Log(cameraCenter.x+"Camera x");
            Vector2 cameraSize = new Vector2 (cameraCenter.x, cameraCenter.z);
            

            //InitCards(cameraSize);
        }

    }

    public void WholePoleInit()
    {
        CreateCards(TableGrid.SpiralMatrixCards(4, 4), card.ShowMaterial());

    }

    private void CreateCards(Vector3[] vectorArray, List<Material> materials)
    {
        Debug.Log(vectorArray.Length+ "vector");
        if (vectorArray.Length < 2) throw new ArgumentException("Cards count <2");

        for(int i = 0; i< vectorArray.Length/2; i++)
        {
            for (int j = 0; j < gameMode.gameMode.CardsToMatch; j++)
            {
                Debug.Log(i);
                CardScript newCard = Instantiate(card, vectorArray[i* gameMode.gameMode.CardsToMatch+j], card.transform.rotation);
                newCard.ChangeMaterial(i);
            }
            Thread.Sleep(50);

        }
    }


    public bool IsMatch(CardScript cardId)
    {
        //Debug.Log(cardId.ShowStats().ShowId());

        currentCardId.Add(cardId);
        cardId.GetComponent<BoxCollider>().enabled = false;
        if (currentCardId.Count == gameMode.gameMode.CardsToMatch) {
            if (currentCardId.All(x => x.ShowStats().ShowId() == cardId.ShowStats().ShowId()))
            {
                Debug.Log("Same Cards!"); 
                gameMode.gameMode.CardsInGame-= gameMode.gameMode.CardsToMatch;
               
                StartCoroutine(EnableCards());

            }
            else { Debug.Log("Didnt Match!"); gameMode.gameMode.mistakes--; }

            StartCoroutine(ResetCards());
                       
            
        }
        return gameMode.gameMode.IsContinueValid();
        
    }

    private IEnumerator ResetCards()
    {
        yield return new WaitForSeconds(1f);
        foreach (CardScript card in currentCardId) { card.GetComponent<BoxCollider>().enabled = true; }
        currentCardId.Clear();

    }

    private IEnumerator EnableCards()
    {
        yield return new WaitForSeconds(1f);
        foreach (CardScript card in currentCardId) { Destroy(card); }
        
        Debug.Log("Cards Destroyed");
        currentCardId.Clear();

    }

   
    private void InitCards(Vector2 cameraCenter)
    {
        Vector3 gameBoardVec = Vector3.one;
        if (GameBoard)
        {
            Vector3 boardVec = GameBoard.transform.position;
            gameBoardVec = new Vector3(boardVec.x, .1f, boardVec.z);

           
        }

        Rect safeArea = Screen.safeArea;
        Vector3 StartPos = new Vector3(safeArea.position.normalized.x, .1f, safeArea.position.normalized.y);

        

        for (int i = 0; i < CurrentGameMode.CardsInGame; i++)
        {
            for (int j = 0; j < CurrentGameMode.CardsToMatch; j++)
            {
                CardScript newCard = Instantiate(
                    card,
                   StartPos, 
                    card.transform.rotation);
                Debug.Log(newCard.transform.position);
                newCard.ChangeMaterial(i);
                Debug.Log(newCard.ShowStats().ShowId());
                //startPosition.transform.position = newCard.transform.position;   
            }
            


        }

    }


}
