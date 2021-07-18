using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopDown : MonoBehaviour
{
    public static int ScreenHeight = 1080;
    RectTransform rt;
    Vector3 downPos;
    Vector3 upPos;
    public bool isDown;

    void Awake()
    {
        //get the recttransform of the sliding panel 
        rt = GetComponent<RectTransform>();
        downPos = rt.localPosition;
        upPos = downPos + new Vector3(0, PopDown.ScreenHeight, 0);
        if (isDown)
            SetDown();
        else
            SetUp();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Toggle();
        }
    }

    public void SetDown()
    {
        rt.localPosition = downPos;
        isDown = true;
    }

    public void SetUp()
    {
        rt.localPosition = upPos;
        isDown = false;
    }

    public void Toggle()
    {
        if (isDown)
            SetUp();
        else
            SetDown();
    }
}