using System;
using UnityEngine;

public enum Modes
{
    Basic = 0,
    Mistake = 1,
    Infinite = 2,
}

public class GameMode : MonoBehaviour
{
    private static int basicCardsCount = 12;
    private static int basicCardsToMatchCount = 2;

    public static Basic GameModeSelector(int selectedMode)
    
        => selectedMode switch
        {
            (int)Modes.Infinite => new Infinite(basicCardsToMatchCount, basicCardsCount, true, -1),
            (int)Modes.Mistake => new Mistake(basicCardsToMatchCount, basicCardsCount, true, 3),
            (int)Modes.Basic => new Basic(basicCardsToMatchCount, basicCardsCount, false, -1),
            _ => throw new ArgumentOutOfRangeException(nameof(selectedMode))
        };
    
}

public class Basic
{
    public int CardsToMatch;
    public int CardsInGame;
    public bool isInfinite;
    public int mistakes;

    public Basic(int cardsToMatch, int CardsInGame, bool isInfinite, int mistakes)
    {
        this.CardsToMatch = cardsToMatch;
        this.CardsInGame = CardsInGame;
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
    public Mistake(int cardsToMatch, int CardsInGame, bool isInfinite, int mistakes) : base(cardsToMatch, CardsInGame, isInfinite, mistakes) { }

    public override bool IsContinueValid()
    {
        return mistakes > 0;
    }
}
public class Infinite : Basic
{
    public Infinite(int cardsToMatch, int CardsInGame, bool isInfinite, int mistakes) : base(cardsToMatch, CardsInGame, isInfinite, mistakes) { }
    public override bool IsContinueValid()
    {
        return true;
    }

}

