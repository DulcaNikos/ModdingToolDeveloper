using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuManager : MonoBehaviour
{
    /// <summary> Button to start the game. </summary>
    [SerializeField, Tooltip("Button to start the game.")]
    private Button _StartButton;
    /// <summary> Button to open the mods panel. </summary>
    [SerializeField, Tooltip("Button to open the mods panel.")]
    private Button _ModsButton;
    /// <summary> Button to quit the game. </summary>
    [SerializeField, Tooltip("Button to quit the game.")]
    private Button _QuitButton;
    /// <summary> Button to go back to the start menu.</summary>
    [SerializeField, Tooltip("Button to go back to the start menu.")]
    private Button _BackButton;
    /// <summary> Button to apply selected mods. </summary>
    [SerializeField, Tooltip("Button to apply selected mods.")]
    private Button _ApplyModsButton;

    /// <summary> Name of the game scene to load. </summary>
    [SerializeField, Tooltip("Name of the game scene to load.")]
    private string _GameSceneName = "GameScene";

    /// <summary> Panel containing start menu UI elements. </summary>
    [SerializeField, Tooltip("Panel containing start menu UI elements.")]
    private GameObject _StartMenuPanel;
    /// <summary> Panel containing mods UI elements. </summary>
    [SerializeField, Tooltip("Panel containing mods UI elements.")]
    private GameObject _ModsPanel;

    /// <summary>
    /// Initializes button listeners.
    /// </summary>
    private void Start()
    {
        _StartButton.onClick.AddListener(StartGame);
        _ModsButton.onClick.AddListener(ActivateModsPanel);
        _QuitButton.onClick.AddListener(QuitGame);
        _BackButton.onClick.AddListener(BackButton);
        _ApplyModsButton.onClick.AddListener(ApplyModsButton);
    }

    /// <summary>
    /// Loads the game scene.
    /// </summary>
    private void StartGame()
    {
        SceneManager.LoadScene(_GameSceneName);
    }

    /// <summary>
    /// Activates the mods panel and deactivates the start menu panel.
    /// </summary>
    private void ActivateModsPanel()
    {
        _StartMenuPanel.SetActive(false);
        _ModsPanel.SetActive(true);
    }

    /// <summary>
    /// Quits the application.
    /// </summary>
    private void QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Goes back to the start menu from the mods panel.
    /// </summary>
    private void BackButton()
    {
        _ModsPanel.SetActive(false);
        _StartMenuPanel.SetActive(true);
    }

    /// <summary>
    /// Saves the selected mods information to the json and goes back to the start menu.
    /// </summary>
    private void ApplyModsButton()
    {
        SerializationHelper.SaveDictionary(ModListUI.Instance.ObjectsToSpawn);
        _ModsPanel.SetActive(false);
        _StartMenuPanel.SetActive(true);
    }
}
