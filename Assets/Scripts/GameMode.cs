using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum Modes
{
    Basic = 0,
    Mistake = 1,
    Infinite = 2,
    Swipe = 3,
}

public class GameMode : MonoBehaviour
{
    private static int basicCardsToMatchCount = 2;

    public static Basic GameModeSelector(int selectedMode)
    
        => selectedMode switch
        {
            (int)Modes.Infinite => new Infinite(basicCardsToMatchCount, true, -1),
            (int)Modes.Mistake => new Mistake(basicCardsToMatchCount, true, 3),
            (int)Modes.Basic => new Basic(basicCardsToMatchCount, false, -1),
           

            _ => throw new ArgumentOutOfRangeException(nameof(selectedMode))
        };
    
}

public class Basic
{
    public int CardsToMatch;
    public int CardsInGame;
    public bool isInfinite;
    public int mistakes;
    public int SelectedCards;

    private bool isJoker;
    public bool isSwipe;

    public List<CardScript> localCards;

    public List<EventDelegate> eventDelgates;

    public Basic(int cardsToMatch, bool isInfinite, int mistakes)
    {
        this.CardsToMatch = cardsToMatch;
        this.CardsInGame = SelectedCards;
        this.isInfinite = isInfinite;
        this.mistakes = mistakes;

        eventDelgates = new List<EventDelegate>();
        Debug.Log(isSwipe + "swipe");
       

    }

    public virtual void InitModeFitures()
    { 
        if (isSwipe) {
            eventDelgates.Add(AfterCardsInit);
            eventDelgates.Add(Swipe); 
        }

    }

    public virtual bool IsContinueValid()
    {
        Debug.Log(CardsInGame + "cards in game");
        return CardsInGame >= CardsToMatch;
    }

   
    public virtual void Swipe() {

        List<CardScript> shuffledCards = new List<CardScript>(localCards);
        Shuffle(shuffledCards);

        List<CardScript> selectedCards = shuffledCards.Take(CardsToMatch).ToList();

        List<Vector3> targetPositions = selectedCards.Select(card => card.transform.position).ToList();

        Shuffle(targetPositions);
        for (int i = 0; i < targetPositions.Count; i++) { selectedCards[i].StartChange(targetPositions[i]); }


    }

    private void Shuffle<T>(List<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public virtual void AfterCardsInit()
    {
        localCards = new List<CardScript>();

        Debug.Log("cards in game:" + CardsInGame + " cards in pool:" + PoolManager.Instance.cardsList.Count);
        foreach (var card in PoolManager.Instance.cardsList)
        {
            localCards.Add(card);
        }

        foreach(var card in localCards)
        {
            Debug.Log(card+" local card");
        }
    }

 

    public delegate void EventDelegate();
}

public class Mistake : Basic
{
    public Mistake(int cardsToMatch, bool isInfinite, int mistakes) : base(cardsToMatch, isInfinite, mistakes) {  }

    public override bool IsContinueValid()
    {
        Debug.Log("Infinit");

        return mistakes > 0;
    }

    
}
public class Infinite : Basic
{
    public Infinite(int cardsToMatch, bool isInfinite, int mistakes) : base(cardsToMatch, isInfinite, mistakes) { }
    public override bool IsContinueValid()
    {
        Debug.Log("Cards in game:" + CardsInGame + " Cards to mathc:" + CardsToMatch);
        if (CardsInGame < CardsToMatch) { SceneManager.LoadScene("SampleScene");  }

        
        return true;
    }

}


