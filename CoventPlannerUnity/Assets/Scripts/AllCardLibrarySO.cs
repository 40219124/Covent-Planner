using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AllCardLibrary", menuName = "ScriptableObjects/AllCardLibrarySO")]
public class AllCardLibrarySO : ScriptableObject
{
    public List<DialogueCardSO> AllCards;
}
