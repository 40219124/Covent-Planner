using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCard : MonoBehaviour
{
    public DialogueCardSO CardDetails;
    private SpriteRenderer SpriteRenderer;
    private int LocationInHand;
    private bool IsUsable = true;

    private Vector3 VerticalChange = Vector3.up * 1.0f;

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

    public void SetHandLocation(int location)
    {
        LocationInHand = location;
        SetZLocation(LocationInHand);
    }

    private void SetZLocation(int location)
    {
        SpriteRenderer.sortingOrder = location;
        Vector3 pos = transform.position;
        pos.z = -0.01f * location;
        transform.position = pos;
    }

    public void HoverCard()
    {
        // ~~~ instant size/position change
        transform.Translate(VerticalChange);
        // bump to front of sorting ~~~ could do with better way to get number than set to 20
        SetZLocation(20);
    }

    public void UnHoverCard()
    {
        // ~~~ shrink back down
        transform.Translate(-VerticalChange);
        SetZLocation(LocationInHand);
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

    private void OnMouseEnter()
    {
        HoverCard();
    }

    private void OnMouseExit()
    {
        UnHoverCard();
    }
}
