using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleHand : MonoBehaviour
{
    [SerializeField]
    private Transform CardPrefab;

    private float CardGap = 1.0f;

    private List<DialogueCard> Cards = new List<DialogueCard>();

    private bool AssignOnEnable = false;

    private void OnEnable()
    {
        if (AssignOnEnable)
        {
            StartCoroutine(AssignInstantiatedCards());
            AssignOnEnable = false;
        }
    }

    private IEnumerator AssignInstantiatedCards()
    {

        foreach (CardInstance ci in CardDeck.Deck)
        {
            DialogueCard dc = Instantiate(CardPrefab, transform).GetComponent<DialogueCard>();
            //dc.AssignCard(ci);
            //dc.SetHandLocation(Cards.Count);

            Cards.Add(dc);
        }

        yield return null;
        yield return null;
        yield return null;

        for (int i = 0; i < Cards.Count; ++i)
        {
            Cards[i].AssignCard(CardDeck.Deck[i]);
            Cards[i].SetHandLocation(i);
        }

        float halfHand = (Cards.Count - 1) * 0.5f;
        for (int i = 0; i < Cards.Count; ++i)
        {
            Cards[i].transform.localPosition = new Vector3(-halfHand + i * CardGap, 0.0f, 0.0f);
        }
    }

    public void FillHand()
    {
        AssignOnEnable = true;
    }

    public void EmptyHand()
    {
        foreach (DialogueCard card in Cards)
        {
            Destroy(card.gameObject);
        }
        Cards.Clear();
    }
}
