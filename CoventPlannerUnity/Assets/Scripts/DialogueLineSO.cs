using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Line", menuName = "DialogueScreen/LineSO")]
public class DialogueLineSO : ScriptableObject
{
    public string Line = "";
    public Sprite Background;
    public float SecondsPerChar = -1.0f;
    public bool UseColour = false;
    public Color Colour;
}
