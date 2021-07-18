using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ResetMenu : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI PerformanceText;
    [SerializeField]
    private Selectable DefaultSelected;

    [SerializeField]
    private Camera ResetCamera;

    private bool MonitorSelected = false;

    private void Awake()
    {
        CentreToCamera();
    }

    private void Start()
    {
        HideMenu();
    }

    private void Update()
    {
        if (MonitorSelected)
        {
            if(EventSystem.current.currentSelectedGameObject == null)
            {
                StartCoroutine(SelectDefault());
                MonitorSelected = false;
            }
        }
    }

    public void HideMenu()
    {
        MonitorSelected = false;
        gameObject.SetActive(false);
    }

    private void CentreToCamera()
    {
        Vector3 camPos = ResetCamera.transform.position;
        camPos.z = transform.position.z;
        transform.position = camPos;
    }

    public void DisplayMenu(List<GameplayAdmin.NPCToScore> npcScores)
    {
        gameObject.SetActive(true);

        CentreToCamera();

        string performanceText = "Your scores:\n";
        foreach(GameplayAdmin.NPCToScore pair in npcScores)
        {
            performanceText += $"  {pair.NPC}\n        {pair.Score}\n";
        }

        PerformanceText.text = performanceText;

        StartCoroutine(SelectDefault());
    }

    private IEnumerator SelectDefault()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return null;
        EventSystem.current.SetSelectedGameObject(DefaultSelected.gameObject);
        MonitorSelected = true;
    }

    public void DoReset()
    {
        GameplayAdmin.Instance.ResetRun();
        HideMenu();
    }

    public void DoEnd()
    {
        GameplayAdmin.Instance.NoMoreRuns();
        HideMenu();
    }
}
