using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleSceneManager : MonoBehaviour
{
    public static SimpleSceneManager Instance { get; private set; }
    public static string deathSceneName = "DeathScene"; // name of the death scene

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Loads a scene by name, optionally additive
    public void LoadScene(string sceneName, bool additive = false)
    {
        if (additive)
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        else
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    // Loads the main game scene (non-additive by default)
    public void LoadGameScene(string gameSceneName)
    {
        LoadScene(gameSceneName, false);
    }

    // Loads the main game scene additively
    public void LoadGameSceneAdditive(string gameSceneName)
    {
        LoadScene(gameSceneName, true);
    }
}
