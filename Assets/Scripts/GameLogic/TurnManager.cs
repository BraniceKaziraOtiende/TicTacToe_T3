/// <summary>
/// Manages turn switching between players.
/// Pure C# class with no Unity dependencies.
/// </summary>
public class TurnManager
{
    public CellState CurrentPlayer { get; private set; }

    public TurnManager()
    {
        CurrentPlayer = CellState.X; // X always starts
    }

    /// <summary>
    /// Switch to the next player's turn
    /// </summary>
    public void SwitchTurn()
    {
        CurrentPlayer = CurrentPlayer == CellState.X ? CellState.O : CellState.X;
    }

    /// <summary>
    /// Get the opposite player of the current one
    /// </summary>
    public CellState GetOpponentPlayer()
    {
        return CurrentPlayer == CellState.X ? CellState.O : CellState.X;
    }

    /// <summary>
    /// Reset to starting player (X)
    /// </summary>
    public void Reset()
    {
        CurrentPlayer = CellState.X;
    }

    /// <summary>
    /// Check if it's a specific player's turn
    /// </summary>
    public bool IsPlayerTurn(CellState player)
    {
        return CurrentPlayer == player;
    }
}