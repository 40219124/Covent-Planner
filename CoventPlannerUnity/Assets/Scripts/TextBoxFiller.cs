using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextBoxFiller : MonoBehaviour
{
    private TextMeshProUGUI Text;

    public float TimePerChar = 0.05f;

    private void Awake()
    {
        Text = GetComponent<TextMeshProUGUI>();
    }

    public IEnumerator TextScroll(string text)
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
            if (progress == lastProgress)
            {
                continue;
            }
            Text.text = text.Substring(0, progress) + "<color=#00000000>" + text.Substring(progress);
            lastProgress = progress;
        }

        Text.text = text;
        yield return null;
    }

    public void ClearText()
    {
        Text.text = "";
    }
}
