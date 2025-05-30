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

    private Texture2D[] textures;

    private bool isJokerHere;


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
       // DontDestroyOnLoad(gameObject);
    }

     void Start()
    {

        textures = SaveManager.Instance.saveMaterial;
        isJokerHere = false;
        CurrentGameMode = SaveManager.Instance.gameMode;

        if (MainCamera)
        {
            Vector3 cameraCenter =  MainCamera.GetComponent<Camera>().transform.position;
            Debug.Log(cameraCenter.x+"Camera x");
            Vector2 cameraSize = new Vector2 (cameraCenter.x, cameraCenter.z);
            
            
            //InitCards(cameraSize);
        }
        Debug.Log(CurrentGameMode.CardsInGame);
        (int, int) sides = MatrixSidesAnalizer(CurrentGameMode.CardsInGame);
     
        CreateCards(TableGrid.SpiralMatrixCards(sides.Item1, sides.Item2));

    }

    private (int,int) MatrixSidesAnalizer(int square)
    {
        ( int x,int y) tuple = (0,0);
        if (square % 2 != 0)
        {
            isJokerHere = true;
            square -= 1;
        }
        for(int i = 4; i > 0; i--)
        {
            if (square % i == 0) { tuple.y = i; tuple.x = square / i; break; }
        }
        return tuple;   
    }

    public void WholePoleInit()
    {
        CreateCards(TableGrid.SpiralMatrixCards(4, 4));

    }

    private void CreateCards(Vector3[] vectorArray)
    {
        Debug.Log(vectorArray.Length+ "vector");
        if (vectorArray.Length < 2) throw new ArgumentException("Cards count <2");

        for(int i = 0; i< vectorArray.Length/2; i++)
        {
            int newId = (i + textures.Length) % textures.Length;
            for (int j = 0; j < CurrentGameMode.CardsToMatch; j++)
            {
                Debug.Log(newId);
                CardScript newCard = Instantiate(card, vectorArray[i* CurrentGameMode.CardsToMatch+j], card.transform.rotation);
                
                newCard.ChangeMaterial( newId,textures[newId]);
            }
            Thread.Sleep(50);

        }
    }


    public bool IsMatch(CardScript cardId)
    {
        //Debug.Log(cardId.ShowStats().ShowId());

        currentCardId.Add(cardId);
        cardId.GetComponent<BoxCollider>().enabled = false;
        if (currentCardId.Count == CurrentGameMode.CardsToMatch) {
            

            if (currentCardId.All(x => x.ShowStats().ShowId() == cardId.ShowStats().ShowId()))
            {
                Debug.Log("Same Cards!"); 
                CurrentGameMode.CardsInGame-= CurrentGameMode.CardsToMatch;
               
                StartCoroutine(EnableCards());

            }
            else { Debug.Log("Didnt Match!"); CurrentGameMode.mistakes--; }
            foreach (var card in currentCardId) { card.ShowStats().ShowId(); }
            StartCoroutine(ResetCards());
                       
            
        }
        return CurrentGameMode.IsContinueValid();
        
    }

    private IEnumerator ResetCards()
    {
        yield return new WaitForSeconds(0.3f);
        foreach (CardScript card in currentCardId) { 

            card.GetComponent<BoxCollider>().enabled = true;
            card.StartRotation(0f);
        }
        currentCardId.Clear();

    }

    private IEnumerator EnableCards()
    {
        yield return new WaitForSeconds(0.3f);
        foreach (CardScript card in currentCardId) {
            card.StartRotation(0f);

            //Destroy(card);
        }

        Debug.Log("Cards Destroyed");
        currentCardId.Clear();

    }

   
    /*private void InitCards(Vector2 cameraCenter)
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
*/

}//Cards in game
