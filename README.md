# WPF Hangman Game

A desktop Hangman game built with C# and Windows Presentation Foundation (WPF). This project is designed using the Model-View-ViewModel (MVVM) architectural pattern and features user profiles, persistent save states, and a multi-level gameplay loop.

## 🎮 Features

* **Multi-Level Gameplay:** Players must guess 3 consecutive words correctly to win the game, progressing through levels 1 to 3.
* **Category Selection:** Words are drawn dynamically from multiple categories including Cars, Movies, Cities, Rivers, or a randomized "All categories" mode.
* **Time Mechanics:** Each level includes a 30-second countdown timer, adding a layer of difficulty and requiring quick thinking.
* **Persistent Save States:** Users can save their mid-game progress and resume later. 
* **State Management:** The save files retain the hidden word, the remaining time, the current level, mistakes made, and the exact letters already guessed on the virtual keyboard.
* **User Management & Statistics:** The application tracks different user profiles and records wins/losses into a statistics view. User data is serialized and saved locally to a `users.json` file.
* **Dual Input System:** Players can interact with the game using a clickable on-screen virtual keyboard or through physical keyboard bindings.

## 🛠️ Technologies & Architecture

* **Language:** C#
* **Framework:** .NET / WPF (Windows Presentation Foundation)
* **Architecture:** Strict MVVM (Model-View-ViewModel) leveraging `INotifyPropertyChanged` and `RelayCommand` for a clean separation of UI and business logic.
* **Data Storage:** Local JSON serialization (`System.Text.Json`).

## 🚀 Getting Started

1. Clone the repository to your local machine.
2. Open the `.sln` file in Visual Studio.
3. Build the solution to restore any dependencies.
4. Run the application. Data files (like `users.json`) will be generated automatically in the execution directory upon saving.
