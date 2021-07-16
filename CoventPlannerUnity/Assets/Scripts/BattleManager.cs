using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleManager : MonoBehaviour
{
    [SerializeField]
    private Transform SceneContainer;

    public Transform BattleCharacterMark;
    public Transform BattleCharacterWings;
    public SpriteRenderer BattleCharacter;
    [SerializeField]
    private float SlideTime = 2.0f;
    [SerializeField]
    private float PowerC = 2.0f;

    public TextMeshProUGUI DialogueText;
    [SerializeField]
    private float TimePerChar = 0.05f;

    public BattleOpponentSO DebugOpponent;
    private BattleOpponentSO Opponent;
    // Start is called before the first frame update
    void Start()
    {
        CleanTools();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time % 5 <= 1)
        {
            if (Opponent == null)
            {
                PrepareBattle(DebugOpponent);
                StartBattle();
            }
        }
    }

    private void CleanTools()
    {
        Opponent = null;
        BattleCharacter.sprite = null;
        BattleCharacter.transform.position = BattleCharacterWings.position;

        DialogueText.text = "";
    }

    public void PrepareBattle(BattleOpponentSO opponent)
    {
        Opponent = opponent;
        BattleCharacter.transform.position = BattleCharacterWings.position;
        BattleCharacter.sprite = opponent.Sprite;

        DialogueText.text = "";
    }

    public void StartBattle()
    {
        StartCoroutine(RunBattle());

    }

    private IEnumerator RunBattle()
    {
        yield return StartCoroutine(SlideCharacter(SlideInProgress));
        // ~~~ present text, etc
        yield return TextScroll(DialogueText, Opponent.OpeningText);
        yield return StartCoroutine(WaitForUser());

        for (int i = 0; i < 3; ++i)
        {

            yield return TextScroll(DialogueText, Opponent.VibeText);
            yield return StartCoroutine(WaitForUser());

            // ~~~ play card
            // ~~~ dialogue response
            // ~~~ wait user

        }

        yield return TextScroll(DialogueText, Opponent.ClosingText);
        yield return StartCoroutine(WaitForUser());

        yield return StartCoroutine(SlideCharacter(SlideOutProgress));

        CleanTools();

        // ~~~ Transition out
    }

    private IEnumerator TextScroll(TextMeshProUGUI textbox, string text)
    {
        float timeElapsed = 0.0f;

        int lastProgress = -1;
        while (lastProgress < text.Length)
        {
            yield return null;
            if (Input.anyKey && timeElapsed > 0.3f)
            {
                break;
            }
            timeElapsed += Time.deltaTime;
            int progress = (int)(timeElapsed / TimePerChar);
            if(progress == lastProgress)
            {
                continue;
            }
            textbox.text = text.Substring(0, progress) + "<color=#00000000>" + text.Substring(progress);
            lastProgress = progress;
        }

        textbox.text = text;
        yield return null;
    }

    private IEnumerator WaitForUser()
    {
        bool wait = true;
        while (wait)
        {
            if (Input.anyKeyDown)
            {
                wait = false;
            }
            yield return null;
        }
    }

    private IEnumerator SlideCharacter(System.Func<float, float> progressCalculator)
    {
        float timeElapsed = 0.0f;
        float xDiff = BattleCharacterMark.position.x - BattleCharacterWings.position.x;
        while (timeElapsed < SlideTime)
        {
            timeElapsed += Time.deltaTime;
            if (timeElapsed > SlideTime)
            {
                timeElapsed = SlideTime;
            }
            Vector3 pos = BattleCharacter.transform.position;
            float progress = progressCalculator(timeElapsed);
            pos.x = BattleCharacterWings.position.x + xDiff * progress;
            BattleCharacter.transform.position = pos;

            yield return null;
        }
    }

    private float SlideInProgress(float timeElapsed)
    {
        return Mathf.Pow(timeElapsed / SlideTime, PowerC); //Mathf.Sin((Mathf.PI * timeElapsed) / (2.0f * SlideTime));
    }

    private float SlideOutProgress(float timeElapsed)
    {
        return 1.0f - timeElapsed / SlideTime;
    }
}
