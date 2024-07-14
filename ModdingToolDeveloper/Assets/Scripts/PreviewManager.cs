using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class PreviewManager : MonoBehaviour
{
    /// <summary>  Singleton instance of this script. </summary>
    public static PreviewManager Instance;

    /// <summary> Name of the preview scene to load additively. </summary>
    [SerializeField, Tooltip("Name of the preview scene to load additively.")]
    private string _PreviewSceneName = "PreviewScene";
    /// <summary> Name of the placeholder object in the preview scene where preview objects will be instantiated.</summary>
    [SerializeField, Tooltip("Name of the placeholder object in the preview scene where preview objects will be instantiated.")]
    private string _PreviewPlaceholderName = "PreviewPlaceholder";
    /// <summary> Layer number to set for the preview objects. </summary>
    [SerializeField, Tooltip("Layer number to set for the preview objects. ")]
    private int _PreviewLayerNumber = 6;
    /// <summary> Render texture to display the preview.</summary>
    [SerializeField, Tooltip("Render texture to display the preview.")]
    private RenderTexture _RenderTexture;

    /// <summary> The currently active preview object. </summary>
    private GameObject currentPreviewObject;
    /// <summary> The placeholder object in the preview scene. </summary>
    private GameObject previewPlaceholder;

    private void Start()
    {
        if (Instance != null && Instance != this) { Destroy(this); }
        Instance = this;

        // Load the preview scene additively
        StartCoroutine(LoadPreviewScene());
    }

    /// <summary>
    /// Coroutine to load the preview scene additively.
    /// </summary>
    private IEnumerator LoadPreviewScene()
    {
        yield return SceneManager.LoadSceneAsync(_PreviewSceneName, LoadSceneMode.Additive);

        // Find the placeholder object in the loaded scene
        previewPlaceholder = GameObject.Find(_PreviewPlaceholderName);
    }

    /// <summary>
    /// Assigns the render texture to the RawImage components in the assets list panel.
    /// </summary>
    public void AssignTexture(object sender, EventArgs e)
    {
        foreach (Transform child in ModListUI.Instance.AssetsListPanelParent)
        {
            Transform previewRawImageTransform = child.GetChild(2); // Raw Image

            if (previewRawImageTransform != null)
            {
                // Assign the render texture to the Raw Image
                RawImage previewRawImage = previewRawImageTransform.GetComponent<RawImage>();
                previewRawImage.texture = _RenderTexture;
            }
        }
    }

    /// <summary>
    /// Sets the preview object to be displayed in the preview scene.
    /// </summary>
    public void SetPreviewObject(GameObject _previewObjectPrefab)
    {
        // Destroy the current preview object if it exists
        if (currentPreviewObject != null)
        {
            Destroy(currentPreviewObject);
        }

        // Instantiate and set the new preview object
        if (previewPlaceholder != null && _previewObjectPrefab != null)
        {
            currentPreviewObject = Instantiate(_previewObjectPrefab, previewPlaceholder.transform.position, Quaternion.identity, previewPlaceholder.transform);
            currentPreviewObject.layer = _PreviewLayerNumber;
            currentPreviewObject.transform.GetChild(0).gameObject.layer = _PreviewLayerNumber;
        }
    }
}
