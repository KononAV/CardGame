using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] private CardScript card;
    
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

    public bool IsMatch(CardScript cardId)
    {
        Debug.Log(cardId.ShowStats().ShowId());
        currentCardId.Add(cardId);
        cardId.GetComponent<BoxCollider>().enabled = false;
        if (currentCardId.Count == 2) {
            if (currentCardId.All(x => x.ShowStats().ShowId() == cardId.ShowStats().ShowId())) Debug.Log("Same Cards!");
            else Debug.Log("Didnt Match!");

            StartCoroutine(ResetCards());
                       
            return true;
        }
        return false;
        
    }

    private IEnumerator ResetCards()
    {
        yield return new WaitForSeconds(1f);
        foreach (CardScript card in currentCardId) { card.GetComponent<BoxCollider>().enabled = true; }
        currentCardId.Clear();

    }

    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                CardScript newCard = Instantiate(card);
                newCard.ChangeMaterial(i);
                newCard.ShowStats().NewId(i);
                Debug.Log(newCard.ShowStats().ShowId());
            }
            


        }
    }

   
}
