using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BattleOppLibrary", menuName = "ScriptableObjects/BattleOppLibrarySO")]
public class BattleOppLibrarySO : ScriptableObject
{
    [System.Serializable]
    public class NPCOpponentObjects
    {
        public eNPC NPC;
        public BattleOpponentSO Object;
    }

    public List<NPCOpponentObjects> Objects;

    public BattleOpponentSO GetOpponentObject(eNPC npc)
    {
        return Objects.Find(x => x.NPC == npc)?.Object;
    }
}
