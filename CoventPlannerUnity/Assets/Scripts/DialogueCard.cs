using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueCard : MonoBehaviour
{
    public CardInstance CardDetails { get; private set; }
    private SpriteRenderer SpriteRenderer;
    private TextMeshProUGUI TextElement;
    private int LocationInHand;

    private Vector3 VerticalChange = Vector3.up * 1.0f;

    private void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        TextElement = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void AssignCard(CardInstance card)
    {
        CardDetails = card;
        SpriteRenderer.sprite = CardDetails.Object.Sprite;
        SpriteRenderer.color = Color.white;

       // TextElement.text = CardDetails.Object.Body;

        SetUsable(card.Playable);

        ShowMatchup();
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

    private void HoverCard()
    {
        // ~~~ instant size/position change
        transform.Translate(VerticalChange);
        // bump to front of sorting ~~~ could do with better way to get number than set to 20
        SetZLocation(20);
    }

    private void UnHoverCard()
    {
        // ~~~ shrink back down
        transform.Translate(-VerticalChange);
        SetZLocation(LocationInHand);
    }

    private void ShowMatchup()
    {
        eDialogueResponse score = CardLibrary.Instance.MatchupQuality(BattleManager.Instance.Opponent, CardDetails.Object);
        if (score != eDialogueResponse.none)
        {
            // ~~~ display helpful information

        }
    }

    public void SetUsable(bool state)
    {
        CardDetails.Playable = state;
        if (state == false)
        {
            SpriteRenderer.color = Color.grey;
        }
        else
        {
            SpriteRenderer.color = Color.white;
        }
    }

    private void OnMouseUpAsButton()
    {
        if (CardDetails.Playable)
        {
            if (BattleManager.Instance.PlayCard(this))
            {
                SetUsable(false);
            }
        }
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
