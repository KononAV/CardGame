using UnityEngine;
using UnityEngine.Pool;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class PoolManager : MonoBehaviour
{

    public static PoolManager Instance {  get; private set; }

    public IObjectPool<CardScript> cardsPool { get; set; }

    public List<CardScript> cardsList;

    [SerializeField] private GameObject cardPrefab;
    private void Awake()
    {
        if (Instance != null && Instance != this) 
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        cardsList = new();
        cardsPool = new ObjectPool<CardScript>(
            CreateCard,
            OnGetFromPool,
            OnReleaseToPool,
            OnDestroyPoolObject,
            collectionCheck: true,
            defaultCapacity: 4,
            maxSize: 28
        );    
    }

    public CardScript CreateCard()
    {
        GameObject card = Instantiate( cardPrefab );
        card.SetActive( false );
        cardsList.Add(card.GetComponent<CardScript>());
        return cardsList.Last();
    }

    private void OnGetFromPool(CardScript card)=>card.gameObject.SetActive( true );

    private void OnReleaseToPool(CardScript card) => card.gameObject.SetActive(false);

    private void OnDestroyPoolObject(CardScript card)=>Destroy(card.gameObject);

    public CardScript GetCard() {
        return cardsPool.Get(); }

    public void ReleaseCard(CardScript card) {cardsPool.Release(card); }

    public void ReleaseAllCards()
    {
        foreach (var card in cardsList)
        {
            card.GetComponent<BoxCollider>().enabled = true;
            ReleaseCard(card);
        }
    }
}
