using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionEventsRelay : MonoBehaviour
{
    public void BattleStartedTransEnd()
    {
        TransitionManager.Instance.TransitionFinished();
    }

    public void BattleEndedTransEnd()
    {
        TransitionManager.Instance.TransitionFinished();
    }
}
