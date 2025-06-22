# Unity Word Boggle Game

This is a Unity-based word puzzle game inspired by Boggle. Players can form words by dragging across adjacent letters in a grid. 
The game includes two modes: Endless and Level-based gameplay with objectives.

# Game Modes

1. Endless Mode

- A random 4x4 letter grid is generated.
- Players form words by dragging across adjacent letters.
- Words are scored based on length.
- Letters used are removed, and new ones drop in.
- The game continues endlessly.

2. Level Mode

- Levels are defined in a JSON file.
- Each level has a grid layout and specific objectives.
- Objectives can include word count, score, timer, bug collection, or breaking rocks.
- After completing a level, the next one automatically loads.
- Special tiles include bugs (bonus) and rocks (blocked unless cleared by adjacent word matches).

## Project Structure

**MainMenuController.cs**

Handles user input in the main menu. Sets the mode (Endless or Level) and stores the configuration in PlayerPrefs. Then it loads the GamePlay scene.

**GameInitializer.cs**

Called when the GamePlay scene loads. It reads the selected game mode from PlayerPrefs and either starts an endless session or loads level data from PlayerPrefs and starts the first level.

**GameManager.cs**

This is the central controller of the game. It tracks score, timer, word validation, level objectives, and progression. It also handles UI updates and ending levels.

**GridManager.cs**

Responsible for generating and managing the tile grid. In Endless Mode, it creates random letter tiles and ensures a few valid words exist. In Level Mode, it uses the predefined grid from the JSON data.

**InputController.cs**

Manages player input for dragging across letter tiles. Builds the current word path and passes it to the GameManager when a word is completed.

**Tile.cs**

Represents an individual grid tile. Each tile holds a letter, position, type (normal, bug, rock), and has visual logic for highlighting and blocking.

**DictionaryManager.cs**

Loads a wordlist from a text file at startup. Used by the GameManager to check if submitted words are valid.

**LevelLoader.cs**

Parses level data from a JSON file (provided as a TextAsset in the Inspector). Tracks which level the player is currently on and provides access to that level's data.

**LevelData.cs**

Contains data models for level configuration, including grid size, grid tiles (letter and tile type), and objectives like bug count, time, and word count.



## Architecture and Design Decisions

The code is modular and structured around clear responsibilities. Each class has one primary role. 
Here's an overview of the design principles and reasoning:

**Singleton Pattern**

We use singletons selectively where global, scene-independent access is necessary:
- `GameManager` is a singleton because it coordinates gameplay across various systems like input, grid, UI, and scoring.
- `LevelLoader` is used as a singleton to persist level data across scenes, enabling seamless progression from one level to the next.

**Decoupled Initialization**

`GameInitializer` ensures that gameplay starts cleanly regardless of mode. It keeps startup logic separate from the GameManager and allows for easy expansion, such as loading a saved game state.

**Scene-based Flow**

The game starts at the Main Menu scene. Based on player selection, it loads the GamePlay scene and initializes either Endless or Level mode. 
This clear separation allows GamePlay to be self-contained and reusable.

**Data-driven Levels**

Levels are defined in JSON, making them easy to design, test, and expand without touching code. The `LevelData` structure supports tile types, objectives, and variable grid sizes.
