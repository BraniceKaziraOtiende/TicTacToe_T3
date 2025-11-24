/// <summary>
/// Enum representing the overall state of the game
/// </summary>
public enum GameState
{
    Playing,
    Won,
    Draw
}

/// <summary>
/// Data class to hold complete game state information
/// Useful for saving/loading or passing state between scenes
/// </summary>
public class GameStateData
{
    public GameState State { get; set; }
    public CellState Winner { get; set; }
    public int WinPatternIndex { get; set; }
    public CellState CurrentTurn { get; set; }
    public bool IsGameOver { get; set; }

    public GameStateData()
    {
        Reset();
    }

    public void Reset()
    {
        State = GameState.Playing;
        Winner = CellState.Empty;
        WinPatternIndex = -1;
        CurrentTurn = CellState.X;
        IsGameOver = false;
    }

    public void SetWin(CellState winner, int patternIndex)
    {
        State = GameState.Won;
        Winner = winner;
        WinPatternIndex = patternIndex;
        IsGameOver = true;
    }

    public void SetDraw()
    {
        State = GameState.Draw;
        Winner = CellState.Empty;
        WinPatternIndex = -1;
        IsGameOver = true;
    }
}