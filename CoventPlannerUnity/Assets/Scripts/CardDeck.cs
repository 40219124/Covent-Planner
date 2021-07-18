using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInstance
{
    public bool Playable = true;
    public DialogueCardSO Object;

    public CardInstance(DialogueCardSO cardSO)
    {
        Object = cardSO;
    }
}
public static class CardDeck
{
    public static List<CardInstance> Deck = new List<CardInstance>();

    public static void PopulateDeck()
    {
        foreach (DialogueCardSO card in CardLibrary.Instance.CardObjects.AllCards)
        {
            Deck.Add(new CardInstance(card));
        }
    }

    public static void ResetDeck()
    {
        foreach(CardInstance card in Deck)
        {
            card.Playable = true;
        }
    }
}
