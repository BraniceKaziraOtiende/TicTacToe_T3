using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for AI strategy pattern.
/// Allows different AI implementations to be swapped easily.
/// </summary>
public interface IAIStrategy
{
    /// <summary>
    /// Get the AI's next move based on current board state
    /// </summary>
    /// <param name="boardState">Current state of the board</param>
    /// <param name="aiPlayer">Which player the AI is (X or O)</param>
    /// <returns>Index of the cell to play (0-8), or -1 if no valid move</returns>
    int GetAIMove(CellState[] boardState, CellState aiPlayer);
}