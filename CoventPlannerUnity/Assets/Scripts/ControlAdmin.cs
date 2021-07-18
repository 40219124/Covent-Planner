using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlAdmin : MonoBehaviour
{
    public static ControlAdmin Instance { get; private set; }

    public enum eSceneName { BattleScene, ControlScene, GameplayAdminScene, PartyScene, TransitionScene, MainMenuScene }

    private Scene ThisScene;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        ThisScene = SceneManager.GetActiveScene();
        LoadScene(eSceneName.MainMenuScene);
        LoadScene(eSceneName.TransitionScene);
    }

    public void LoadScene(eSceneName scene)
    {
        if (!SceneManager.GetSceneByName(scene.ToString()).IsValid())
        {
            SceneManager.LoadSceneAsync(scene.ToString(), LoadSceneMode.Additive);
        }
    }
    public void ClearAllAndLoad(eSceneName scene)
    {
        StartCoroutine(ClearAllAndLoadCo(scene));
    }
    private IEnumerator ClearAllAndLoadCo(eSceneName scene)
    {
        yield return null;
        // Get all scenes
        int sCount = SceneManager.sceneCount;
        Scene[] scenes = new Scene[sCount];
        for (int i = 0; i < sCount; ++i)
        {
            scenes[i] = SceneManager.GetSceneAt(i);
        }
        // Unload all other scenes (not the control scene)
        foreach (Scene s in scenes)
        {
            if (s != ThisScene)
            {
                SceneManager.UnloadSceneAsync(s);
            }
        }
        // Load requested scene
        LoadScene(scene);
    }


    public IEnumerator WaitForUser()
    {
        bool wait = true;
        while (wait)
        {
            if (Input.GetButtonDown("Confirm") || Input.GetMouseButtonDown(0))
            {
                wait = false;
            }
            yield return null;
        }
    }
}
