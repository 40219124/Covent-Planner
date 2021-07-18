using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum eFourDirs
{
    None = 0,
    Up = 1 << 0,
    Right = 1 << 1,
    Down = 1 << 2,
    Left = 1 << 3,
    Vertical = Up | Down,
    Horizontal = Right | Left
}

public class PlayerMovementController : MonoBehaviour
{
    private eFourDirs InputDir = eFourDirs.None;
    private eFourDirs PriorityInputDir = eFourDirs.None;
    private Vector2 TravelDir = Vector2.zero;
    private Vector2Int? TravelTarget = null;

    private eFourDirs FacingDir = eFourDirs.Down;

    private bool Ending = false;

    [SerializeField]
    private float Speed = 2.0f;

    private Animator Animator;

    private Vector3 StartPos;

    private void OnEnable()
    {
        GameplayAdmin.ResetRunEvent += ResetRun;
    }
    private void OnDisable()
    {
        GameplayAdmin.ResetRunEvent -= ResetRun;
    }
    private void ResetRun()
    {
        transform.position = StartPos;
        SetFacing(eFourDirs.Down);
        Ending = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        Animator = GetComponent<Animator>();
        SetFacing(eFourDirs.Down);
        StartPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        bool paused = GameplayAdmin.Instance.ActiveInAdmin(GameplayAdmin.eGameState.Paused);
        if (paused)
        {
            return;
        }

        if (!Ending)
        {
            TakeDirInput();
        }
        MoveToTarget();

        if (!Ending)
        {
            PerformActionUpdate();
        }

        if(Ending && TravelTarget == null)
        {
            GameplayAdmin.Instance.DoorExited();
            Ending = false;
        }
    }


    private void TakeDirInput()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                InputDir &= ~eFourDirs.Left;
                InputDir |= eFourDirs.Right;
            }
            else
            {
                InputDir &= ~eFourDirs.Right;
                InputDir |= eFourDirs.Left;
            }
        }
        else if ((InputDir & eFourDirs.Horizontal) != eFourDirs.None)
        {
            InputDir &= ~eFourDirs.Horizontal;
        }

        if (Input.GetAxisRaw("Vertical") != 0)
        {
            if (Input.GetAxisRaw("Vertical") > 0)
            {
                InputDir &= ~eFourDirs.Down;
                InputDir |= eFourDirs.Up;
            }
            else
            {
                InputDir &= ~eFourDirs.Up;
                InputDir |= eFourDirs.Down;
            }
        }
        else if ((InputDir & eFourDirs.Vertical) != eFourDirs.None)
        {
            InputDir &= ~eFourDirs.Vertical;
        }
    }

    private eFourDirs PrioFromMixedDirs(eFourDirs mixed)
    {
        eFourDirs prio = eFourDirs.None;
        if (mixed.HasFlag(eFourDirs.Right))
        {
            prio = eFourDirs.Right;
        }
        else if (mixed.HasFlag(eFourDirs.Left))
        {
            prio = eFourDirs.Left;
        }
        else if (mixed.HasFlag(eFourDirs.Up))
        {
            prio = eFourDirs.Up;
        }
        else if (mixed.HasFlag(eFourDirs.Down))
        {
            prio = eFourDirs.Down;
        }
        return prio;
    }

    private Vector2 DirFromFour(eFourDirs fourD)
    {
        Vector2 outDir = Vector2.zero;
        if (fourD.HasFlag(eFourDirs.Right))
        {
            outDir = Vector2.right;
        }
        else if (fourD.HasFlag(eFourDirs.Left))
        {
            outDir = Vector2.left;
        }
        else if (fourD.HasFlag(eFourDirs.Up))
        {
            outDir = Vector2.up;
        }
        else if (fourD.HasFlag(eFourDirs.Down))
        {
            outDir = Vector2.down;
        }
        return outDir;
    }

    private void MoveToTarget()
    {
        float dt = Time.deltaTime;
        while (dt > 0)
        {
            if (!TravelTarget.HasValue)
            {
                CalcMoveTarget();
            }
            if (!TravelTarget.HasValue)
            {
                break;
            }
            dt = TranslateCharacter(dt);
        }
    }

    private void CalcMoveTarget()
    {
        PriorityInputDir = PrioFromMixedDirs(InputDir);
        TravelDir = DirFromFour(PriorityInputDir);
        if (TravelDir == Vector2.zero)
        {
            TravelTarget = null;
            return;
        }

        SetFacing(PriorityInputDir);

        Vector2 posPlusDir = (Vector2)transform.position + TravelDir;
        RaycastHit2D hit = Physics2D.Raycast(posPlusDir, Vector2.zero, 1.0f, LayerMask.GetMask("Impassable"));
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("NoMove"))
            {
                Debug.Log("Hit!");
                TravelTarget = null;
                return;
            }
            else if (hit.collider.CompareTag("Finish"))
            {
                Ending = true;
            }
        }
        else
        {
            Debug.Log("Miss!");
        }
        TravelTarget = new Vector2Int(Mathf.RoundToInt(posPlusDir.x), Mathf.RoundToInt(posPlusDir.y));
        // ~~~ Check a grid of empty/not-empty squares for validity
    }

    private void SetFacing(eFourDirs dir)
    {
        FacingDir = dir;
        Animator.SetInteger("Facing", (int)FacingDir);
    }

    private float TranslateCharacter(float dt)
    {
        float remainder = 0;
        Vector2 dir = TravelTarget.Value - (Vector2)transform.position;

        Vector2 travel = dir.normalized * dt * Speed;

        if (travel.sqrMagnitude > dir.sqrMagnitude)
        {
            remainder = dir.magnitude / Speed;
            transform.position = (Vector3Int)TravelTarget.Value;
            TravelTarget = null;
        }
        else
        {
            transform.position += (Vector3)travel;
        }
        return remainder;
    }

    private void PerformActionUpdate()
    {
        if (!Input.GetButtonDown("Confirm") || TravelTarget != null)
        {
            return;
        }

        Debug.Log((Vector2)transform.position + DirFromFour(FacingDir));
        RaycastHit2D[] hits = Physics2D.RaycastAll((Vector2)transform.position + DirFromFour(FacingDir), Vector2.zero, 1.0f, LayerMask.GetMask("Person"));
        foreach(RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("NPC"))
            {
                BattleOpponentSO npcDetails = hit.collider.GetComponent<NPCController>().GetDetails();
                if (PartyManager.Instance.AlreadyTalked(npcDetails.NPCType))
                {
                    break;
                }
                PartyManager.Instance.AddToTalked(npcDetails.NPCType);
                GameplayAdmin.Instance.StartBattleWith(npcDetails);
                break;
            }
        }
    }
}
