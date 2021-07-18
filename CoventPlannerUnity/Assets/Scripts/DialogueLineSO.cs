using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Line", menuName = "ScriptableObject/DialogueScreen/LineSO")]
public class DialogueLineSO : ScriptableObject
{
    public string Line = "";
    public Sprite Background;
    public float SecondsPerChar = -1.0f;
}
