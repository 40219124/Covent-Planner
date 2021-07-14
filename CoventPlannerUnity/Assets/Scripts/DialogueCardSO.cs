using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DialogueCard", menuName = "ScriptableObjects/DialogueCardSO")]
public class DialogueCardSO : ScriptableObject
{
    public string Title;
    public Sprite Sprite;
    public string Body;
}
