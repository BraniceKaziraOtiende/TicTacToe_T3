using UnityEngine;

/// <summary>
/// Singleton manager to store and persist game settings across scenes.
/// Uses DontDestroyOnLoad to maintain settings between scene transitions.
/// </summary>
public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    // Game Mode Settings
    public enum GameMode
    {
        HumanVsHuman = 0,
        HumanVsAI = 1
    }

    public GameMode CurrentGameMode { get; set; } = GameMode.HumanVsHuman;

    // AI Difficulty (0 = Easy, 1 = Medium, 2 = Hard)
    // Note: For this assignment, we use one AI. Difficulty could affect delay time.
    public int AIDifficulty { get; set; } = 1;

    // Sound Settings
    public bool SoundEnabled { get; set; } = true;

    // Additional Settings
    public Color PlayerXColor { get; set; } = Color.blue;
    public Color PlayerOColor { get; set; } = Color.red;
    public string PlayerXName { get; set; } = "Player X";
    public string PlayerOName { get; set; } = "Player O";

    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadSettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Check if AI mode is enabled
    /// </summary>
    public bool IsAIMode()
    {
        return CurrentGameMode == GameMode.HumanVsAI;
    }

    /// <summary>
    /// Get AI thinking delay based on difficulty (for visual effect)
    /// </summary>
    public float GetAIDelay()
    {
        switch (AIDifficulty)
        {
            case 0: return 0.3f; // Easy - fast
            case 1: return 0.7f; // Medium
            case 2: return 1.2f; // Hard - slower (appears to "think")
            default: return 0.7f;
        }
    }

    /// <summary>
    /// Save settings to PlayerPrefs
    /// </summary>
    public void SaveSettings()
    {
        PlayerPrefs.SetInt("GameMode", (int)CurrentGameMode);
        PlayerPrefs.SetInt("AIDifficulty", AIDifficulty);
        PlayerPrefs.SetInt("SoundEnabled", SoundEnabled ? 1 : 0);
        PlayerPrefs.SetString("PlayerXName", PlayerXName);
        PlayerPrefs.SetString("PlayerOName", PlayerOName);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Load settings from PlayerPrefs
    /// </summary>
    public void LoadSettings()
    {
        CurrentGameMode = (GameMode)PlayerPrefs.GetInt("GameMode", 0);
        AIDifficulty = PlayerPrefs.GetInt("AIDifficulty", 1);
        SoundEnabled = PlayerPrefs.GetInt("SoundEnabled", 1) == 1;
        PlayerXName = PlayerPrefs.GetString("PlayerXName", "Player X");
        PlayerOName = PlayerPrefs.GetString("PlayerOName", "Player O");
    }

    /// <summary>
    /// Reset to default settings
    /// </summary>
    public void ResetToDefaults()
    {
        CurrentGameMode = GameMode.HumanVsHuman;
        AIDifficulty = 1;
        SoundEnabled = true;
        PlayerXColor = Color.blue;
        PlayerOColor = Color.red;
        PlayerXName = "Player X";
        PlayerOName = "Player O";
        SaveSettings();
    }
}