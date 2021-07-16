using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public static PartyManager Instance { get; private set; }

    [SerializeField]
    private Transform SceneContainer;

    private GameplayAdmin.eGameState ThisState = GameplayAdmin.eGameState.Party;


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
        SetActive(GameplayAdmin.Instance.ActiveInAdmin(ThisState));
    }

    public void SetActive(bool state)
    {
        SceneContainer.gameObject.SetActive(state);
    }
}
