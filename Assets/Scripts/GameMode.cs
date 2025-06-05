using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using TMPro;


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
    public float scoresRate;
    public bool isSwipe;

    System.Random rng;

    public List<CardScript> localCards;

    public List<EventDelegate> eventDelgates;

    public Basic(int cardsToMatch, bool isInfinite, int mistakes)
    {
        this.CardsToMatch = cardsToMatch;
        this.CardsInGame = SelectedCards;
        this.isInfinite = isInfinite;
        this.mistakes = mistakes;
        scoresRate = 10;
        eventDelgates = new List<EventDelegate>();
        localCards = new List<CardScript>();
        //Debug.Log(isSwipe + "swipe");
        rng = new System.Random();

    }

    public virtual void InitModeFitures()
    {
        if (isSwipe)
        {
            //eventDelgates.Add(AfterCardsInit);
            eventDelgates.Add(Swipe);
            scoresRate += 5;
            
        }

    }

    public virtual bool IsContinueValid()
    {
        Debug.Log(CardsInGame + "cards in game");
        return CardsInGame >= CardsToMatch;
    }


    public virtual void Swipe()
    {
        List<CardScript> shuffledCards = new List<CardScript>(localCards);
        Shuffle(shuffledCards);

        shuffledCards = shuffledCards.Take(CardsToMatch).ToList();

        List<Vector3> targetPositions = shuffledCards.Select(card => card.transform.position).ToList();

        Shuffle(targetPositions);
        for (int i = 0; i < targetPositions.Count; i++)
        {

            localCards.Remove(shuffledCards[i]);
            shuffledCards[i].GetComponent<BoxCollider>().enabled = false;
            shuffledCards[i].StartChange(targetPositions[i]);

        }


    }

    public virtual void Shuffle<T>(IList<T> list)
    {
        
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(0, n - 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public virtual void AfterCardsInit()
    {
        localCards = new List<CardScript>();

        foreach (var card in PoolManager.Instance.cardsList)
        {
            localCards.Add(card);
        }

    }


    public virtual void RestartGame()
    {
        CardsInGame = SelectedCards;
        localCards.Clear();


        (int, int) sides = GameManagerScript.Instance.MatrixSidesAnalizer(ref SelectedCards);
        GameManagerScript.Instance.CreateCardsCoroutine(TableGrid.SpiralMatrixCards(sides.Item1, sides.Item2), PoolManager.Instance.cardsList);
    }


    public virtual void EndScreen(ref float total ,ref float scores, ref int pairs, in GameObject screen)
    {


        screen.SetActive(true);
        TextMeshProUGUI scoresText = GameObject.Find("Scores")?.GetComponent<TextMeshProUGUI>();
        scoresText.text = $"Scores: {scores}";

        TextMeshProUGUI pairText = GameObject.Find("Pairs")?.GetComponent<TextMeshProUGUI>();
        pairText.text = $"Pairs: {pairs}";

        TextMeshProUGUI totalText = GameObject.Find("Total")?.GetComponent<TextMeshProUGUI>();
        totalText.text = $"Total: {total}";

        total = scores = pairs = 0;

    }




    public delegate void EventDelegate();
}

public class Mistake : Basic
{
    public Mistake(int cardsToMatch, bool isInfinite, int mistakes) : base(cardsToMatch, isInfinite, mistakes) 
    {
        scoresRate = 15;
    }

    public override bool IsContinueValid()
    {
        Debug.Log("Mistake");

        return mistakes > 0;
    }


}
public class Infinite : Basic
{
    public Infinite(int cardsToMatch, bool isInfinite, int mistakes) : base(cardsToMatch, isInfinite, mistakes) { }
    public override bool IsContinueValid()
    {



        Debug.Log("Cards in game:" + CardsInGame + " Cards to mathc:" + CardsToMatch);
        if (CardsInGame < CardsToMatch)
        {

            if (GameManagerScript.Instance.isFool) return false;

            //PoolManager.Instance.ReleaseAllCards();
            /*Debug.Log("Selected cards" + SelectedCards);
            base.RestartGame();*/
            return false;
        }


        return true;
    }

    public override void EndScreen(ref float total,ref float scores,ref int pairs, in GameObject screen=null)
    {
        Debug.Log("Selected cards" + SelectedCards);
        base.RestartGame();
    }

}