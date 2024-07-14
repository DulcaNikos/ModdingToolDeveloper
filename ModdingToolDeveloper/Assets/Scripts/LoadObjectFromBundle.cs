using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class LoadObjectFromBundle : MonoBehaviour
{
    /// <summary>
    /// Enum defining various types of custom object types that can be loaded from asset bundles.
    /// </summary>
    private enum CustomObjectType
    {
        Texture,
        Material,
        Mesh,
        Animation,
        Audio,
        ScriptableObject,
        GameObject,
        Sprite,
        Shader,
        Font,
        SceneAsset
    }

    /// <summary> Singleton instance of this script. </summary>
    public static LoadObjectFromBundle Instance;

    private void Awake()
    {
        // Singleton pattern to ensure only one instance exists
        if (Instance != null && Instance != this) { Destroy(this); }
        Instance = this;
    }

    /// <summary>
    /// Replace objects in the scene with assets from loaded bundles.
    /// </summary>
    public void ReplaceObjectsInScene(Dictionary<string, (ModPackage, GameObject)> _objectsToSpawn)
    {
        foreach (KeyValuePair<string, (ModPackage, GameObject)> objToSpawn in _objectsToSpawn)
        {
            StartCoroutine(ReplaceObject(objToSpawn.Value.Item1, objToSpawn.Value.Item2));
        }
    }

    /// <summary>
    /// Coroutine to load and replace a specific object in the scene from an asset bundle.
    /// </summary>
    /// <param name="_mod">ModPackage containing information about the asset bundle and asset name.</param>
    /// <param name="_objectToReplace">GameObject in the scene to be replaced.</param>
    private IEnumerator ReplaceObject(ModPackage _mod, GameObject _objectToReplace)
    {
        // Construct the path to the asset bundle file
        string a = Path.Combine(Application.dataPath, "AssetBundles");
        string b = Path.Combine(a, _mod.BundleName);
        string c = Path.Combine(b, "Bundle");
        string assetBundlePath = Path.Combine(c, _mod.BundleName.ToLower());

        // Load the asset bundle asynchronously
        AssetBundleCreateRequest bundleLoadRequest = AssetBundle.LoadFromFileAsync(assetBundlePath);
        yield return bundleLoadRequest;

        // Check if the asset bundle was loaded successfully
        if (bundleLoadRequest != null && bundleLoadRequest.assetBundle != null)
        {
            AssetBundle assetBundle = bundleLoadRequest.assetBundle;

            // Load the specific asset from the bundle asynchronously
            AssetBundleRequest assetLoadRequest = assetBundle.LoadAssetAsync(_mod.AssetName);
            yield return assetLoadRequest;

            // Check if the asset was loaded successfully
            if (assetLoadRequest != null && assetLoadRequest.asset != null)
            {
                object replacementObject = CustomSwitch(assetLoadRequest);

                if (replacementObject != null)
                {
                    if (_objectToReplace != null)
                    {
                        // Handle the loaded asset based on its type
                        HandleAsset(assetLoadRequest, _objectToReplace);

                        // Unload the asset bundle when done to free up resources
                        assetBundle.Unload(false);
                    }
                    else
                    {
                        Debug.LogError("Existing object not found in scene.");
                    }
                }
                else
                {
                    Debug.LogError("Failed to load replacement object from asset bundle.");
                }
            }
            else
            {
                Debug.LogError("Failed to load asset bundle.");
            }
        }
    }

    /// <summary>
    /// Switch statement to convert AssetBundleRequest.asset into specific object types.
    /// </summary>
    /// <param name="assetLoadRequest">AssetBundleRequest containing the loaded asset.</param>
    /// <returns>The loaded object casted to its appropriate type.</returns>
    private object CustomSwitch(AssetBundleRequest _assetLoadRequest)
    {
        switch (_assetLoadRequest.asset)
        {
            case Texture:
                return _assetLoadRequest.asset as Texture;
            case Material:
                return _assetLoadRequest.asset as Material;
            case Mesh:
                return _assetLoadRequest.asset as Mesh; ;
            case Animation:
                return _assetLoadRequest.asset as Animation;
            case AudioClip:
                return _assetLoadRequest.asset as AudioClip;
            case GameObject:
                return _assetLoadRequest.asset as GameObject;
            case Sprite:
                return _assetLoadRequest.asset as Sprite;
            case Shader:
                return _assetLoadRequest.asset as Shader;
            case Font:
                return _assetLoadRequest.asset as Font;
            default:
                Debug.LogWarning("Unsupported asset type: " + _assetLoadRequest.asset.GetType());
                return null;
        }
    }

    /// <summary>
    /// Handles the loaded asset and replaces the existing GameObject in the scene.
    /// </summary>
    /// <param name="assetLoadRequest">AssetBundleRequest containing the loaded asset.</param>
    /// <param name="objectToReplace">GameObject in the scene to be replaced.</param>
    private void HandleAsset(AssetBundleRequest _assetLoadRequest, GameObject _objectToReplace)
    {
        // Switch statement to handle different asset types and apply them to the objectToReplace
        switch (_assetLoadRequest.asset)
        {
            case Texture texture:
                Renderer renderer0 = _objectToReplace.GetComponent<Renderer>();
                if (renderer0 != null)
                {
                    renderer0.material.mainTexture = texture;
                }
                break;
            case Material material:
                Renderer renderer1 = _objectToReplace.GetComponent<Renderer>();
                if (renderer1 != null)
                {
                    renderer1.material = material;
                }
                break;
            case Mesh mesh:
                MeshFilter meshFilter = _objectToReplace.GetComponent<MeshFilter>();
                if (meshFilter != null)
                {
                    meshFilter.mesh = mesh;
                }
                break;
            case Animation animation:
                Animation animComponent = _objectToReplace.GetComponent<Animation>();
                if (animComponent != null)
                {
                    animComponent.clip = animation.clip;
                }
                break;
            case AudioClip audioClip:
                AudioSource audioSource = _objectToReplace.GetComponent<AudioSource>();
                if (audioSource != null)
                {
                    audioSource.clip = audioClip;
                }
                break;
            case GameObject gameObject:
                Instantiate(gameObject, _objectToReplace.transform.position, _objectToReplace.transform.rotation, _objectToReplace.transform.parent);
                Destroy(_objectToReplace);
                break;
            case Sprite sprite:
                SpriteRenderer spriteRenderer = _objectToReplace.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.sprite = sprite;
                }
                break;
            case Shader shader:
                Renderer renderer2 = _objectToReplace.GetComponent<Renderer>();
                if (renderer2 != null)
                {
                    renderer2.material.shader = shader;
                }
                break;
            case Font font:
                TextMesh textMesh = _objectToReplace.GetComponent<TextMesh>();
                if (textMesh != null)
                {
                    textMesh.font = font;
                }
                break;
            default:
                Debug.LogWarning("Unsupported asset type: " + _assetLoadRequest.asset.GetType());
                break;
        }
    }
}