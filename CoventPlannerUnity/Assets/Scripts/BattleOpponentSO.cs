using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueResponse
{
    public DialogueCardSO DialogueCard;
    public eDialogueResponse ResponseTier;
    public string ResponseText;
}

[CreateAssetMenu(fileName = "BattleOpponent", menuName = "ScriptableObjects/BattleOpponentSO")]
public class BattleOpponentSO : ScriptableObject
{

    public string Name;
    public Sprite Sprite;

    public string OpeningText;
    public string VibeText;
    public string ClosingText;

    public List<DialogueResponse> Responses;

    public eDialogueResponse GetCardTier(DialogueCardSO card)
    {
        return Responses.Find(x => x.DialogueCard == card).ResponseTier;
    }

    public string GetResponseText(DialogueCardSO card)
    {
        return Responses.Find(x => x.DialogueCard == card).ResponseText;
    }

    public DialogueResponse GetFullResponse(DialogueCardSO card)
    {
        return Responses.Find(x => x.DialogueCard == card);
    }
}
