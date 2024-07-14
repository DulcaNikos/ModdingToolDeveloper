using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class ModListUI : MonoBehaviour
{
    /// <summary> Singleton instance of this script. </summary>
    public static ModListUI Instance;

    /// <summary> Parent transform for mod buttons. </summary>
    [SerializeField, Tooltip("Parent transform for mod buttons.")]
    private Transform _ModsList;
    /// <summary> Prefab for mod buttons. </summary>
    [SerializeField, Tooltip("Prefab for mod buttons.")]
    private GameObject _ButtonModPrefab;

    /// <summary> Parent transform for assets panels </summary>
    [SerializeField, Tooltip(" Parent transform for assets panels")]
    private Transform _AssetsListPanelParent;
    /// <summary> Prefab for assets panels. </summary>
    [SerializeField, Tooltip("Prefab for assets panels.")]
    private GameObject _AssetsListPanelPrefab;

    /// <summary> Currently active assets panel. </summary>
    private GameObject _ActivePanelInstance = null;
    /// <summary>  Currently selected game object. </summary>
    private GameObject _SelectedGameobject = null;
    /// <summary>  Dictionary to store assets panels instances per mod. </summary>
    private Dictionary<ModPackage, GameObject> _AssetsListPanelsInstances = new Dictionary<ModPackage, GameObject>();
    /// <summary>  Dictionary to store mod button instances per mod. </summary>
    private Dictionary<ModPackage, GameObject> _ModsButtonInstances = new Dictionary<ModPackage, GameObject>();
    /// <summary> Dictionary to store objects to spawn per object name. </summary>
    private Dictionary<string, (ModPackage, GameObject)> _ObjectsToSpawn = new Dictionary<string, (ModPackage, GameObject)>();
    /// <summary>  Dictionary to store selected button names per mod. </summary>
    private Dictionary<ModPackage, string> _SelectedButtonNames = new Dictionary<ModPackage, string>();

    /// <summary>  Public property to access _ObjectsToSpawn dictionary. </summary>
    public Dictionary<string, (ModPackage, GameObject)> ObjectsToSpawn => _ObjectsToSpawn;
    /// <summary> Public property to access _AssetsListPanelParent transform. </summary>
    public Transform AssetsListPanelParent => _AssetsListPanelParent;

    /// <summary> Event raised when mod buttons are created. </summary>
    public event EventHandler ModButtonsCreated;

    void Start()
    {
        // Singleton pattern implementation
        if (Instance != null && Instance != this) { Destroy(this); }
        Instance = this;
    }

    /// <summary>
    /// Creates mod buttons based on installed mods.
    /// </summary>
    public void CreateButtons()
    {
        foreach (ModPackage mod in ModManager.Instance.GetCompatibleMods())
        {
            // Instantiate a new button for compatible mods
            GameObject newButton = Instantiate(_ButtonModPrefab, _ModsList);

            // Set button text and version info
            newButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = mod.Name;
            newButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Version: " + mod.Version;

            // Display compatibility status
            newButton.transform.GetChild(2).GetComponent<TextMeshProUGUI>().color = Color.green;
            newButton.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Compatible";

            // Setup toggle and panel instances
            Toggle toggle = newButton.transform.GetChild(3).GetComponent<Toggle>();
            toggle.interactable = false;
            toggle.isOn = false;

            // Instantiate assets panel and store its instance
            GameObject panelInstance = Instantiate(_AssetsListPanelPrefab, _AssetsListPanelParent);
            panelInstance.SetActive(false);
            _AssetsListPanelsInstances[mod] = panelInstance;

            // Store mod button instance
            _ModsButtonInstances[mod] = newButton;

            // Setup button click listener
            Button button = newButton.GetComponent<Button>();
            button.interactable = true;
            button.onClick.AddListener(() => OnButtonClicked(mod));

            // Setup toggle change listener
            toggle.onValueChanged.AddListener((bool isOn) =>
            {
                OnToggleChanged(toggle, button, mod);
            });
        }

        // Handle incompatible mods
        foreach (ModPackage mod in ModManager.Instance.GetIncompatibleMods())
        {
            // Instantiate a new button for incompatible mods
            GameObject newButton = Instantiate(_ButtonModPrefab, _ModsList);

            // Set button text and version info
            newButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = mod.Name;
            newButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Version: " + mod.Version;

            // Display incompatibility status
            newButton.transform.GetChild(2).GetComponent<TextMeshProUGUI>().color = Color.red;
            newButton.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Incompatible";

            // Disable toggle and make button non-interactable
            newButton.transform.GetChild(3).GetComponent<Toggle>().interactable = false;
            newButton.transform.GetChild(3).GetComponent<Toggle>().isOn = false;
            newButton.GetComponent<Button>().interactable = false;
        }

        // Raise event to signal that mod buttons are created
        ModButtonsCreated?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Handles toggle change event for mod buttons.
    /// </summary>
    private void OnToggleChanged(Toggle _toggle, Button _button, ModPackage _mod)
    {
        _button.interactable = true;
        _toggle.interactable = false;

        // Remove selected object from spawn list and clear selected button name
        if (LoadAssetsFromBundle.Instance.Buttons.ContainsKey(_SelectedButtonNames[_mod]))
        {
            foreach (Button button in LoadAssetsFromBundle.Instance.Buttons[_SelectedButtonNames[_mod]])
            {
                if (button != null)
                {
                    button.interactable = true;

                    if (_ObjectsToSpawn.ContainsKey(button.name))
                    {
                        _ObjectsToSpawn.Remove(button.name);
                    }

                    if (_SelectedButtonNames.ContainsKey(_mod))
                    {
                        _SelectedButtonNames.Remove(_mod);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Handles button click event for mod buttons.
    /// </summary>
    private void OnButtonClicked(ModPackage _mod)
    {
        if (_AssetsListPanelsInstances.TryGetValue(_mod, out GameObject panelInstance))
        {
            // Check if there's another panel currently active and close it.
            if (_ActivePanelInstance != null && _ActivePanelInstance != panelInstance)
            {
                _ActivePanelInstance.SetActive(false);
            }

            // Toggle panel visibility
            panelInstance.SetActive(!panelInstance.activeSelf);

            if (panelInstance.activeSelf)
            {
                _ActivePanelInstance = panelInstance;
                Button button = panelInstance.transform.GetChild(3).GetComponent<Button>();
                button.onClick.AddListener(() => ApllyMod(_mod));

            }
            else
            {
                _ActivePanelInstance = null;
            }
        }

        // Reset preview object every time a mod button is presses.
        PreviewManager.Instance.SetPreviewObject(null);
        LoadAssetsFromBundle.Instance.FoundObject = null;
    }

    /// <summary>
    /// Applies selected mod to the selected object.
    /// </summary>
    private void ApllyMod(ModPackage _mod)
    {
        _SelectedGameobject = LoadAssetsFromBundle.Instance.FoundObject;

        // Check if a selected object exists
        if (_SelectedGameobject != null)
        {
            // Add selected object and mod to spawn dictionary
            if (!_ObjectsToSpawn.ContainsKey(_SelectedGameobject.name))
            {
                _ObjectsToSpawn[_SelectedGameobject.name] = (_mod, _SelectedGameobject);
            }

            // Disable buttons associated with the selected object
            if (LoadAssetsFromBundle.Instance.Buttons.ContainsKey(LoadAssetsFromBundle.Instance.ButtonName))
            {
                foreach (Button button in LoadAssetsFromBundle.Instance.Buttons[LoadAssetsFromBundle.Instance.ButtonName])
                {
                    _SelectedButtonNames[_mod] = button.name;
                    button.interactable = false;
                }
            }

            // Update mod button UI to reflect applied state
            if (_ModsButtonInstances.TryGetValue(_mod, out GameObject buttonMod))
            {
                buttonMod.transform.GetChild(3).GetComponent<Toggle>().SetIsOnWithoutNotify(true);
                buttonMod.transform.GetChild(3).GetComponent<Toggle>().interactable = true;
                buttonMod.GetComponent<Button>().interactable = false;
            }

            // Hide assets panel after mod applied
            if (_AssetsListPanelsInstances.TryGetValue(_mod, out GameObject panelInstance))
            {
                panelInstance.SetActive(false);
            }
        }

    }
}
