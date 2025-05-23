using System;
using UnityEngine;

enum Modes
{
    Basic,
    Mistake,
    Infinite,
}

public class GameMode : MonoBehaviour
{

    private Modes mode = Modes.Basic;
    public Basic gameMode;
    private int basicCardsCount = 12;
    private int basicCardsToMatchCount = 2;

    public Basic GameModeSelector()
    {
        gameMode = mode switch
        {
            Modes.Infinite => new Infinite(basicCardsToMatchCount, basicCardsCount, true, -1),
            Modes.Mistake => new Mistake(basicCardsToMatchCount, basicCardsCount, true, 10),
            _ => new Basic(basicCardsToMatchCount, basicCardsCount, false, -1)
        };
        return gameMode;
    }


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

