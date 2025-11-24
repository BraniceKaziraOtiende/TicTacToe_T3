using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Represents a single cell in the Tic-Tac-Toe grid.
/// Handles display and click events for one cell.
/// </summary>
public class GridCell : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI symbolText;
    [SerializeField] private Image backgroundImage;

    [Header("Visual Settings")]
    [SerializeField] private Color emptyColor = Color.white;
    [SerializeField] private Color xColor = new Color(0.3f, 0.6f, 1f); // Light blue
    [SerializeField] private Color oColor = new Color(1f, 0.4f, 0.4f); // Light red

    public int CellIndex { get; private set; }
    private CellState currentState = CellState.Empty;

    /// <summary>
    /// Initialize the cell with its index
    /// </summary>
    public void Initialize(int index)
    {
        CellIndex = index;

        // Get references if not set in inspector
        if (button == null)
            button = GetComponent<Button>();
        if (symbolText == null)
            symbolText = GetComponentInChildren<TextMeshProUGUI>();
        if (backgroundImage == null)
            backgroundImage = GetComponent<Image>();

        Reset();
    }

    /// <summary>
    /// Set up the click listener
    /// </summary>
    public void SetClickListener(System.Action<int> onClickCallback)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClickCallback?.Invoke(CellIndex));
    }

    /// <summary>
    /// Display a symbol (X or O) in this cell
    /// </summary>
    public void SetSymbol(CellState state)
    {
        currentState = state;

        switch (state)
        {
            case CellState.X:
                symbolText.text = "X";
                symbolText.color = xColor;
                button.interactable = false;
                break;

            case CellState.O:
                symbolText.text = "O";
                symbolText.color = oColor;
                button.interactable = false;
                break;

            case CellState.Empty:
                symbolText.text = "";
                button.interactable = true;
                break;
        }

        // Optional: Animate the symbol appearance
        if (state != CellState.Empty)
        {
            AnimateSymbol();
        }
    }

    /// <summary>
    /// Simple scale animation when symbol appears
    /// </summary>
    private void AnimateSymbol()
    {
        // Simple instant appearance (no animation)
        symbolText.transform.localScale = Vector3.one;
    }

    /// <summary>
    /// Highlight this cell (for winning line)
    /// </summary>
    public void Highlight()
    {
        if (backgroundImage != null)
        {
            backgroundImage.color = new Color(1f, 1f, 0.6f, 0.5f); // Yellow tint
        }
    }

    /// <summary>
    /// Reset cell to empty state
    /// </summary>
    public void Reset()
    {
        currentState = CellState.Empty;
        symbolText.text = "";
        button.interactable = true;

        if (backgroundImage != null)
        {
            backgroundImage.color = emptyColor;
        }

        symbolText.transform.localScale = Vector3.one;
    }

    /// <summary>
    /// Get current state of this cell
    /// </summary>
    public CellState GetState()
    {
        return currentState;
    }

    /// <summary>
    /// Disable interaction (for game over state)
    /// </summary>
    public void DisableInteraction()
    {
        button.interactable = false;
    }

    /// <summary>
    /// Enable interaction (for new game)
    /// </summary>
    public void EnableInteraction()
    {
        if (currentState == CellState.Empty)
        {
            button.interactable = true;
        }
    }
}