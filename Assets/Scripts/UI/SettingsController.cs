using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Controls the Settings scene.
/// Manages all settings UI elements and saves preferences.
/// </summary>
public class SettingsController : MonoBehaviour
{
    [Header("Game Mode")]
    [SerializeField] private TMP_Dropdown gameModeDropdown;

    [Header("AI Difficulty")]
    [SerializeField] private Slider aiDifficultySlider;
    [SerializeField] private TextMeshProUGUI difficultyValueText;

    [Header("Sound")]
    [SerializeField] private Toggle soundToggle;

    [Header("Player Names (Optional)")]
    [SerializeField] private TMP_InputField playerXNameInput;
    [SerializeField] private TMP_InputField playerONameInput;

    [Header("Navigation Buttons")]
    [SerializeField] private Button backButton;
    [SerializeField] private Button resetButton;

    [Header("Platform-Specific Panels")]
    [SerializeField] private GameObject mobileOnlyPanel;
    [SerializeField] private GameObject pcOnlyPanel;

    private void Start()
    {
        // Make sure SettingsManager exists
        EnsureSettingsManagerExists();

        SetupUI();
        LoadCurrentSettings();
        HandlePlatformSpecificUI();
    }

    /// <summary>
    /// Create SettingsManager if it doesn't exist
    /// </summary>
    private void EnsureSettingsManagerExists()
    {
        if (SettingsManager.Instance == null)
        {
            GameObject settingsManagerObj = new GameObject("SettingsManager");
            settingsManagerObj.AddComponent<SettingsManager>();
            Debug.Log("SettingsManager created automatically");
        }
    }

    /// <summary>
    /// Setup all UI elements and listeners
    /// </summary>
    private void SetupUI()
    {
        // Setup Game Mode Dropdown
        if (gameModeDropdown != null)
        {
            gameModeDropdown.ClearOptions();
            gameModeDropdown.AddOptions(new System.Collections.Generic.List<string>
            {
                "Human vs Human",
                "Human vs AI"
            });
            gameModeDropdown.onValueChanged.AddListener(OnGameModeChanged);
        }

        // Setup AI Difficulty Slider
        if (aiDifficultySlider != null)
        {
            aiDifficultySlider.minValue = 0;
            aiDifficultySlider.maxValue = 2;
            aiDifficultySlider.wholeNumbers = true;
            aiDifficultySlider.onValueChanged.AddListener(OnDifficultyChanged);
        }

        // Setup Sound Toggle
        if (soundToggle != null)
        {
            soundToggle.onValueChanged.AddListener(OnSoundToggled);
        }

        // Setup Player Name Inputs
        if (playerXNameInput != null)
        {
            playerXNameInput.onEndEdit.AddListener(OnPlayerXNameChanged);
        }

        if (playerONameInput != null)
        {
            playerONameInput.onEndEdit.AddListener(OnPlayerONameChanged);
        }

        // Setup Buttons
        if (backButton != null)
        {
            backButton.onClick.AddListener(OnBackClicked);
        }

        if (resetButton != null)
        {
            resetButton.onClick.AddListener(OnResetClicked);
        }
    }

    /// <summary>
    /// Load current settings from SettingsManager and display them
    /// </summary>
    private void LoadCurrentSettings()
    {
        if (SettingsManager.Instance == null)
        {
            Debug.LogWarning("SettingsManager not found!");
            return;
        }

        // Load Game Mode
        if (gameModeDropdown != null)
        {
            gameModeDropdown.value = (int)SettingsManager.Instance.CurrentGameMode;
            gameModeDropdown.RefreshShownValue();
        }

        // Load AI Difficulty
        if (aiDifficultySlider != null)
        {
            aiDifficultySlider.value = SettingsManager.Instance.AIDifficulty;
            UpdateDifficultyText(SettingsManager.Instance.AIDifficulty);
        }

        // Load Sound Setting
        if (soundToggle != null)
        {
            soundToggle.isOn = SettingsManager.Instance.SoundEnabled;
        }

        // Load Player Names
        if (playerXNameInput != null)
        {
            playerXNameInput.text = SettingsManager.Instance.PlayerXName;
        }

        if (playerONameInput != null)
        {
            playerONameInput.text = SettingsManager.Instance.PlayerOName;
        }

        // Update AI section visibility
        UpdateAIDifficultyVisibility();
    }

    /// <summary>
    /// Handle platform-specific UI
    /// </summary>
    private void HandlePlatformSpecificUI()
    {
#if UNITY_ANDROID || UNITY_IOS
        // Show mobile panel, hide PC panel
        if (mobileOnlyPanel != null) mobileOnlyPanel.SetActive(true);
        if (pcOnlyPanel != null) pcOnlyPanel.SetActive(false);
        Debug.Log("Mobile UI activated");
        
#elif UNITY_WEBGL
        // WebGL specific settings
        if (mobileOnlyPanel != null) mobileOnlyPanel.SetActive(false);
        if (pcOnlyPanel != null) pcOnlyPanel.SetActive(false);
        Debug.Log("WebGL UI activated");
        
#else
        // PC/Standalone
        if (mobileOnlyPanel != null) mobileOnlyPanel.SetActive(false);
        if (pcOnlyPanel != null) pcOnlyPanel.SetActive(true);
        Debug.Log("PC UI activated");
#endif
    }

    /// <summary>
    /// Called when game mode changes
    /// </summary>
    private void OnGameModeChanged(int index)
    {
        Debug.Log("Game mode changed to: " + index);

        if (SettingsManager.Instance != null)
        {
            SettingsManager.Instance.CurrentGameMode = (SettingsManager.GameMode)index;
            SettingsManager.Instance.SaveSettings();
        }

        UpdateAIDifficultyVisibility();
    }

    /// <summary>
    /// Show/hide AI difficulty based on game mode
    /// </summary>
    private void UpdateAIDifficultyVisibility()
    {
        bool isAIMode = gameModeDropdown != null && gameModeDropdown.value == 1;

        // Make slider interactive only in AI mode
        if (aiDifficultySlider != null)
        {
            aiDifficultySlider.interactable = isAIMode;
        }

        // Dim the text when disabled
        if (difficultyValueText != null)
        {
            Color textColor = difficultyValueText.color;
            textColor.a = isAIMode ? 1f : 0.4f;
            difficultyValueText.color = textColor;
        }
    }

    /// <summary>
    /// Called when difficulty slider changes
    /// </summary>
    private void OnDifficultyChanged(float value)
    {
        int difficulty = Mathf.RoundToInt(value);
        Debug.Log("Difficulty changed to: " + difficulty);

        if (SettingsManager.Instance != null)
        {
            SettingsManager.Instance.AIDifficulty = difficulty;
            SettingsManager.Instance.SaveSettings();
        }

        UpdateDifficultyText(difficulty);
    }

    /// <summary>
    /// Update the difficulty label text
    /// </summary>
    private void UpdateDifficultyText(int difficulty)
    {
        if (difficultyValueText == null) return;

        switch (difficulty)
        {
            case 0:
                difficultyValueText.text = "Easy";
                break;
            case 1:
                difficultyValueText.text = "Medium";
                break;
            case 2:
                difficultyValueText.text = "Hard";
                break;
            default:
                difficultyValueText.text = "Medium";
                break;
        }
    }

    /// <summary>
    /// Called when sound toggle changes
    /// </summary>
    private void OnSoundToggled(bool isOn)
    {
        Debug.Log("Sound toggled: " + isOn);

        if (SettingsManager.Instance != null)
        {
            SettingsManager.Instance.SoundEnabled = isOn;
            SettingsManager.Instance.SaveSettings();
        }
    }

    /// <summary>
    /// Called when Player X name changes
    /// </summary>
    private void OnPlayerXNameChanged(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
        {
            newName = "Player X";
            if (playerXNameInput != null)
                playerXNameInput.text = newName;
        }

        Debug.Log("Player X name changed to: " + newName);

        if (SettingsManager.Instance != null)
        {
            SettingsManager.Instance.PlayerXName = newName;
            SettingsManager.Instance.SaveSettings();
        }
    }

    /// <summary>
    /// Called when Player O name changes
    /// </summary>
    private void OnPlayerONameChanged(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
        {
            newName = "Player O";
            if (playerONameInput != null)
                playerONameInput.text = newName;
        }

        Debug.Log("Player O name changed to: " + newName);

        if (SettingsManager.Instance != null)
        {
            SettingsManager.Instance.PlayerOName = newName;
            SettingsManager.Instance.SaveSettings();
        }
    }

    /// <summary>
    /// Go back to Main Menu
    /// </summary>
    private void OnBackClicked()
    {
        Debug.Log("Back button clicked - loading MainMenu");
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Reset all settings to defaults
    /// </summary>
    private void OnResetClicked()
    {
        Debug.Log("Reset button clicked");

        if (SettingsManager.Instance != null)
        {
            SettingsManager.Instance.ResetToDefaults();
        }

        // Reload the UI with default values
        LoadCurrentSettings();
    }

    /// <summary>
    /// Handle keyboard input
    /// </summary>
    private void Update()
    {
        // Press Escape to go back (PC only)
#if UNITY_STANDALONE || UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnBackClicked();
        }
#endif
    }
}