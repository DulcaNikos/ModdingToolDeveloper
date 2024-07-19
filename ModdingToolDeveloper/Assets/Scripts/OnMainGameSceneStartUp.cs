using UnityEngine;
using UnityEngine.SceneManagement;

public class OnMainGameSceneStartUp : MonoBehaviour
{
    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /// <summary>
    /// Called when the main scene is loaded.
    /// Sets the _isSceneLoaded flag in DontDestroyOnLoadGameSceneScript to true.
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        DontDestroyOnLoadGameSceneScript.Instance._isSceneLoaded = true;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
