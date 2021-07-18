using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardLibrary : MonoBehaviour
{
    public static CardLibrary Instance { get; private set; }

    public AllCardLibrarySO CardObjects;
    public AllOpponentLibrarySO OpponentObjects;

    private bool[,] MatchupKnownTable;
    private List<Vector2Int> FutureKnowledgeCoords;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError($"Duplicate {GetType()}");
            Destroy(gameObject);
            return;
        }


        MatchupKnownTable = new bool[OpponentObjects.AllOpponents.Count, CardObjects.AllCards.Count];
    }

    public void CombinationPlayed(BattleOpponentSO opponent, DialogueCardSO card)
    {
        FutureKnowledgeCoords.Add(new Vector2Int(GetOppI(opponent), GetCardI(card)));
    }

    private int GetOppI(BattleOpponentSO opp)
    {
        return OpponentObjects.AllOpponents.FindIndex(x => x == opp);
    }

    private int GetCardI(DialogueCardSO card)
    {
        return CardObjects.AllCards.FindIndex(x => x == card);
    }

    public void CommitNewKnowledge()
    {
        foreach (Vector2Int pos in FutureKnowledgeCoords)
        {
            MatchupKnownTable[pos.x, pos.y] = true;
        }
        FutureKnowledgeCoords.Clear();
    }

    public bool MatchupIsKnown(BattleOpponentSO opponent, DialogueCardSO card)
    {
        return MatchupKnownTable[GetOppI(opponent), GetCardI(card)];
    }

    /// <summary>
    /// </summary>
    /// <param name="opponent"></param>
    /// <param name="card"></param>
    /// <returns> Returns none if the matchup is unknown, unless forced.</returns>
    public eDialogueResponse MatchupQuality(BattleOpponentSO opponent, DialogueCardSO card, bool force = false)
    {
        if (!MatchupIsKnown(opponent, card) && !force)
        {
            return eDialogueResponse.none;
        }
        return opponent.GetCardTier(card);
    }

    // ~~~ Some serialization/deserialization things
}
