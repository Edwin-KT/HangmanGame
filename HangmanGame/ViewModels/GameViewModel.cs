using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using HangmanGame.Models;
using HangmanGame.Services;

namespace HangmanGame.ViewModels
{
    public class GameViewModel : BaseViewModel
    {
        private User _currentUser;
        public User CurrentUser
        {
            get => _currentUser;
            set { _currentUser = value; OnPropertyChanged(); }
        }

        private readonly GameService _gameService = new GameService();
        private string _currentCategory = "Cars"; 

        private readonly Dictionary<string, List<string>> _wordBank = new()
        {
            { "Cars", new List<string> { "BMW", "AUDI", "PORSCHE", "FERRARI", "MAZDA" } },
            { "Movies", new List<string> { "TITANIC", "AVATAR", "INCEPTION", "GLADIATOR" } }
        };

        private string _hiddenWord = string.Empty;

        private string _displayedWord = string.Empty;
        public string DisplayedWord
        {
            get => _displayedWord;
            set { _displayedWord = value; OnPropertyChanged(); }
        }

        private int _mistakes;
        public int Mistakes
        {
            get => _mistakes;
            set
            {
                _mistakes = value;
                OnPropertyChanged();
                HangmanImagePath = $"/Images/hang{_mistakes}.png";
                IsAvatarVisible = _mistakes >= 1;
            }
        }

        private string _hangmanImagePath = string.Empty;
        public string HangmanImagePath
        {
            get => _hangmanImagePath;
            set { _hangmanImagePath = value; OnPropertyChanged(); }
        }

        private bool _isAvatarVisible;
        public bool IsAvatarVisible
        {
            get => _isAvatarVisible;
            set { _isAvatarVisible = value; OnPropertyChanged(); }
        }

        private int _currentLevel = 1;
        public int CurrentLevel
        {
            get => _currentLevel;
            set { _currentLevel = value; OnPropertyChanged(); }
        }

        private int _timeLeft;
        public int TimeLeft
        {
            get => _timeLeft;
            set { _timeLeft = value; OnPropertyChanged(); }
        }

        private DispatcherTimer _timer;
        // ------------------------------

        public ObservableCollection<LetterModel> Keyboard { get; set; }
        public ICommand GuessLetterCommand { get; }
        public ICommand SaveGameCommand { get; }
        public ICommand OpenGameCommand { get; }

        public GameViewModel(User selectedUser)
        {
            CurrentUser = selectedUser;
            Keyboard = new ObservableCollection<LetterModel>();
            GuessLetterCommand = new RelayCommand(ExecuteGuessLetter);
            SaveGameCommand = new RelayCommand(ExecuteSaveGame);
            OpenGameCommand = new RelayCommand(ExecuteOpenGame);

            // Configurăm timer-ul să bată la fiecare 1 secundă
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;

            StartNewLevel("Cars");
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            TimeLeft--; 

            if (TimeLeft <= 0)
            {
                _timer.Stop();
                HandleLoss("Timpul a expirat!"); 
            }
        }

        private void StartNewLevel(string category)
        {
            _currentCategory = category;
            var random = new Random();
            var words = _wordBank[category];
            _hiddenWord = words[random.Next(words.Count)];

            DisplayedWord = string.Join(" ", _hiddenWord.Select(c => "_"));
            Mistakes = 0;
            ResetKeyboard();

            TimeLeft = 30;
            _timer.Start();
        }

        private void ResetKeyboard()
        {
            Keyboard.Clear();
            for (char c = 'A'; c <= 'Z'; c++)
            {
                Keyboard.Add(new LetterModel(c));
            }
        }

        private void ExecuteGuessLetter(object? parameter)
        {
            if (parameter is LetterModel letter && letter.IsEnabled)
            {
                letter.IsEnabled = false;

                if (_hiddenWord.Contains(letter.Character))
                {
                    char[] displayChars = DisplayedWord.Replace(" ", "").ToCharArray();
                    for (int i = 0; i < _hiddenWord.Length; i++)
                    {
                        if (_hiddenWord[i] == letter.Character)
                        {
                            displayChars[i] = letter.Character;
                        }
                    }
                    DisplayedWord = string.Join(" ", displayChars);

                    if (!DisplayedWord.Contains("_"))
                    {
                        _timer.Stop();
                        HandleWin();
                    }
                }
                else
                {
                    Mistakes++;
                    if (Mistakes >= 6)
                    {
                        _timer.Stop();
                        HandleLoss("Ai fost spânzurat!");
                    }
                }
            }
        }

        private void HandleWin()
        {
            if (CurrentLevel == 3)
            {
                MessageBox.Show("FELICITĂRI! Ai ghicit 3 cuvinte la rând și ai câștigat jocul!", "Victorie!");
                CurrentLevel = 1; 
            }
            else
            {
                MessageBox.Show($"Bravo! Ai ghicit cuvântul: {_hiddenWord}. Treci la nivelul următor!", "Nivel Complet");
                CurrentLevel++;
            }

            StartNewLevel("Cars"); 
        }

        private void HandleLoss(string motiv)
        {
            MessageBox.Show($"{motiv} Cuvântul era: {_hiddenWord}.\nNivelurile tale s-au resetat.", "Game Over");
            CurrentLevel = 1;
            StartNewLevel("Cars");
        }

        private void ExecuteSaveGame(object? parameter)
        {
            _timer.Stop();

            var newSave = new GameSave
            {
                UserName = CurrentUser.Name,
                Category = _currentCategory,
                HiddenWord = _hiddenWord,
                DisplayedWord = DisplayedWord,
                Mistakes = Mistakes,
                TimeLeft = TimeLeft,
                CurrentLevel = CurrentLevel,
                SaveDate = DateTime.Now
            };

            _gameService.SaveCurrentGame(newSave);

            MessageBox.Show("Jocul a fost salvat cu succes!", "Salvare", MessageBoxButton.OK, MessageBoxImage.Information);

            _timer.Start();
        }

        private void ExecuteOpenGame(object? parameter)
        {
            _timer.Stop(); 

            var userSaves = _gameService.LoadSavesForUser(CurrentUser.Name);

            if (userSaves.Count == 0)
            {
                MessageBox.Show("Nu ai niciun joc salvat încă!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                _timer.Start();
                return;
            }

            var loadWindow = new LoadGameWindow();

            loadWindow.SavesListBox.ItemsSource = userSaves;

            loadWindow.Owner = Application.Current.MainWindow;

            if (loadWindow.ShowDialog() == true && loadWindow.SelectedGameSave != null)
            {
                var save = loadWindow.SelectedGameSave;

                _currentCategory = save.Category;
                _hiddenWord = save.HiddenWord;
                DisplayedWord = save.DisplayedWord;
                Mistakes = save.Mistakes;
                TimeLeft = save.TimeLeft;
                CurrentLevel = save.CurrentLevel;

                ResetKeyboard();
                foreach (var letter in Keyboard)
                {
                    if (DisplayedWord.Contains(letter.Character))
                    {
                        letter.IsEnabled = false;
                    }
                }

                MessageBox.Show("Jocul a fost încărcat cu succes!", "Succes");
            }

            _timer.Start(); 
        }
    }
}