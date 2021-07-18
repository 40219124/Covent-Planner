using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public static PartyManager Instance { get; private set; }

    [SerializeField]
    private Transform SceneContainer;

    private GameplayAdmin.eGameState ThisState = GameplayAdmin.eGameState.Party;

    private List<eNPC> TalkedTo = new List<eNPC>();


    private void OnEnable()
    {
        GameplayAdmin.StateChangeActivations += UpdateState;
        GameplayAdmin.ResetRunEvent += ResetRun;
    }

    private void OnDisable()
    {
        GameplayAdmin.StateChangeActivations -= UpdateState;
        GameplayAdmin.ResetRunEvent -= ResetRun;
    }

    private void ResetRun()
    {
        TalkedTo.Clear();
    }

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateState();
    }
    private void UpdateState()
    {
        SetActive(GameplayAdmin.Instance.ActiveInAdmin(ThisState));
    }

    public void SetActive(bool state)
    {
        SceneContainer.gameObject.SetActive(state);
    }

    public bool AlreadyTalked(eNPC npc)
    {
        return TalkedTo.Contains(npc);
    }

    public void AddToTalked(eNPC npc)
    {
        TalkedTo.Add(npc);
    }
}
