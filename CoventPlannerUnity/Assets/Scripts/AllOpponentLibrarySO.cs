using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AllOpponentLibrary", menuName = "ScriptableObjects/AllOpponentLibrarySO")]
public class AllOpponentLibrarySO : ScriptableObject
{
    public List<BattleOpponentSO> AllOpponents;
}
