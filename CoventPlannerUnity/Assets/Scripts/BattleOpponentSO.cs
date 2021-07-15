using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
    public class DialogueResponse
{
    public DialogueCardSO DialogueCard;
    public string Response;
    public int Score;
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
}
