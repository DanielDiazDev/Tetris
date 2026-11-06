# Tetris

A casual 2D Tetris puzzle game made with **Unity 6**.

Move, rotate and drop tetrominoes to clear lines, level up, earn currency, climb the leaderboards, and progress through a smooth UI flow with menus, options, and game-over screens.

<p align="center">
  <img src="https://img.shields.io/badge/Unity-6%2B-232323?style=flat-square&logo=unity" alt="Unity version" />
  <img src="https://img.shields.io/badge/URP-Universal%20Render%20Pipeline-5a5a5a?style=flat-square" alt="URP" />
  <img src="https://img.shields.io/badge/Platform-Android%20%7C%20Standalone-green?style=flat-square" />
</p>

## 🎮 Features

- **Classic Tetris gameplay** — move, rotate, soft drop, hard drop, line clears, and ghost piece preview
- **Level & score system** — score tracking, line goals, and level progression
- **In-game currency** — earn and spend coins with a currency activation flow
- **Player authentication** — PlayFab login, display name, and user data persistence
- **Leaderboards** — global score leaderboards via PlayFab
- **Mobile-ready controls** — touch buttons for move left/right, rotate, and soft drop
- **Audio system** — background music and SFX with mute toggles
- **Smooth UI transitions** — custom UI Node Graph with fade/slide/overlay transitions
- **Particle VFX** — line clear effects, spawns, and UI feedback
- **Editor tools** — custom inspector drawers and prefab screenshot tools
- **Persistent settings** — player preferences for volume, username, and game state

## 📸 Screenshots

Add your screenshots here:

```markdown
| Main Menu | In-Game | Game Over |
|-----------|---------|-----------|
| ![](docs/screenshots/main-menu.png) | ![](docs/screenshots/gameplay.png) | ![](docs/screenshots/game-over.png) |
```

## 🛠️ Tech Stack & Dependencies

**Engine**
- Unity 6 LTS or newer (6000.4.0f1)

**Third-party packages**
- [UniRx](https://github.com/neuecc/UniRx) — reactive extensions for Unity
- PlayFab SDK — authentication, user data, currency, and leaderboards

**Built-in Unity packages used**
- Input System, TextMeshPro, 2D Sprite, Tilemap, Particle System, Timeline, UGUI, Universal Render Pipeline (URP)

## 🗂️ Project Structure

```
Assets/
├── _Project/
│   ├── Arts/               # Sprites, backgrounds, UI art (Violet Theme UI, etc.)
│   ├── Scenes/             # Bootstrap, MainMenu, Game
│   ├── Settings/           # URP renderer and project settings
│   ├── Scripts/            # C# source code
│   │   ├── Core/           # ServiceLocator, installers (Game, MainMenu, Persistence)
│   │   ├── MVC/            # BaseModel, BaseView, BaseController
│   │   ├── SOAP/           # ScriptableObject variables and GameEvents architecture
│   │   ├── Grid/           # Grid system (IGridSystem, GridSystem)
│   │   ├── Pieces/         # Tetromino, TetrominoGenerator, GhostTetrominoController
│   │   ├── Score/          # ScoreSystem, LevelSystem
│   │   ├── GameOver/       # GameOverSystem
│   │   ├── Input/          # InputManager, SwipeDirection
│   │   ├── Sound/          # SoundManager
│   │   ├── Prefs/          # PlayerPrefsManager
│   │   ├── Auth/           # PlayFab login, display name, leaderboards
│   │   ├── PlayFabServices/# User data, currency, title data services
│   │   ├── UI/             # UI controllers & views (score, currency, game over, buttons, toggles)
│   │   ├── UINodeGraph/    # Runtime UI flow system + editor window
│   │   ├── Editor/         # Custom drawers, MVCCreator, PrefabScreenshot
│   │   ├── Utils/          # Extensions, attributes, singletons, results, PlayFab utils
│   │   ├── Predicate/      # Game-over predicates
│   │   └── Test/           # Unit tests
│   └── ...
├── Plugins/                # UniRx
├── Packages/               # Unity package manifest
├── ProjectSettings/
└── UserSettings/
```

## 🚀 Getting Started

### Prerequisites
- [Unity Hub](https://unity.com/download) and Unity Editor **6 LTS or newer**
- The **Universal Render Pipeline** package (included via `Packages/manifest.json`)

### Clone and open

```bash
git clone https://github.com/USERNAME/Tetris.git
cd Tetris
```

1. Open **Unity Hub**.
2. Click **Add** and select the `Tetris` folder.
3. Open the project. Unity will import packages automatically (may take a few minutes on first launch).

### Play
- Open `Assets/_Project/Scenes/Bootstrap.unity` (or `MainMenu.unity`) and press **Play**.
- Scenes available: `Bootstrap`, `MainMenu`, `Game`.

### Build for Android
1. File → Build Settings → select **Android** → Switch Platform.
2. Configure Player Settings (bundle identifier, icons, etc.).
3. Click **Build** (APK/AAB).

> The `.gitignore` excludes `Library/`, `Temp/`, builds, `.csproj`/`.sln` user files, and IDE files. Do not commit generated binaries.

## 🧩 Contributing

Contributions are welcome! To contribute:

1. Fork the repository.
2. Create a feature branch: `git checkout -b feature/your-feature`
3. Commit your changes: `git commit -m "Add some feature"`
4. Push to the branch: `git push origin feature/your-feature`
5. Open a Pull Request.

Please keep code consistent with the existing style and folder structure.

## 🙏 Credits & Assets

- **UI assets:** Violet Theme UI and related UI packs
- **Reactive extensions:** UniRx
- **Backend:** PlayFab (auth, user data, currency, leaderboards)
- **Render pipeline:** Universal Render Pipeline (URP)

## 📫 Contact

Questions or suggestions? Open an issue or contact the repository owner.

---

Made with ❤️ and Unity.