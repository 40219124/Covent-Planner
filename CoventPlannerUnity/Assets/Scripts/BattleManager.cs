using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum eDialogueResponse { none = -1, red = 0, orange = 1, green = 2 }

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    [SerializeField]
    private Transform SceneContainer;
    [SerializeField]
    private BattleHand Hand;

    public Transform BattleCharacterMark;
    public Transform BattleCharacterWings;
    public SpriteRenderer BattleCharacter;
    [SerializeField]
    private float SlideTime = 2.0f;
    [SerializeField]
    private float PowerC = 2.0f;

    public TextBoxFiller DialogueText;
    [SerializeField]
    private float TimePerChar = 0.05f;

    public BattleOpponentSO Opponent { get; private set; }

    private GameplayAdmin.eGameState ThisState = GameplayAdmin.eGameState.Battle;

    public bool WaitingForCard { get; private set; }
    DialogueCard PlayedCard = null;

    private void OnEnable()
    {
        GameplayAdmin.StateChangeActivations += UpdateState;
    }

    private void OnDisable()
    {
        GameplayAdmin.StateChangeActivations -= UpdateState;
    }

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        UpdateState();
        CleanTools();
    }

    private void UpdateState()
    {
        SetActive(GameplayAdmin.Instance.ActiveInAdmin(ThisState));
    }
    public void SetActive(bool state)
    {
        SceneContainer.gameObject.SetActive(state);
    }



    private void CleanTools()
    {
        Opponent = null;
        BattleCharacter.sprite = null;
        BattleCharacter.transform.position = BattleCharacterWings.position;

        DialogueText.ClearText();

        WaitingForCard = false;
        PlayedCard = null;

        Hand.EmptyHand();
    }

    public void PrepareBattle(BattleOpponentSO opponent)
    {
        Opponent = opponent;
        BattleCharacter.transform.position = BattleCharacterWings.position;
        BattleCharacter.sprite = opponent.Sprite;

        DialogueText.ClearText();

        Hand.FillHand();
    }

    public void StartBattle()
    {
        SetActive(GameplayAdmin.Instance.ActiveInAdmin(ThisState));
        StartCoroutine(RunBattle());
    }

    private IEnumerator RunBattle()
    {
        int battleScore = 0;
        yield return StartCoroutine(SlideCharacter(SlideInProgress));
        // ~~~ present text, etc
        yield return DialogueText.TextScroll(Opponent.OpeningText);
        yield return StartCoroutine(WaitForUser());

        for (int i = 0; i < 3; ++i)
        {

            yield return DialogueText.TextScroll(Opponent.VibeText);
            yield return StartCoroutine(WaitForCard());
            // ~~~ play card
            DialogueResponse response = Opponent.GetFullResponse(PlayedCard.CardDetails.Object);
            battleScore += (int)response.ResponseTier;
            yield return DialogueText.TextScroll(response.ResponseText);
            yield return StartCoroutine(WaitForUser());

            PlayedCard = null;
        }

        yield return DialogueText.TextScroll(Opponent.ClosingText);
        yield return StartCoroutine(WaitForUser());

        yield return StartCoroutine(SlideCharacter(SlideOutProgress));

        Debug.Log($"Battle score {battleScore}");

        CleanTools();

        // ~~~ Transition out
        GameplayAdmin.Instance.ReturnToParty(battleScore);

    }

    private void ActivateButtonPromptIcon()
    {
        DialogueText.ActivateButtonPromptIcon();
    }
    private void ActivateCardPromptIcon()
    {
        DialogueText.ActivateCardPromptIcon();
    }
    private void HidePromptIcon()
    {
        DialogueText.HidePromptIcon();
    }

    private IEnumerator WaitForUser()
    {
        yield return StartCoroutine(DialogueText.WaitForUser());
    }
    private IEnumerator WaitForCard()
    {
        ActivateCardPromptIcon();
        WaitingForCard = true;
        while (PlayedCard == null)
        {
            yield return null;
        }
        WaitingForCard = false;
        HidePromptIcon();
    }

    /// <summary>
    /// Return true if the card was played
    /// </summary>
    public bool PlayCard(DialogueCard card)
    {
        if (WaitingForCard)
        {
            PlayedCard = card;
            CardLibrary.Instance.CombinationPlayed(Opponent, card.CardDetails.Object);
        }
        return WaitingForCard;
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
