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

    TransitionController ActiveTransition;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            PlayBattleOpeningTransition();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            if(ActiveTransition == null)
            {
                return;
            }
            Destroy(ActiveTransition.gameObject);
            ActiveTransition = null;
        }
    }

    public void PlayBattleOpeningTransition()
    {
        if(ActiveTransition != null)
        {
            return;
        }
        StartCoroutine(OpeningCoroutine());
    }

    private IEnumerator OpeningCoroutine()
    {
        ActiveTransition = Instantiate(BattleOpeningTransitions[Random.Range(0, BattleOpeningTransitions.Count)], transform).GetComponent<TransitionController>();
        yield return null;
        ActiveTransition.StartAnim();
    }

    public void PlayBattleClosingTransition()
    {

    }
}
