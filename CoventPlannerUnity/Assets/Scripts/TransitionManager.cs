using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    private static TransitionManager instance;
    public static TransitionManager Instance { get { return instance; } }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public List<Transform> BattleOpeningTransitions = new List<Transform>();
    public List<Transform> BattleClosingTransitions = new List<Transform>();

    TransitionController ActiveTransition;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            PlayBattleOpeningTransition();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            if(IsBusy)
            {
                return;
            }
            CleanUpTransition();
        }
    }

    public void PlayBattleOpeningTransition()
    {
        if(IsBusy)
        {
            return;
        }
        StartCoroutine(OpeningCoroutine(BattleOpeningTransitions));
    }

    private IEnumerator OpeningCoroutine(List<Transform> TransitionList)
    {
        ActiveTransition = Instantiate(TransitionList[Random.Range(0, TransitionList.Count)], transform).GetComponent<TransitionController>();
        yield return null;
        ActiveTransition.StartAnim();
    }

    public void PlayBattleClosingTransition()
    {
        if (IsBusy)
        {
            return;
        }
        StartCoroutine(OpeningCoroutine(BattleOpeningTransitions));
    }

    public void CleanUpTransition()
    {
        Destroy(ActiveTransition.gameObject);
        ActiveTransition = null;
    }

    public void ForceTerminate()
    {
        if (IsBusy)
        {
            CleanUpTransition();
        }
    }

    public bool IsBusy
    {
        get { return ActiveTransition != null; }
    }
}
