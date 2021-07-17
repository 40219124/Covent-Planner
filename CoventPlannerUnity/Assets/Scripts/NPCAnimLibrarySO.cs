using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCAnimatorLibrary", menuName = "ScriptableObjects/NPCAnimatorLibrarySO")]
public class NPCAnimLibrarySO : ScriptableObject
{
    [System.Serializable]
    public class NPCAnimatorOverrides
    {
        public eNPC NPC;
        public AnimatorOverrideController Controller;
    }

    public List<NPCAnimatorOverrides> Controllers;

    public AnimatorOverrideController GetController(eNPC npc)
    {
        return Controllers.Find(x => x.NPC == npc)?.Controller;
    }
}
