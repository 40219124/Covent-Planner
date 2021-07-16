using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayAdmin : MonoBehaviour
{
    public static GameplayAdmin Instance { get; private set; }

    [System.Flags]
    public enum eGameState { 
        Running = 1<<0,
        Paused = 1<<1,
        Party = 1<<2,
        Battle = 1<<3
    }
    public eGameState GameState = eGameState.Paused;


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
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        ControlAdmin.Instance.LoadScene(ControlAdmin.eSceneName.PartyScene);
        ControlAdmin.Instance.LoadScene(ControlAdmin.eSceneName.BattleScene);
        GameState = eGameState.Paused | eGameState.Party;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
