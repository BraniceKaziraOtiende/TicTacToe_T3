using UnityEngine;

/// <summary>
/// Checks for win conditions in Tic-Tac-Toe.
/// Pure C# class with no Unity dependencies.
/// </summary>
public class WinChecker
{
    /// <summary>
    /// All possible winning patterns (8 total: 3 rows, 3 columns, 2 diagonals)
    /// Pattern index corresponds to strikethrough index in UI
    /// </summary>
    private static readonly int[][] WinPatterns = new int[][]
    {
        // Rows
        new int[] {0, 1, 2}, // 0: Top row
        new int[] {3, 4, 5}, // 1: Middle row
        new int[] {6, 7, 8}, // 2: Bottom row
        
        // Columns
        new int[] {0, 3, 6}, // 3: Left column
        new int[] {1, 4, 7}, // 4: Middle column
        new int[] {2, 5, 8}, // 5: Right column
        
        // Diagonals
        new int[] {0, 4, 8}, // 6: Top-left to bottom-right
        new int[] {2, 4, 6}  // 7: Top-right to bottom-left
    };

    /// <summary>
    /// Check if there's a winner on the board
    /// Returns tuple: (hasWinner, winner, patternIndex)
    /// </summary>
    public (bool hasWinner, CellState winner, int patternIndex) CheckWin(CellState[] board)
    {
        if (board == null || board.Length != 9)
        {
            Debug.LogError("WinChecker: Invalid board!");
            return (false, CellState.Empty, -1);
        }

        // Debug: Print current board state
        Debug.Log($"=== Checking Win ===");
        Debug.Log($"Board: [{board[0]}][{board[1]}][{board[2]}]");
        Debug.Log($"       [{board[3]}][{board[4]}][{board[5]}]");
        Debug.Log($"       [{board[6]}][{board[7]}][{board[8]}]");

        for (int i = 0; i < WinPatterns.Length; i++)
        {
            int[] pattern = WinPatterns[i];

            CellState cell0 = board[pattern[0]];
            CellState cell1 = board[pattern[1]];
            CellState cell2 = board[pattern[2]];

            // Debug: Log each pattern check
            Debug.Log($"Pattern {i}: cells [{pattern[0]},{pattern[1]},{pattern[2]}] = [{cell0},{cell1},{cell2}]");

            // Check if all three cells in the pattern match and are not empty
            if (cell0 != CellState.Empty &&
                cell0 == cell1 &&
                cell1 == cell2)
            {
                Debug.Log($"*** WIN DETECTED! Pattern {i}, Winner: {cell0} ***");
                return (true, cell0, i);
            }
        }

        Debug.Log("No winner found");
        return (false, CellState.Empty, -1);
    }

    /// <summary>
    /// Check if the game is a draw (board full, no winner)
    /// </summary>
    public bool IsDraw(CellState[] board, bool hasWinner)
    {
        if (hasWinner) return false;

        // Check if board is full
        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] == CellState.Empty)
                return false;
        }

        return true;
    }

    /// <summary>
    /// Get the specific winning pattern cells
    /// </summary>
    public int[] GetWinPattern(int patternIndex)
    {
        if (patternIndex < 0 || patternIndex >= WinPatterns.Length)
            return null;

        return WinPatterns[patternIndex];
    }
}