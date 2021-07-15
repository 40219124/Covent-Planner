using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCard : MonoBehaviour
{
    public DialogueCardSO CardDetails;
    private SpriteRenderer SpriteRenderer;
    private int LocationInHand;
    private bool IsUsable = true;

    private void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void AssignCard(DialogueCardSO card)
    {
        CardDetails = card;
        SpriteRenderer.sprite = CardDetails.Sprite;
        SpriteRenderer.color = Color.white;
    }

    public void SetLocation(int location)
    {
        LocationInHand = location;
        SpriteRenderer.sortingOrder = location;
    }

    public void HoverCard()
    {
        // ~~~ instant size/position change
        // bump to front of sorting ~~~ could do with better way to get number than set to 20
        SpriteRenderer.sortingOrder = 20;
    }

    public void UnHoverCard()
    {
        // ~~~ shrink back down
        SpriteRenderer.sortingOrder = LocationInHand;
    }

    public void ShowMatchup(BattleOpponentSO opponent)
    {
        int score = opponent.Responses.Find(x => x.DialogueCard == CardDetails).Score;
        // ~~~ display helpful information
    }

    public void SetUsable(bool state)
    {
        IsUsable = state;
        SpriteRenderer.color = Color.grey;
    }
}
