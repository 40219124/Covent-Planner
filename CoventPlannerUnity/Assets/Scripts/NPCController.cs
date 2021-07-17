using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eNPC
{
    Unassigned = -1,
    Back1, Back2,
    Boss,
    CGWiz,
    DWitch,
    Nerd1,
    Side1, Side2, Side3, Side4,
    Smart1
}


public class NPCController : MonoBehaviour
{

    [SerializeField]
    private eNPC NPCType;
    [SerializeField]
    private bool FlipSprite = false;
    private Animator Animator;

    [SerializeField]
    private NPCAnimLibrarySO AnimLib;

    // Start is called before the first frame update
    void Start()
    {
        if(NPCType == eNPC.Unassigned)
        {
            Debug.LogError("Unassigned NPC " + name);
            Destroy(gameObject);
            return;
        }

        Animator = GetComponent<Animator>();
        Animator.runtimeAnimatorController = AnimLib.GetController(NPCType);

        if (FlipSprite)
        {
            Transform spriteT = GetComponentInChildren<SpriteRenderer>().transform;
            Vector3 scale = spriteT.localScale;
            scale.x *= -1;
            spriteT.localScale = scale;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
