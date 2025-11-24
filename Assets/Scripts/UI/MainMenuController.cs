using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls the Main Menu scene.
/// Handles navigation to Game and Settings scenes.
/// </summary>
public class MainMenuController : MonoBehaviour
{
    [Header("UI Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button statsButton; // Optional

    [Header("Panels (Optional)")]
    [SerializeField] private GameObject statsPanel;

    private void Start()
    {
        SetupButtons();
        HandlePlatformSpecificUI();
    }

    /// <summary>
    /// Setup button listeners
    /// </summary>
    private void SetupButtons()
    {
        if (playButton != null)
            playButton.onClick.AddListener(OnPlayClicked);

        if (settingsButton != null)
            settingsButton.onClick.AddListener(OnSettingsClicked);

        if (quitButton != null)
            quitButton.onClick.AddListener(OnQuitClicked);

        if (statsButton != null)
            statsButton.onClick.AddListener(OnStatsClicked);

        // Hide stats panel initially
        if (statsPanel != null)
            statsPanel.SetActive(false);
    }

    /// <summary>
    /// Handle platform-specific UI elements
    /// </summary>
    private void HandlePlatformSpecificUI()
    {
#if UNITY_WEBGL
        // Hide Quit button on WebGL (can't quit a web game)
        if (quitButton != null)
        {
            quitButton.gameObject.SetActive(false);
        }
#endif

#if UNITY_ANDROID || UNITY_IOS
        // Optional: Adjust layout for mobile
        // Example: Make buttons larger, adjust spacing
        if (playButton != null)
        {
            RectTransform rt = playButton.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(rt.sizeDelta.x * 1.2f, rt.sizeDelta.y * 1.2f);
        }
#endif

#if UNITY_STANDALONE
        // Show keyboard shortcuts hint
        Debug.Log("Press SPACE to start game, ESC to quit");
#endif
    }

    /// <summary>
    /// Load the Game scene
    /// </summary>
    private void OnPlayClicked()
    {
        SceneManager.LoadScene("Game");
    }

    /// <summary>
    /// Load the Settings scene
    /// </summary>
    private void OnSettingsClicked()
    {
        SceneManager.LoadScene("Settings");
    }

    /// <summary>
    /// Quit the application
    /// </summary>
    private void OnQuitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    /// <summary>
    /// Show/Hide statistics panel
    /// </summary>
    private void OnStatsClicked()
    {
        if (statsPanel != null)
        {
            statsPanel.SetActive(!statsPanel.activeSelf);
            UpdateStatsDisplay();
        }
    }

    /// <summary>
    /// Update statistics display (optional feature)
    /// </summary>
    private void UpdateStatsDisplay()
    {
        if (GameManager.Instance != null && statsPanel != null)
        {
            // Find text components in stats panel and update them
            // This is optional - you can add stats UI if you want extra credit
        }
    }

    /// <summary>
    /// Handle keyboard shortcuts (optional enhancement)
    /// </summary>
    private void Update()
    {
#if UNITY_STANDALONE
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnPlayClicked();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnQuitClicked();
        }
#endif
    }
}