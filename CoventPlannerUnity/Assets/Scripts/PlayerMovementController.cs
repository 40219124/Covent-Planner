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
    private Vector2 TravelDir = Vector2.zero;
    private Vector2Int? TravelTarget = null;

    [SerializeField]
    private float Speed = 2.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        TakeDirInput();
        MoveToTarget();
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

    private void CalcDirInput()
    {
        if (InputDir.HasFlag(eFourDirs.Right))
        {
            TravelDir = Vector2.right;
        }
        else if (InputDir.HasFlag(eFourDirs.Left))
        {
            TravelDir = Vector2.left;
        }
        else if (InputDir.HasFlag(eFourDirs.Up))
        {
            TravelDir = Vector2.up;
        }
        else if (InputDir.HasFlag(eFourDirs.Down))
        {
            TravelDir = Vector2.down;
        }
        else
        {
            TravelDir = Vector2.zero;
        }
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
        CalcDirInput();
        if (TravelDir == Vector2.zero)
        {
            TravelTarget = null;
            return;
        }
        Vector2 posPlusDir = (Vector2)transform.position + TravelDir;
        RaycastHit2D hit = Physics2D.Raycast(posPlusDir, Vector2.zero);
        if(hit.collider != null && hit.collider.tag.Equals("NoMove"))
        {
            Debug.Log("Hit!");
            TravelTarget = null;
            return;
        }
        else
        {
            Debug.Log("Miss!");
        }
        TravelTarget = new Vector2Int(Mathf.RoundToInt(posPlusDir.x), Mathf.RoundToInt(posPlusDir.y));
        // ~~~ Check a grid of empty/not-empty squares for validity
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
}
