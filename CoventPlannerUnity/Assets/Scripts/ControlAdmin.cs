using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlAdmin : MonoBehaviour
{
    public static ControlAdmin Instance { get; private set; }

    public enum eSceneName { BattleScene, ControlScene, GameplayAdminScene, PartyScene, TransitionScene }

    private Scene ThisScene;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError($"Duplicate {GetType()}");
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        ThisScene = SceneManager.GetActiveScene();
        LoadScene(eSceneName.GameplayAdminScene);
        LoadScene(eSceneName.TransitionScene);
    }

    public void LoadScene(eSceneName scene)
    {
        if (SceneManager.GetSceneByName(scene.ToString()) == null)
        {
            SceneManager.LoadSceneAsync(scene.ToString());
        }
    }

    public void ClearAllAndLoad(eSceneName scene)
    {
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
}