using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuEventRelay : MonoBehaviour
{
    public void StartGame()
    {
        ControlAdmin.Instance.ClearAllAndLoad(ControlAdmin.eSceneName.GameplayAdminScene);
    }
}
