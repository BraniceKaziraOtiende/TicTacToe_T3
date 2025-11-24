using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Rule-based AI implementation using a priority system.
/// 
/// AI Algorithm:
/// 1. WIN: If AI can win in this move, take it
/// 2. BLOCK: If opponent can win in their next move, block it
/// 3. CENTER: If center cell (4) is empty, take it (strategic position)
/// 4. CORNER: Take an available corner (0, 2, 6, 8)
/// 5. EDGE: Take any available edge (1, 3, 5, 7)
/// 6. RANDOM: Take any remaining available cell
/// </summary>
public class RuleBasedAIStrategy : IAIStrategy
{
    private WinChecker winChecker;
    private System.Random random;

    public RuleBasedAIStrategy()
    {
        winChecker = new WinChecker();
        random = new System.Random();
    }

    public int GetAIMove(CellState[] boardState, CellState aiPlayer)
    {
        if (boardState == null || boardState.Length != 9)
            return -1;

        CellState opponent = aiPlayer == CellState.X ? CellState.O : CellState.X;

        // 1. Check if AI can WIN in this move
        int winMove = FindWinningMove(boardState, aiPlayer);
        if (winMove != -1)
        {
            return winMove;
        }

        // 2. Check if need to BLOCK opponent's winning move
        int blockMove = FindWinningMove(boardState, opponent);
        if (blockMove != -1)
        {
            return blockMove;
        }

        // 3. Take CENTER if available (index 4)
        if (boardState[4] == CellState.Empty)
        {
            return 4;
        }

        // 4. Take a CORNER (strategic positions)
        int cornerMove = GetRandomFromList(new int[] { 0, 2, 6, 8 }, boardState);
        if (cornerMove != -1)
        {
            return cornerMove;
        }

        // 5. Take an EDGE
        int edgeMove = GetRandomFromList(new int[] { 1, 3, 5, 7 }, boardState);
        if (edgeMove != -1)
        {
            return edgeMove;
        }

        // 6. FALLBACK: Take any available cell (should not reach here)
        return GetRandomAvailableCell(boardState);
    }

    /// <summary>
    /// Find if a player can win in the next move
    /// </summary>
    private int FindWinningMove(CellState[] board, CellState player)
    {
        // Try each empty cell and check if it creates a win
        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] == CellState.Empty)
            {
                // Temporarily place the player's symbol
                board[i] = player;

                // Check if this creates a win
                var result = winChecker.CheckWin(board);

                // Undo the move
                board[i] = CellState.Empty;

                // If this move wins, return it
                if (result.hasWinner && result.winner == player)
                {
                    return i;
                }
            }
        }

        return -1; // No winning move found
    }

    /// <summary>
    /// Get a random available cell from a list of preferred positions
    /// </summary>
    private int GetRandomFromList(int[] positions, CellState[] board)
    {
        // Filter to only empty cells
        var available = positions.Where(pos => board[pos] == CellState.Empty).ToList();

        if (available.Count == 0)
            return -1;

        // Return random from available
        return available[random.Next(available.Count)];
    }

    /// <summary>
    /// Get any random available cell on the board
    /// </summary>
    private int GetRandomAvailableCell(CellState[] board)
    {
        var emptyCells = new List<int>();

        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] == CellState.Empty)
            {
                emptyCells.Add(i);
            }
        }

        if (emptyCells.Count == 0)
            return -1;

        return emptyCells[random.Next(emptyCells.Count)];
    }
}