using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayAdmin : MonoBehaviour
{
    public static event System.Action StateChangeActivations;

    public static GameplayAdmin Instance { get; private set; }

    [System.Flags]
    public enum eGameState
    {
        Running = 1 << 0,
        Paused = 1 << 1,
        ExeGroup = Running | Paused,
        Party = 1 << 2,
        Battle = 1 << 3,
        SceneGroup = Party | Battle
    }
    public eGameState GameState { get; private set; }

    private int RunScore = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError($"Duplicate {GetType()}");
            Destroy(gameObject);
            return;
        }

        CardDeck.PopulateDeck();
    }
    // Start is called before the first frame update
    void Start()
    {
        GameState = eGameState.Running | eGameState.Party;
        DialogueScreen.Instance.PlayOpening();
        ControlAdmin.Instance.LoadScene(ControlAdmin.eSceneName.PartyScene);
        ControlAdmin.Instance.LoadScene(ControlAdmin.eSceneName.BattleScene);
    }

    public bool ActiveInAdmin(eGameState scene)
    {
        return (GameState & scene) != 0;
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void StartBattleWith(BattleOpponentSO opponent)
    {
        SetGameScene(eGameState.Battle);
        // ~~~ everything else
        TransitionManager.Instance.PlayBattleOpeningTransition();
        Debug.Log("Ping!");
        BattleManager.Instance.PrepareBattle(opponent);
    }

    public void ReturnToParty(int battleScore)
    {
        RunScore += battleScore;
        SetGameScene(eGameState.Party);
        StateChangeActivations?.Invoke();
    }

    public void SetGameRunning(eGameState state)
    {
        GameState &= ~eGameState.ExeGroup;
        GameState |= state;
    }

    private void SetGameScene(eGameState scene)
    {
        GameState &= ~eGameState.SceneGroup;
        GameState |= scene;
    }

    public void TransitionFinished()
    {
        StateChangeActivations?.Invoke();
        TransitionManager.Instance.CleanUpTransition();
        if (ActiveInAdmin(eGameState.Battle))
        {
            BattleManager.Instance.StartBattle();
        }
    }

    public void EndRun()
    {
        CardLibrary.Instance.CommitNewKnowledge();
    }
}
