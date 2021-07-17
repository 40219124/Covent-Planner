using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueCard : MonoBehaviour
{
    public DialogueCardSO CardDetails;
    private SpriteRenderer SpriteRenderer;
    private TextMeshProUGUI TextElement;
    private int LocationInHand;
    private bool IsUsable = true;

    private Vector3 VerticalChange = Vector3.up * 1.0f;

    private void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        TextElement = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void AssignCard(DialogueCardSO card)
    {
        CardDetails = card;
        SpriteRenderer.sprite = CardDetails.Sprite;
        SpriteRenderer.color = Color.white;

        TextElement.text = CardDetails.Body;
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
        eDialogueResponse score = opponent.GetCardTier(CardDetails);
        // ~~~ display helpful information
    }

    public void SetUsable(bool state)
    {
        IsUsable = state;
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
        if (IsUsable)
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
