using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueScreen : MonoBehaviour
{
    public static DialogueScreen Instance { get; private set; }

    [SerializeField]
    private TextBoxFiller TextFiller;
    [SerializeField]
    private SpriteRenderer Background;

    [SerializeField]
    private DialogueCutsceneSO Opening;
    [SerializeField]
    private DialogueCutsceneSO PreEntrance;

    private Camera MainCamera;

    private void Awake()
    {
        Instance = this;
        MainCamera = Camera.main;
    }

    private void Update()
    {
        Vector3 camPos = MainCamera.transform.position;
        camPos.z = transform.position.z;
        transform.position = camPos;
    }

    private void CleanTools()
    {
        Background.sprite = null;
        TextFiller.ClearText();
    }

    private void SetVisible(bool state)
    {
        TextFiller.gameObject.SetActive(state);
        Background.gameObject.SetActive(state);
    }

    public void PlayOpening()
    {
        StartCoroutine(PlayScenes(new List<DialogueCutsceneSO>() { Opening, PreEntrance }));

    }

    private IEnumerator PlayScenes(List<DialogueCutsceneSO> scenes)
    {

        SetVisible(true);
        yield return null;
        yield return null;
        CleanTools();

        foreach (DialogueCutsceneSO scene in scenes)
        {
            yield return StartCoroutine(PlayScene(scene));
            yield return null;
        }
        yield return null;
        CleanTools();
        yield return null;
        SetVisible(false);
    }

    private IEnumerator PlayScene(DialogueCutsceneSO scene)
    {
        // Abort
        if (scene == null)
        {
            Debug.LogError("No such scene");
            yield break;
        }

        // Pause game
        GameplayAdmin.Instance.SetGameRunning(GameplayAdmin.eGameState.Paused);

        // Run lines
        foreach (DialogueLineSO line in scene.Lines)
        {
            // Background
            if (line.Background != null)
            {
                Background.sprite = line.Background;
            }
            // Line speed
            if (line.SecondsPerChar == -1.0f)
            {
                TextFiller.ResetSpeed();
            }
            else
            {
                TextFiller.TimePerChar = line.SecondsPerChar;
            }
            // Colour
            if (line.UseColour)
            {
                TextFiller.SetTextColour(line.Colour);
            }
            else
            {
                TextFiller.ResetColour();
            }
            // Print
            yield return StartCoroutine(TextFiller.TextScroll(line.Line));
            // Wait
            yield return StartCoroutine(TextFiller.WaitForUser());
        }

        yield return null;
        // Resume game
        GameplayAdmin.Instance.SetGameRunning(GameplayAdmin.eGameState.Running);
    }


}
