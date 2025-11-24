using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

/// <summary>
/// Main controller for the Tic-Tac-Toe game.
/// Coordinates between game logic and UI display.
/// Implements Observer pattern through events.
/// </summary>
public class GameController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GridCell[] gridCells; // 9 cells
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI turnText;
    [SerializeField] private Button reloadButton;
    [SerializeField] private GameObject[] strikethroughLines; // 8 lines (3 rows, 3 cols, 2 diagonals)

    [Header("Audio (Optional)")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip moveSound;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip drawSound;

    // Game Logic Components (Pure C# classes)
    private BoardState boardState;
    private TurnManager turnManager;
    private WinChecker winChecker;
    private GameStateData gameStateData;
    private AIPlayer aiPlayer;

    // Game State
    private bool isGameOver = false;
    private bool isAIMode = false;
    private bool isAIThinking = false;

    private void Start()
    {
        InitializeGame();
        SetupUI();
    }

    /// <summary>
    /// Initialize all game logic components
    /// </summary>
    private void InitializeGame()
    {
        // Initialize pure logic classes
        boardState = new BoardState();
        turnManager = new TurnManager();
        winChecker = new WinChecker();
        gameStateData = new GameStateData();

        // Check if AI mode is enabled from settings
        if (SettingsManager.Instance != null)
        {
            isAIMode = SettingsManager.Instance.IsAIMode();
        }

        // Initialize AI if needed (AI always plays as O)
        if (isAIMode)
        {
            aiPlayer = new AIPlayer(CellState.O, new RuleBasedAIStrategy());
        }

        isGameOver = false;
        isAIThinking = false;
    }

    /// <summary>
    /// Setup UI elements and listeners
    /// </summary>
    private void SetupUI()
    {
        // Initialize all grid cells
        for (int i = 0; i < gridCells.Length; i++)
        {
            gridCells[i].Initialize(i);
            gridCells[i].SetClickListener(OnCellClicked);
        }

        // Setup reload button
        if (reloadButton != null)
        {
            reloadButton.onClick.AddListener(ReloadGame);
        }

        // Hide all strikethrough lines initially
        foreach (var line in strikethroughLines)
        {
            if (line != null)
                line.SetActive(false);
        }

        UpdateTurnDisplay();
    }

    /// <summary>
    /// Handle cell click event
    /// </summary>
    private void OnCellClicked(int cellIndex)
    {
        // Ignore clicks if game is over or AI is thinking
        if (isGameOver || isAIThinking)
            return;

        // Ignore clicks on filled cells
        if (!boardState.IsCellEmpty(cellIndex))
            return;

        // In AI mode, ignore clicks when it's AI's turn (O)
        if (isAIMode && turnManager.CurrentPlayer == CellState.O)
            return;

        // Make the move
        MakeMove(cellIndex, turnManager.CurrentPlayer);
    }

    /// <summary>
    /// Execute a move on the board
    /// </summary>
    private void MakeMove(int cellIndex, CellState player)
    {
        // Update board state
        bool moveSuccessful = boardState.SetCell(cellIndex, player);
        if (!moveSuccessful)
            return;

        // Update UI
        gridCells[cellIndex].SetSymbol(player);

        // Play sound
        PlaySound(moveSound);

        // Check for game end conditions
        CheckGameEnd();

        // If game is not over, switch turn
        if (!isGameOver)
        {
            turnManager.SwitchTurn();
            UpdateTurnDisplay();

            // Trigger AI turn if needed
            if (isAIMode && turnManager.CurrentPlayer == CellState.O)
            {
                StartCoroutine(AITurnCoroutine());
            }
        }
    }

    /// <summary>
    /// AI turn with delay for better UX
    /// </summary>
    private IEnumerator AITurnCoroutine()
    {
        isAIThinking = true;

        // Get AI delay from settings
        float delay = SettingsManager.Instance != null
            ? SettingsManager.Instance.GetAIDelay()
            : 0.7f;

        yield return new WaitForSeconds(delay);

        // Get AI move
        int aiMove = aiPlayer.GetMove(boardState.GetBoardCopy());

        if (aiMove != -1)
        {
            MakeMove(aiMove, CellState.O);
        }

        isAIThinking = false;
    }

    /// <summary>
    /// Check if the game has ended (win or draw)
    /// </summary>
    private void CheckGameEnd()
    {
        // Get a copy of the board
        CellState[] boardCopy = boardState.GetBoardCopy();

        // Debug: Log the board state we're checking
        Debug.Log("CheckGameEnd called - Getting board copy...");

        // Check for win
        var winResult = winChecker.CheckWin(boardCopy);

        if (winResult.hasWinner)
        {
            Debug.Log($"HandleWin being called - Winner: {winResult.winner}, Pattern: {winResult.patternIndex}");
            HandleWin(winResult.winner, winResult.patternIndex);
            return;
        }

        // Check for draw
        if (boardState.IsBoardFull())
        {
            Debug.Log("Board is full - HandleDraw being called");
            HandleDraw();
        }
    }

    /// Handle win condition
    
    private void HandleWin(CellState winner, int patternIndex)
    {
        isGameOver = true;
        gameStateData.SetWin(winner, patternIndex);

        // Update title text
        string winnerName = winner == CellState.X ? "Player X" : "Player O";
        if (SettingsManager.Instance != null)
        {
            winnerName = winner == CellState.X
                ? SettingsManager.Instance.PlayerXName
                : SettingsManager.Instance.PlayerOName;
        }

        titleText.text = $"{winnerName} Wins!";
        turnText.text = "Game Over";

        // Show strikethrough
        ShowStrikethrough(patternIndex);

        // Disable all cells
        foreach (var cell in gridCells)
        {
            cell.DisableInteraction();
        }

        // Play win sound
        PlaySound(winSound);

        // Record stats
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RecordGameResult(GameState.Won, winner);
        }
    }

   
    /// Handle draw condition
  
    private void HandleDraw()
    {
        isGameOver = true;
        gameStateData.SetDraw();

        titleText.text = "It's a Draw!";
        turnText.text = "Game Over";

        // Disable all cells
        foreach (var cell in gridCells)
        {
            cell.DisableInteraction();
        }

        // Play draw sound
        PlaySound(drawSound);

        // Record stats
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RecordGameResult(GameState.Draw, CellState.Empty);
        }
    }

    
    /// Display the strikethrough line for winning pattern

    private void ShowStrikethrough(int patternIndex)
    {
        if (patternIndex >= 0 && patternIndex < strikethroughLines.Length)
        {
            if (strikethroughLines[patternIndex] != null)
            {
                strikethroughLines[patternIndex].SetActive(true);

                // Optional: Animate the line
                AnimateStrikethrough(strikethroughLines[patternIndex]);
            }
        }
    }

  
    /// <summary>
    /// Animate strikethrough line appearance
    /// </summary>
    private void AnimateStrikethrough(GameObject line)
    {
        // Simple instant appearance (no animation)
        line.transform.localScale = Vector3.one;
    }
    /// <summary>
    /// Update turn display text
    /// </summary>
    private void UpdateTurnDisplay()
    {
        string currentPlayerName = turnManager.CurrentPlayer == CellState.X
            ? "Player X"
            : "Player O";

        if (SettingsManager.Instance != null)
        {
            currentPlayerName = turnManager.CurrentPlayer == CellState.X
                ? SettingsManager.Instance.PlayerXName
                : SettingsManager.Instance.PlayerOName;
        }

        turnText.text = $"{currentPlayerName}'s Turn";

        // Update title if game hasn't started
        if (boardState.IsBoardFull() == false && !isGameOver)
        {
            titleText.text = "Tic-Tac-Toe";
        }
    }

    /// <summary>
    /// Reload/Reset the game
    /// </summary>
    public void ReloadGame()
    {
        // Reset game logic
        boardState.Reset();
        turnManager.Reset();
        gameStateData.Reset();
        isGameOver = false;
        isAIThinking = false;

        // Reset UI
        foreach (var cell in gridCells)
        {
            cell.Reset();
        }

        // Hide all strikethroughs
        foreach (var line in strikethroughLines)
        {
            if (line != null)
                line.SetActive(false);
        }

        // Reset text
        titleText.text = "Tic-Tac-Toe";
        UpdateTurnDisplay();
    }

    /// <summary>
    /// Play sound effect if enabled
    /// </summary>
    private void PlaySound(AudioClip clip)
    {
        if (audioSource == null || clip == null)
            return;

        bool soundEnabled = SettingsManager.Instance != null
            ? SettingsManager.Instance.SoundEnabled
            : true;

        if (soundEnabled)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    /// <summary>
    /// Return to main menu
    /// </summary>
    public void ReturnToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}