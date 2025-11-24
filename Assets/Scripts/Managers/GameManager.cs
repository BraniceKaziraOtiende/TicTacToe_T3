using UnityEngine;

/// <summary>
/// Optional: Global game manager for cross-scene functionality.
/// For this assignment, most game logic is in GameController.
/// This can handle global events, analytics, or scene management.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Statistics (optional enhancement)
    public int GamesPlayed { get; private set; }
    public int XWins { get; private set; }
    public int OWins { get; private set; }
    public int Draws { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadStats();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Record a game result
    /// </summary>
    public void RecordGameResult(GameState result, CellState winner)
    {
        GamesPlayed++;

        switch (result)
        {
            case GameState.Won:
                if (winner == CellState.X)
                    XWins++;
                else if (winner == CellState.O)
                    OWins++;
                break;
            case GameState.Draw:
                Draws++;
                break;
        }

        SaveStats();
    }

    /// <summary>
    /// Save statistics to PlayerPrefs
    /// </summary>
    private void SaveStats()
    {
        PlayerPrefs.SetInt("GamesPlayed", GamesPlayed);
        PlayerPrefs.SetInt("XWins", XWins);
        PlayerPrefs.SetInt("OWins", OWins);
        PlayerPrefs.SetInt("Draws", Draws);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Load statistics from PlayerPrefs
    /// </summary>
    private void LoadStats()
    {
        GamesPlayed = PlayerPrefs.GetInt("GamesPlayed", 0);
        XWins = PlayerPrefs.GetInt("XWins", 0);
        OWins = PlayerPrefs.GetInt("OWins", 0);
        Draws = PlayerPrefs.GetInt("Draws", 0);
    }

    /// <summary>
    /// Reset all statistics
    /// </summary>
    public void ResetStats()
    {
        GamesPlayed = 0;
        XWins = 0;
        OWins = 0;
        Draws = 0;
        SaveStats();
    }

    /// <summary>
    /// Get win rate for a player
    /// </summary>
    public float GetWinRate(CellState player)
    {
        if (GamesPlayed == 0) return 0f;

        int wins = player == CellState.X ? XWins : OWins;
        return (float)wins / GamesPlayed * 100f;
    }
}