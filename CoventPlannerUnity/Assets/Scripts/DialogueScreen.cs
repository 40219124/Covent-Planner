using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueScreen : MonoBehaviour
{
    [SerializeField]
    private DialogueCutsceneSO Opening;

    [SerializeField]
    private TextBoxFiller TextFiller;
    [SerializeField]
    private Image Background;

    public void PlayOpening()
    {
        StartCoroutine(PlayScene(Opening));
    }

    private IEnumerator PlayScene(DialogueCutsceneSO scene)
    {
        foreach (DialogueLineSO line in scene.Lines)
        {
            if (line.Background != null)
            {
                Background.sprite = line.Background;
            }
            if(line.SecondsPerChar == -1.0f)
            {
                TextFiller.ResetSpeed();
            }
            else
            {
                TextFiller.TimePerChar = line.SecondsPerChar;
            }
            yield return StartCoroutine(TextFiller.TextScroll(line.Line));
            yield return StartCoroutine(TextFiller.WaitForUser());
        }
    }


}
