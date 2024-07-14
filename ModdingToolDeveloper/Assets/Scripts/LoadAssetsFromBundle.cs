using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadAssetsFromBundle : MonoBehaviour
{
    /// <summary> Singleton instance of this script. </summary>
    public static LoadAssetsFromBundle Instance;

    /// <summary> List of generic AssetReference for Gameobject assets. </summary>
    [SerializeField, Tooltip("List of generic AssetReference for Gameobject assets.")]
    private List<AssetReference> _AddressableAssets;
    /// <summary> List of AssetReferenceT for Material assets. </summary>
    [SerializeField, Tooltip("List of AssetReferenceT for Material assets.")]
    private List<AssetReferenceT<Material>> _AddressableMaterialAssets;
    /// <summary> List of AssetReferenceT for AudioClip assets. </summary>
    [SerializeField, Tooltip("List of AssetReferenceT for AudioClip assets.")]
    private List<AssetReferenceT<AudioClip>> _AddressableAudioClipAssets;
    //Create more if you want

    /// <summary> Prefab used for creating buttons in the UI. </summary>
    [SerializeField, Tooltip("Prefab used for creating buttons in the UI.")]
    private GameObject _ButtonAssetPrefab;
    /// <summary> Name of the main scene to load additively. </summary>
    [SerializeField, Tooltip("Name of the main scene to load additively.")]
    private string _MainSceneName = "GameScene";

    /// <summary> Name of the currently selected button. </summary>
    private string _ButtonName;
    /// <summary> Dictionary to store lists of buttons associated with their names. </summary>
    private Dictionary<string, List<Button>> _Buttons = new Dictionary<string, List<Button>>();

    /// <summary> Property to access the currently selected button name. </summary>
    public string ButtonName => _ButtonName;
    /// <summary> Property to access the dictionary of buttons. </summary>
    public Dictionary<string, List<Button>> Buttons => _Buttons;

    /// <summary> Reference to the found GameObject in the scene. </summary>
    public GameObject FoundObject { get; set; }

    private void Start()
    {
        // Singleton pattern enforcement
        if (Instance != null && Instance != this) { Destroy(this); }
        Instance = this;

        // Start coroutine to load the preview scene
        StartCoroutine(LoadPreviewScene());
    }

    /// <summary>
    /// Coroutine to load the main scene additively and subscribe to events.
    /// </summary>
    private IEnumerator LoadPreviewScene()
    {
        // Load the main scene additively
        yield return SceneManager.LoadSceneAsync(_MainSceneName, LoadSceneMode.Additive);

        // Subscribe to ModListUI events for button creation and texture assignment
        ModListUI.Instance.ModButtonsCreated += LoadAndInstantiateAsset;
        ModListUI.Instance.ModButtonsCreated += PreviewManager.Instance.AssignTexture;
        ModListUI.Instance.CreateButtons();
    }

    /// <summary>
    /// Unsubscribe from events when the GameObject is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        // Unsubscribe from the events to avoid memory leaks
        ModListUI.Instance.ModButtonsCreated -= LoadAndInstantiateAsset;
        ModListUI.Instance.ModButtonsCreated -= PreviewManager.Instance.AssignTexture;
    }

    /// <summary>
    /// Load and instantiate assets from Addressables when ModButtonsCreated event is raised.
    /// </summary>
    private void LoadAndInstantiateAsset(object sender, EventArgs e)
    {
        foreach (AssetReference assetReference in _AddressableAssets)
        {
            // Iterate through each asset reference and load it asynchronously
            assetReference.LoadAssetAsync<GameObject>().Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    // Create a button in the UI for each loaded asset
                    CreateButtonForAsset(handle.Result);
                }
                else
                {
                    Debug.LogError("Failed to load asset from Addressables.");
                }
            };
        }
    }

    /// <summary>
    /// Create a button in the UI for the loaded asset.
    /// </summary>
    private void CreateButtonForAsset(GameObject loadedAsset)
    {
        foreach (Transform child in ModListUI.Instance.AssetsListPanelParent)
        {
            // Navigate through the hierarchy to find the correct panel for button creation
            Transform scrollView = child.GetChild(1); // Scroll View
            Transform viewport = scrollView.GetChild(0); // Viewport
            Transform assetsContent = viewport.GetChild(0); // AssetsContent

            if (assetsContent != null)
            {
                // Instantiate the button prefab in the UI
                GameObject newButton = Instantiate(_ButtonAssetPrefab, assetsContent);

                // Set the button's text to the loaded asset's name
                newButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = loadedAsset.name;
                Button button = newButton.GetComponent<Button>();
                button.name = loadedAsset.name;

                // Add the button to the dictionary
                if (!_Buttons.ContainsKey(button.name))
                {
                    _Buttons[button.name] = new List<Button>();
                }
                _Buttons[button.name].Add(button);

                // Attach a listener to the button click event
                button.onClick.AddListener(() => SearchAndReplace(loadedAsset));
            }
        }
    }

    /// <summary>
    /// Search for and replace a preview object in the loaded scene.
    /// </summary>
    private void SearchAndReplace(GameObject _previewObjectPrefab)
    {
        // Set the selected button name
        _ButtonName = _previewObjectPrefab.name;

        // Attempt to find the preview object in the loaded scene
        GameObject foundObject = GameObject.Find(_previewObjectPrefab.name);

        if (foundObject != null)
        {
            // Set the found object as the preview object
            PreviewManager.Instance.SetPreviewObject(_previewObjectPrefab);
            FoundObject = foundObject;
        }
        else
        {
            Debug.LogError("Object not found in the loaded scene.");
        }
    }
}
