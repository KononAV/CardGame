using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] private CardScript card;

    [SerializeField] private GameObject startPosition;

    private GameMode gameMode;

    private Basic CurrentGameMode;
    
    public List<CardScript> currentCardId = new();

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
        InitCards();



    }


    public bool IsMatch(CardScript cardId)
    {
        Debug.Log(cardId.ShowStats().ShowId());
        currentCardId.Add(cardId);
        cardId.GetComponent<BoxCollider>().enabled = false;
        if (currentCardId.Count == 2) {
            if (currentCardId.All(x => x.ShowStats().ShowId() == cardId.ShowStats().ShowId()))
            {
                Debug.Log("Same Cards!"); 
                gameMode.gameMode.CardsInGame--;
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

   
    private void InitCards()
    {
        for (int i = 0; i < CurrentGameMode.CardsInGame; i++)
        {
            for (int j = 0; j < CurrentGameMode.CardsToMatch; j++)
            {
                CardScript newCard = Instantiate(card, startPosition.transform.position+ new Vector3(3f,0f,0f), card.transform.rotation);
                newCard.ChangeMaterial(i);
                newCard.ShowStats().NewId(i);
                Debug.Log(newCard.ShowStats().ShowId());
                startPosition.transform.position = newCard.transform.position;   
            }
            


        }

    }

    

    delegate void Message();
}
