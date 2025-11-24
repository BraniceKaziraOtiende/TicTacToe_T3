using System.Linq;
using UnityEngine;

/// <summary>
/// Represents the state of the Tic-Tac-Toe board.
/// Pure C# class with no Unity dependencies for easy testing.
/// </summary>
public class BoardState
{
    private CellState[] cells = new CellState[9];

    public BoardState()
    {
        // Initialize all cells to Empty
        Reset();
    }

    /// <summary>
    /// Check if a specific cell is empty
    /// </summary>
    public bool IsCellEmpty(int index)
    {
        if (index < 0 || index >= 9)
        {
            Debug.LogError($"BoardState.IsCellEmpty: Invalid index {index}");
            return false;
        }
        return cells[index] == CellState.Empty;
    }

    /// <summary>
    /// Set a cell to a specific state (X or O)
    /// </summary>
    public bool SetCell(int index, CellState state)
    {
        if (index < 0 || index >= 9)
        {
            Debug.LogError($"BoardState.SetCell: Invalid index {index}");
            return false;
        }

        if (!IsCellEmpty(index))
        {
            Debug.LogWarning($"BoardState.SetCell: Cell {index} is not empty (current: {cells[index]})");
            return false;
        }

        cells[index] = state;
        Debug.Log($"BoardState: Cell {index} set to {state}");

        // Debug: Print current board
        PrintBoard();

        return true;
    }

    /// <summary>
    /// Get the state of a specific cell
    /// </summary>
    public CellState GetCell(int index)
    {
        if (index < 0 || index >= 9)
        {
            Debug.LogError($"BoardState.GetCell: Invalid index {index}");
            return CellState.Empty;
        }
        return cells[index];
    }

    /// <summary>
    /// Check if the board is completely filled
    /// </summary>
    public bool IsBoardFull()
    {
        return !cells.Any(c => c == CellState.Empty);
    }

    /// <summary>
    /// Get a copy of the board state (for AI to analyze without modifying original)
    /// </summary>
    public CellState[] GetBoardCopy()
    {
        CellState[] copy = new CellState[9];
        for (int i = 0; i < 9; i++)
        {
            copy[i] = cells[i];
        }
        return copy;
    }

    /// <summary>
    /// Reset the board to empty state
    /// </summary>
    public void Reset()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i] = CellState.Empty;
        }
        Debug.Log("BoardState: All cells reset to Empty");
    }

    /// <summary>
    /// Get the full board array
    /// </summary>
    public CellState[] GetBoard()
    {
        return cells;
    }

    /// <summary>
    /// Debug helper: Print current board state
    /// </summary>
    private void PrintBoard()
    {
        Debug.Log($"Current Board State:");
        Debug.Log($"  [{cells[0]}][{cells[1]}][{cells[2]}]");
        Debug.Log($"  [{cells[3]}][{cells[4]}][{cells[5]}]");
        Debug.Log($"  [{cells[6]}][{cells[7]}][{cells[8]}]");
    }
}

/// <summary>
/// Enum representing the state of each cell
/// </summary>
public enum CellState
{
    Empty = 0,
    X = 1,
    O = 2
}