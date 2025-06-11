using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] private CardScript card;

    [SerializeField] private GameObject startPosition;

    [SerializeField] private Vector3 finalPoint;

    [SerializeField] private GameObject endScreen;

    
    [SerializeField] private GameObject GameBoard;

    private Vector3 deckPlace;


    private Texture2D[] textures;
    private MeshCollider BoardProtecter;

    public bool isFool { get; private set; }


    private Basic CurrentGameMode;
    private float scores;
    private int pairs;

    
    
    private List<CardScript> currentCardId = new();

    public static GameManagerScript Instance { get; private set; }

    private void Awake()
    {/*
        if(Instance != null && Instance != this)
        {
            Destroy(Instance);
            return;
        }
*/
        Instance = this;

        //DontDestroyOnLoad(gameObject);



    }



   
    private Vector3 TrashPlaceInit()
    {
        Rect safeArea =  SaveManager.Instance.safeArea;
        Vector3 viewportPos = Camera.main.ScreenToViewportPoint(safeArea.position);

        Vector3 worldPos = Camera.main.ViewportToWorldPoint(new Vector3(viewportPos.x,0 ,viewportPos.y ));

        Vector3 maxPos = Camera.main.ViewportToWorldPoint(
            Camera.main.ScreenToViewportPoint(
                new Vector3(
                    safeArea.xMax,
                    0,
                    safeArea.yMax)));

        float width = maxPos.x - worldPos.x;
        return new Vector3(worldPos.x+width/15f, 0, worldPos.z);
    }


    void Start()
     {
        BoardProtecter = GameObject.Find("BoardProtecter").GetComponent<MeshCollider>();
        finalPoint = TrashPlaceInit();
        
        deckPlace = new Vector3(-finalPoint.x, finalPoint.y, finalPoint.z/2f);

        

        
        isFool = false;
        textures = SaveManager.Instance.saveMaterial;
        CurrentGameMode = SaveManager.Instance.gameMode;
        CurrentGameMode.InitModeFitures();  
        if (Camera.main)
        {
            Vector3 cameraCenter =  Camera.main.GetComponent<Camera>().transform.position;
            Debug.Log(cameraCenter.x+"Camera x");
            Vector2 cameraSize = new Vector2 (cameraCenter.x, cameraCenter.z);
            
            
        }
        Debug.Log(CurrentGameMode.CardsInGame);
        CurrentGameMode.CardsInGame = CurrentGameMode.SelectedCards;
        (int, int) sides = MatrixSidesAnalizer(ref CurrentGameMode.CardsInGame);
        TakeAllCards(CurrentGameMode.CardsInGame);
        CreateCardsCoroutine(TableGrid.SpiralMatrixCards(sides.Item1, sides.Item2), PoolManager.Instance.cardsList);
        
        /*if (CurrentGameMode.eventDelgates.Count != 0)
        {
            //CurrentGameMode.eventDelgates[0]();
            CurrentGameMode.eventDelgates.RemoveAt(0);
        }*/

     }

    public (int,int) MatrixSidesAnalizer(ref int square)
    {
        Debug.Log(square+"sqare");
        /*if (square % 2 != 0)
        {
            square -= 1;
        }*/
        ( int x,int y) tuple = (0,0);
        if (square <= 10)
        {
            tuple.y = 2;
        }
        else if (square <= 20) { tuple.y = 3; }
        else tuple.y = 4;

        tuple.x = (int)Math.Floor((decimal)square / tuple.y)+square%tuple.y; 
        
       
        return tuple;   
    }

  
    public void MoveToDec(in CardScript[] poolCards, int activeCards)
    {
        for (int i = 0; i < activeCards; i++)
        {

            poolCards[i].StartRotation(0f);
            poolCards[i].transform.position = deckPlace;
            PoolManager.Instance.ReleaseCard(poolCards[i]);
        }
    }

    public void TakeAllCards(int count)
    {
        for (int i = 0; i < count; i++) {

            CardScript newCard = PoolManager.Instance.GetCard();

        }
    }

    
    public void CreateCardsCoroutine(Vector3[] vectorArray, List<CardScript> cards)
    {
        CurrentGameMode.Shuffle(vectorArray);
        StartCoroutine(CreateCards());
        
        IEnumerator CreateCards()
        {
            for (int i = 0; i < vectorArray.Length / 2; i++)
            {
                int newId = (i + textures.Length) % textures.Length;

                for (int j = 0; j < CurrentGameMode.CardsToMatch; j++)
                {
                    Debug.Log(newId);
                    CardScript newCard = cards[i * CurrentGameMode.CardsToMatch + j];
                    newCard.StartRotation(180f);

                    newCard.ChangeMaterial(newId, textures[newId]);
                    newCard.StartChange(vectorArray[i * CurrentGameMode.CardsToMatch + j]);
                    

                   
                }
                yield return new WaitForSeconds(1.3f);

                for(int j = 0; j<CurrentGameMode.CardsToMatch; j++)
                {
                    cards[i * CurrentGameMode.CardsToMatch + j].StartRotation(0f);
                }

            }
                if (vectorArray.Length%2!=0)
                {
                    CardScript newCard = cards[vectorArray.Length-1];
                    newCard.transform.rotation = card.transform.rotation;
                    newCard.ChangeMaterial(-1, Resources.Load<Texture2D>("png/StarAnimals/croco"));
                    newCard.StartChange(vectorArray[vectorArray.Length-1]);
                }
            BoardProtecter.enabled = false;

        }
    }

   
    public void GameModeRestart()
    {
        StopAllCoroutines();
        scores = pairs = 0;
        for(int i =0; i < CurrentGameMode.SelectedCards; i++)
        {
            PoolManager.Instance.cardsList[i].StartRotation(0f);
        }
        
        CurrentGameMode.RestartGame();
    }
   


   /* public void CreateCards(in Vector3[] vectorArray, in List<CardScript> cards)
    {

        for(int i = 0; i< vectorArray.Length/2; i++)
        {
            int newId = (i + textures.Length) % textures.Length;
            for (int j = 0; j < CurrentGameMode.CardsToMatch; j++)
            {
                Debug.Log(newId);
                CardScript newCard = cards[i * CurrentGameMode.CardsToMatch + j];
                //
                //newCard.GetComponent<BoxCollider>().enabled = true;
                //newCard.transform.position = vectorArray[i * CurrentGameMode.CardsToMatch + j];
                //
                newCard.transform.rotation = card.transform.rotation;
                newCard.ChangeMaterial( newId,textures[newId]);
                newCard.StartChange(vectorArray[i * CurrentGameMode.CardsToMatch + j]);
                
            }
            StartCoroutine(WaitForCondition(5f));

        }
        
    }*/


    public bool IsMatch(CardScript cardId)
    {

        currentCardId.Add(cardId);
        CurrentGameMode.localCards.Remove(cardId);
        cardId.GetComponent<BoxCollider>().enabled = false;
   
        
        if (currentCardId.Count == CurrentGameMode.CardsToMatch||currentCardId.Any(z=>z.ShowStats().ShowId()!= cardId.ShowStats().ShowId())) {
            isFool = true;

            if (currentCardId.All(x => x.ShowStats().ShowId() == cardId.ShowStats().ShowId()))
            {
                pairs += 1;

                AchivmentsSO.gameEvents.TryGetValue("FirstTenPairs", out Action<object> action);
                action?.Invoke(SaveManager.Instance.saveProgressInstance.pairs+pairs);

                scores += pairs * 1.5f + CurrentGameMode.scoresRate;
                Debug.Log("Same Cards!"); 
                CurrentGameMode.CardsInGame-= CurrentGameMode.CardsToMatch;
                StartCoroutine(EnableCards());

            }
            else 
            { Debug.Log("Didnt Match!");
                scores -= 3.25f;
                CurrentGameMode.mistakes--;
                
                foreach (var del in CurrentGameMode.eventDelgates) 
                {
                    del();
                }
                StartCoroutine(ResetCards());
            }
                       
        }
        if (!CurrentGameMode.IsContinueValid())
        {
            BoardProtecter.enabled = true;

            StartCoroutine(WaitForCondition(1.3f * CurrentGameMode.CardsToMatch));

            AchivmentsSO.gameEvents.TryGetValue("FirstPlay", out Action<object> action);
            action?.Invoke(null);
            
            
            
        }

        return CurrentGameMode.IsContinueValid();
        
    }
  

    private IEnumerator WaitForCondition(float waitingRate)
    {
        yield return new WaitForSeconds(waitingRate);
        //StopAllCoroutines();
        //CurrentGameMode.IsContinueValid();
        CurrentGameMode.EndScreen(ref scores ,ref scores,ref pairs,in endScreen);
        SaveManager.Instance.saveProgressInstance.Save(total: scores, pairs: pairs);

    }

    private IEnumerator ResetCards()
    {
            yield return new WaitForSeconds(0.3f);
            foreach (CardScript card in currentCardId)
            {
            //card.GetComponent<BoxCollider>().enabled = true;
                card.StartRotation(0f);
            card.GetComponent<BoxCollider>().enabled = true;
            //CurrentGameMode.localCards.Add(card) ;
            
            }

            foreach (CardScript card in currentCardId)
        {
            CurrentGameMode.localCards.Add(card);
        }
            currentCardId.Clear();
        
        
        isFool = false;


    }

    private IEnumerator EnableCards()
    {
    
            yield return new WaitForSeconds(0.3f);

        foreach (CardScript card in currentCardId) {
            //card.StartRotation(0f);
            StartCoroutine(card.MoveToFinalPos(finalPoint.x));
            
            
        }

        //Debug.Log("Cards Destroyed");
        currentCardId.Clear();
        isFool = false;

        //StopAllCoroutines();
    }

    public void OnParticleSystemStopped()
    {
            StopAllCoroutines();
    }




}//Cards in vector
