using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextBoxFiller : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI Text;
    [SerializeField]
    private Animator IconAnimator;

    [SerializeField]
    private float DefaultTimePerChar = 0.05f;
    [HideInInspector]
    public float TimePerChar = 0.0f;

    private bool Filling = false;

    private void Awake()
    {
        ResetSpeed();
    }

    public void ResetSpeed()
    {
        TimePerChar = DefaultTimePerChar;
    }

    public IEnumerator TextScroll(string text)
    {
        if (Filling)
        {
            Filling = false;
            yield return null;
        }
        Filling = true;
        float timeElapsed = 0.0f;

        int lastProgress = -1;
        while (lastProgress < text.Length)
        {
            yield return null;
            if (!Filling)
            {
                break;
            }
            if (Input.anyKey && timeElapsed > 0.3f)
            {
                break;
            }
            timeElapsed += Time.deltaTime;
            int progress = (int)(timeElapsed / TimePerChar);
            if (progress == lastProgress)
            {
                continue;
            }
            Text.text = text.Substring(0, progress) + "<color=#00000000>" + text.Substring(progress);
            lastProgress = progress;
        }

        if (Filling)
        {
            Text.text = text;
        }
        else
        {
            Text.text = "";
        }

        Filling = false;
        yield return null;
    }

    public void ClearText()
    {
        Text.text = "";
    }

    public IEnumerator WaitForUser()
    {
        ActivateButtonPromptIcon();
        yield return StartCoroutine(ControlAdmin.Instance.WaitForUser());
        HidePromptIcon();
    }

    public void ActivateButtonPromptIcon()
    {
        IconAnimator.SetBool("Button", true);
    }
    public void ActivateCardPromptIcon()
    {
        IconAnimator.SetBool("Card", true);
    }
    public void HidePromptIcon()
    {
        IconAnimator.SetBool("Button", false);
        IconAnimator.SetBool("Card", false);
    }

}
