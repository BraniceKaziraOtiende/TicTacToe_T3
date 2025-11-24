/// <summary>
/// Wrapper class for AI player functionality.
/// Manages AI strategy and provides interface for game controller.
/// </summary>
public class AIPlayer
{
    private IAIStrategy strategy;
    public CellState PlayerSymbol { get; private set; }

    public AIPlayer(CellState playerSymbol, IAIStrategy aiStrategy = null)
    {
        PlayerSymbol = playerSymbol;
        strategy = aiStrategy ?? new RuleBasedAIStrategy();
    }

    /// <summary>
    /// Get the AI's next move
    /// </summary>
    public int GetMove(CellState[] boardState)
    {
        return strategy.GetAIMove(boardState, PlayerSymbol);
    }

    /// <summary>
    /// Change the AI strategy at runtime (Strategy Pattern)
    /// </summary>
    public void SetStrategy(IAIStrategy newStrategy)
    {
        strategy = newStrategy;
    }

    /// <summary>
    /// Check if this AI player matches the given cell state
    /// </summary>
    public bool IsAITurn(CellState currentPlayer)
    {
        return currentPlayer == PlayerSymbol;
    }
}