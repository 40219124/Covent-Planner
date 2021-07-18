using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cutscene", menuName = "ScriptableObject/DialogueScreen/CutsceneSO")]
public class DialogueCutsceneSO : ScriptableObject
{
    public List<DialogueLineSO> Lines;
}
