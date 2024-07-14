using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct ModPackage
{
    ///<summary> The name of the mod. </summary>
    public string Name;
    ///<summary> The description of the mod. </summary>
    public string Description;
    ///<summary> The author of the mod. </summary>
    public string Author;
    ///<summary> The version of the mod. </summary>
    public string Version;
    ///<summary> The version of unity. </summary>
    public string UnityVersion;
    ///<summary> The name of the asset. </summary>
    public string AssetName;
    ///<summary> The name of the asset. </summary>
    public string BundleName;
}

public class DontDestroyOnLoadGameSceneScript : MonoBehaviour
{
    /// <summary> Singleton instance of this script. </summary>
    public static DontDestroyOnLoadGameSceneScript Instance;

    /// <summary> Dictionary to store objects to spawn, associated with mod packages. </summary>
    private Dictionary<string, (ModPackage, GameObject)> _ObjectsToSpawn = new Dictionary<string, (ModPackage, GameObject)>();

    /// <summary> Flag to indicate if the scene is loaded. </summary>
    public bool _isSceneLoaded { get; set; }

    private void Awake()
    {
        // Ensure only one instance of this script exists
        if (Instance == null)
        {
            Instance = this;

            // Initialize scene loaded flag with a default value
            _isSceneLoaded = false;

            // Make this game object persist across scene loads
            DontDestroyOnLoad(gameObject);

            // Register for sceneLoaded event
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            // If another instance exists, destroy this one
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Called when a scene finishes loading.
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if we are in the main game scene
        if (_isSceneLoaded == true)
        {
            // Assign objects to the dictionary.
            _ObjectsToSpawn = SerializationHelper.LoadDictionary();
            // Replace objects in the scene.
            LoadObjectFromBundle.Instance.ReplaceObjectsInScene(_ObjectsToSpawn);
        }
    }

    /// <summary>
    /// Called when the script's GameObject is destroyed
    /// </summary>
    private void OnDestroy()
    {
        // Unregister from sceneLoaded event when this instance is destroyed
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}