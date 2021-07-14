using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public Transform BattleCharacterMark;
    public Transform BattleCharacterWings;
    public SpriteRenderer BattleCharacter;
    [SerializeField]
    private float SlideTime = 2.0f;
    [SerializeField]
    private float PowerC = 2.0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time % 5 <= 1)
        {
            BattleCharacter.transform.position = BattleCharacterWings.position;
            StartBattle();
        }
    }

    public void PrepareBattle(BattleOpponentSO opponent)
    {
        BattleCharacter.transform.position = BattleCharacterWings.position;
        BattleCharacter.sprite = opponent.Sprite;
    }

    public void StartBattle()
    {
        StartCoroutine(RunBattle());
    }

    private IEnumerator RunBattle()
    {
        yield return StartCoroutine(SlideInCharacter());
        // ~~~ present text, etc
    }

    private IEnumerator SlideInCharacter()
    {
        float timeElapsed = 0.0f;
        float xDiff = BattleCharacterMark.position.x - BattleCharacterWings.position.x;
        while (BattleCharacter.transform.position.x != BattleCharacterMark.position.x)
        {
            timeElapsed += Time.deltaTime;
            if(timeElapsed >= SlideTime)
            {
                BattleCharacter.transform.position = BattleCharacterMark.position;
                break;
            }
            Vector3 pos = BattleCharacter.transform.position;
            float progress = Mathf.Pow(timeElapsed / SlideTime, PowerC); //Mathf.Sin((Mathf.PI * timeElapsed) / (2.0f * SlideTime));
            pos.x = BattleCharacterWings.position.x + xDiff * progress;
            BattleCharacter.transform.position = pos;

            yield return null;
        }
    }
}
