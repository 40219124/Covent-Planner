using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueCard : MonoBehaviour
{
    public CardInstance CardDetails { get; private set; }
    public eDialogueResponse Matchup { get; private set; }

    private SpriteRenderer CardSprite;
    [SerializeField]
    private SpriteRenderer BorderSprite;
    [SerializeField]
    private TextMeshProUGUI TextElement;
    private int LocationInHand;

    private static BattleHand Hand;

    private Vector3 VerticalChange = Vector3.up * 1.0f;

    private bool Hovered = false;

    private void Start()
    {
        if (Hand == null)
        {
            Hand = GetComponentInParent<BattleHand>();
        }
        CardSprite = GetComponent<SpriteRenderer>();
    }

    public void AssignCard(CardInstance card)
    {
        CardDetails = card;
        CardSprite.sprite = CardDetails.Object.Sprite;
        CardSprite.color = Color.white;

        // TextElement.text = CardDetails.Object.Body;

        ShowMatchup();
        SetUsable(card.Playable);

    }

    public void SetHandLocation(int location)
    {
        LocationInHand = location;
        SetZLocation(LocationInHand);
    }

    private void SetZLocation(int location)
    {
        CardSprite.sortingOrder = location;
        Vector3 pos = transform.position;
        pos.z = -0.01f * location;
        transform.position = pos;
    }

    private void HoverCard()
    {
        Hovered = true;
        Hand.NewHoveredCard(this);
        // ~~~ instant size/position change
        transform.Translate(VerticalChange);
        // bump to front of sorting ~~~ could do with better way to get number than set to 20
        SetZLocation(20);
    }

    private void UnHoverCard()
    {
        Hovered = false;
        Hand.CardUnhovered(this);
        // ~~~ shrink back down
        transform.Translate(-VerticalChange);
        SetZLocation(LocationInHand);
    }

    private void ShowMatchup()
    {
        Matchup = CardLibrary.Instance.MatchupQuality(BattleManager.Instance.Opponent, CardDetails.Object);
        BorderSprite.color = ColourFromMatchup(Matchup);
        if (Matchup != eDialogueResponse.none)
        {
            // ~~~ display helpful information
            // CardSprite.color = ColourFromMatchup(score);

        }
    }

    private Color ColourFromMatchup(eDialogueResponse matchup)
    {
        switch (matchup)
        {
            case eDialogueResponse.none:
                return Color.grey;
            case eDialogueResponse.red:
                return Color.red;
            case eDialogueResponse.orange:
                return Color.yellow;
            case eDialogueResponse.green:
                return Color.green;
            default:
                return Color.grey;
        }
    }

    private string TextFromMatchup(eDialogueResponse matchup)
    {
        switch (matchup)
        {
            case eDialogueResponse.none:
                return "Unknown";
            case eDialogueResponse.red:
                return "Poor";
            case eDialogueResponse.orange:
                return "Fine";
            case eDialogueResponse.green:
                return "Good";
            default:
                return "Unknown";
        }
    }

    public void SetUsable(bool state)
    {
        CardDetails.Playable = state;
        if (state == false)
        {
            CardSprite.color = Color.grey;
        }
        else
        {
            //CardSprite.color = Color.white;
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
