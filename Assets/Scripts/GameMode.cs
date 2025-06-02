using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Modes
{
    Basic = 0,
    Mistake = 1,
    Infinite = 2,
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

    public Basic(int cardsToMatch, bool isInfinite, int mistakes)
    {
        this.CardsToMatch = cardsToMatch;
        this.CardsInGame = SelectedCards;
        this.isInfinite = isInfinite;
        this.mistakes = mistakes;
        
    }

    public virtual bool IsContinueValid()
    {
        Debug.Log(CardsInGame+ "cards in game");
        return CardsInGame >= CardsToMatch;
    }

}

public class Mistake : Basic
{
    public Mistake(int cardsToMatch, bool isInfinite, int mistakes) : base(cardsToMatch, isInfinite, mistakes) { }

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

/*public class Swipe: Basic
{
    public Swipe(int cardsToMatch, bool isInfinite, int mistakes) : base(cardsToMatch, isInfinite, mistakes)
    {
    }

    public override bool IsContinueValid() 
    {
        if(base.IsContinueValid());
        //GameObject.FindAll
    }
}
*/