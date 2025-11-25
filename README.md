# Tic-Tac-Toe Game - Unity Project

A fully-featured Tic-Tac-Toe game built in Unity with AI opponent, cross-platform support, and clean architecture following OOP principles.

## 🎮 Features

- **Two Game Modes**: Human vs Human and Human vs AI
- **Smart AI Opponent**: Rule-based AI that makes it challenging for you to win
- **Cross-Platform**: PC, WebGL, and Mobile (Android/iOS) builds
- **Settings System**: Customizable game mode, AI difficulty, sound effects, and player names
- **Clean Architecture**: Separation of game logic from UI using design patterns
- **Unit Tests**: NUnit tests for core game logic
- **Platform-Specific UI**: Conditional compilation for different platforms

---


## 🎯 How to Run the Game

### **In Unity Editor**

1. **Clone the repository:**
   ```bash
   git clone # Tic-Tac-Toe Game - Unity Project

A fully-featured Tic-Tac-Toe game built in Unity with AI opponent, cross-platform support, and clean architecture following OOP principles.

## 🎮 Features

- **Two Game Modes**: Human vs Human and Human vs AI
- **Smart AI Opponent**: Rule-based AI that never loses
- **Cross-Platform**: PC, WebGL, and Mobile (Android/iOS) builds
- **Settings System**: Customizable game mode, AI difficulty, sound effects, and player names
- **Clean Architecture**: Separation of game logic from UI using design patterns
- **Unit Tests**: NUnit tests for core game logic
- **Platform-Specific UI**: Conditional compilation for different platforms

---

## 📁 Project Structure

```
Assets/
├── Scripts/
│   ├── GameLogic/          # Pure C# game logic (no Unity dependencies)
│   │   ├── BoardState.cs
│   │   ├── TurnManager.cs
│   │   ├── WinChecker.cs
│   │   └── GameStateData.cs
│   ├── AI/                 # AI implementation using Strategy Pattern
│   │   ├── IAIStrategy.cs
│   │   ├── RuleBasedAIStrategy.cs
│   │   └── AIPlayer.cs
│   ├── UI/                 # Unity UI controllers
│   │   ├── GameController.cs
│   │   ├── MainMenuController.cs
│   │   ├── SettingsController.cs
│   │   └── GridCell.cs
│   ├── Managers/           # Singleton managers
│   │   ├── GameManager.cs
│   │   └── SettingsManager.cs
│   └── Utilities/
│       └── PlatformSpecific.cs
├── Tests/                  # Unit tests
│   └── EditMode/
│       ├── BoardStateTests.cs
│       ├── WinCheckerTests.cs
│       └── AIStrategyTests.cs
├── Scenes/
│   ├── MainMenu.unity
│   ├── Settings.unity
│   └── Game.unity
└── Audio/
    └── SFX/
```

---

## 🏗️ Architecture & Design Patterns

### **Separation of Concerns**

The project follows a clear separation between **game logic** and **UI**:

#### **Pure Logic Layer** (No Unity Dependencies)
- `BoardState`: Manages the 3x3 grid state
- `TurnManager`: Handles turn switching
- `WinChecker`: Detects win/draw conditions
- `GameStateData`: Stores game state information

These classes are pure C# and can be tested independently without Unity.

#### **UI Layer** (Unity MonoBehaviours)
- `GameController`: Coordinates between logic and UI
- `GridCell`: Represents individual cell UI
- `MainMenuController`: Handles main menu navigation
- `SettingsController`: Manages settings UI

---

### **Design Patterns Used**

#### **1. Strategy Pattern** (AI Implementation)
```
IAIStrategy (Interface)
    ↓
RuleBasedAIStrategy (Implementation)
    ↓
AIPlayer (Context)
```

**Why?** Allows different AI strategies to be swapped easily without changing game code.

**Example:**
```csharp
public interface IAIStrategy
{
    int GetAIMove(CellState[] boardState, CellState aiPlayer);
}

// Can easily add new strategies:
// - MinMaxAIStrategy
// - RandomAIStrategy
// - DeepLearningAIStrategy
```

#### **2. Singleton Pattern** (Managers)
```csharp
public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
```

**Why?** Ensures only one instance exists and persists across scenes.

**Used in:**
- `SettingsManager`: Stores game settings
- `GameManager`: Tracks statistics (optional)

#### **3. Observer Pattern** (Event-Driven Updates)
```csharp
// Events notify UI when game state changes
button.onClick.AddListener(OnCellClicked);
```

**Why?** Decouples UI updates from game logic.

---

### **SOLID Principles Applied**

✅ **Single Responsibility**: Each class has one clear purpose
- `BoardState` only manages board state
- `WinChecker` only checks for wins
- `TurnManager` only manages turns

✅ **Open/Closed**: AI strategies can be extended without modifying existing code

✅ **Liskov Substitution**: Any `IAIStrategy` implementation can replace another

✅ **Interface Segregation**: Small, focused interfaces (e.g., `IAIStrategy`)

✅ **Dependency Inversion**: High-level modules (GameController) depend on abstractions (IAIStrategy), not concrete implementations

---

## 🤖 AI Algorithm Explanation

### **Rule-Based AI Strategy**

The AI uses a **priority-based decision tree** to make optimal moves:

```
Decision Priority:
1. WIN     → If AI can win in this move, take it
2. BLOCK   → If opponent can win next move, block it
3. CENTER  → Take center position (index 4) if available
4. CORNER  → Take a corner (0, 2, 6, 8) if available
5. EDGE    → Take an edge (1, 3, 5, 7) if available
6. RANDOM  → Take any remaining empty cell
```

### **How It Works**

#### **Step 1: Check for Winning Move**
```csharp
// Try placing AI's symbol in each empty cell
for (int i = 0; i < 9; i++)
{
    if (board[i] == Empty)
    {
        board[i] = AIPlayer;  // Temporarily place
        if (IsWin(board))     // Check if this wins
            return i;          // Take this move!
        board[i] = Empty;     // Undo
    }
}
```

**Example:**
```
Board:          AI sees:
O | O | _       O | O | O  ← Winning move at index 2!
---------       ---------
X | X | _       X | X | _
---------       ---------
_ | _ | _       _ | _ | _
```

#### **Step 2: Check for Blocking Move**
Same logic, but checking if opponent (X) can win:

```
Board:          AI blocks:
X | X | _       X | X | O  ← Block at index 2
---------       ---------
O | _ | _       O | _ | _
---------       ---------
_ | _ | _       _ | _ | _
```

#### **Step 3: Strategic Positioning**
If no immediate threat:
1. **Center (4)** → Best position for creating multiple win paths
2. **Corners (0,2,6,8)** → Second-best strategic positions
3. **Edges (1,3,5,7)** → Less strategic but still valid

### **Why This AI Never Loses**

✅ Always takes a winning move when available
✅ Always blocks opponent's winning move
✅ Plays strategically when no threats exist
✅ Makes optimal moves every time

The AI can **win or draw**, but **never loses** when playing optimally.

---

## ⚙️ Settings System

Settings are managed through `SettingsManager` (Singleton):

```csharp
SettingsManager.Instance.CurrentGameMode = GameMode.HumanVsAI;
SettingsManager.Instance.AIDifficulty = 1; // 0=Easy, 1=Medium, 2=Hard
SettingsManager.Instance.SoundEnabled = true;
SettingsManager.Instance.SaveSettings(); // Persists to PlayerPrefs
```

Settings persist across:
- Scene changes (using `DontDestroyOnLoad`)
- Game sessions (using `PlayerPrefs`)

---

## 🎯 How to Run the Game

### **In Unity Editor**

1. **Clone the repository:**
   ```bash
   git clone https://github.com/BraniceKaziraOtiende/TicTacToe_T3.git
   ```

2. **Open in Unity:**
   - Unity version: **2021.3 LTS** or newer
   - Open Unity Hub → Add → Select project folder

3. **Open MainMenu scene:**
   - Navigate to `Assets/Scenes/MainMenu.unity`
   - Double-click to open

4. **Press Play** ▶️
   - Click "Settings" to configure game mode
   - Click "Play" to start the game

### **Building the Game**

#### **PC Build (Windows/Mac/Linux)**
1. File → Build Settings
2. Select **PC, Mac & Linux Standalone**
3. Click **Build**
4. Run the `.exe` file

#### **WebGL Build**
1. File → Build Settings
2. Select **WebGL**
3. Click **Build**
4. Upload to Unity Play, itch.io, or your web server
5. Access via browser

#### **Mobile Build (Android)**
1. File → Build Settings
2. Select **Android**
3. Player Settings → Set package name
4. Click **Build**
5. Install `.apk` on Android device

---

## 🧪 Running Unit Tests

1. **Open Test Runner:**
   - Window → General → Test Runner

2. **Run Tests:**
   - Click "Run All" in EditMode tab

3. **Tests Included:**
   - `BoardStateTests`: Board state management (8 tests)
   - `WinCheckerTests`: Win detection for all patterns (12 tests)
   - `AIStrategyTests`: AI decision-making (13 tests)

**Total: 33 unit tests** covering core game logic

---

## 🎮 How to Play

### **Game Modes**

#### **Human vs Human**
1. Player X goes first
2. Players alternate turns
3. Click any empty cell to place your mark
4. First to get 3 in a row wins!

#### **Human vs AI**
1. You play as X (always go first)
2. AI plays as O
3. AI responds automatically after your move
4. Try to beat the AI! (Good luck 😉)

### **Controls**

**Mouse/Touch:**
- Click/Tap cells to play
- Click "Play Again" to restart
- Click "Main Menu" to return home

**Keyboard (PC Only):**
- ESC - Return to menu
- R - Reload game (when available)

---

## 🌐 Platform-Specific Features

The game uses **Unity Conditional Compilation** to adapt to different platforms:

```csharp
#if UNITY_ANDROID || UNITY_IOS
    // Mobile-specific code
    mobilePanel.SetActive(true);
#elif UNITY_STANDALONE
    // PC-specific code
    quitButton.SetActive(true);
#elif UNITY_WEBGL
    // WebGL-specific code
    quitButton.SetActive(false); // Can't quit browser
#endif
```

**Platform Differences:**
- **Mobile**: Touch-optimized UI, larger buttons
- **PC**: Keyboard shortcuts, quit button
- **WebGL**: No quit button, optimized for browser

---

## 📋 Requirements

- Unity **2021.3 LTS** or newer
- **TextMeshPro** (imported automatically)
- **NUnit Test Framework** (for running tests)
- **Optional**: LeanTween for animations

---

## 🔧 Technical Details

### **Technologies Used**
- Unity Game Engine
- C# Programming Language
- TextMeshPro (UI)
- NUnit (Testing)
- PlayerPrefs (Save System)

### **Key Classes**

| Class | Responsibility |
|-------|---------------|
| `GameController` | Main game coordinator |
| `BoardState` | Manages 3x3 grid data |
| `WinChecker` | Detects win/draw conditions |
| `RuleBasedAIStrategy` | AI decision-making logic |
| `SettingsManager` | Persists game settings |
| `GridCell` | Individual cell UI behavior |

---

## 🐛 Known Issues & Future Improvements

### **Known Issues**
- None currently reported

### **Future Enhancements**
- [ ] Online multiplayer
- [ ] Difficulty levels (AI with intentional mistakes)
- [ ] Undo move functionality
- [ ] Game replay/history
- [ ] Additional AI strategies (MinMax algorithm)
- [ ] Achievements system
- [ ] Leaderboard

---



## 🚀 Quick Start Summary

```bash
# 1. Clone repository
git clone https://github.com/BraniceKaziraOtiende/TicTacToe_T3.git
# 2. Open in Unity 2021.3 LTS or newer

# 3. Open MainMenu scene

# 4. Press Play!
```

**Have fun playing! 🎮**
   ```
