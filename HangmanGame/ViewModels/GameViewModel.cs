using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using HangmanGame.Models;

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

        // --- NOU: Nivelul și Timpul ---
        private int _currentLevel = 1; // Începem de la nivelul 1
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

        public GameViewModel(User selectedUser)
        {
            CurrentUser = selectedUser;
            Keyboard = new ObservableCollection<LetterModel>();
            GuessLetterCommand = new RelayCommand(ExecuteGuessLetter);

            // Configurăm timer-ul să bată la fiecare 1 secundă
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;

            StartNewLevel("Cars");
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            TimeLeft--; // Scădem o secundă

            if (TimeLeft <= 0)
            {
                _timer.Stop();
                HandleLoss("Timpul a expirat!"); // Pierzi dacă ajungi la 0
            }
        }

        private void StartNewLevel(string category)
        {
            var random = new Random();
            var words = _wordBank[category];
            _hiddenWord = words[random.Next(words.Count)];

            DisplayedWord = string.Join(" ", _hiddenWord.Select(c => "_"));
            Mistakes = 0;
            ResetKeyboard();

            // Setăm timpul la 30 de secunde conform cerinței și pornim cronometrul
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

                    // VERIFICĂM DACĂ A CÂȘTIGAT CUVÂNTUL
                    if (!DisplayedWord.Contains("_"))
                    {
                        _timer.Stop();
                        HandleWin();
                    }
                }
                else
                {
                    Mistakes++;
                    // VERIFICĂM DACĂ A PIERDUT (presupunem 6 greșeli maxime)
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
                CurrentLevel = 1; // Resetăm pentru un joc nou
                // Mai târziu aici vom salva statistica de "Joc Câștigat"
            }
            else
            {
                MessageBox.Show($"Bravo! Ai ghicit cuvântul: {_hiddenWord}. Treci la nivelul următor!", "Nivel Complet");
                CurrentLevel++;
            }

            StartNewLevel("Cars"); // Trecem la următorul cuvânt
        }

        private void HandleLoss(string motiv)
        {
            MessageBox.Show($"{motiv} Cuvântul era: {_hiddenWord}.\nNivelurile tale s-au resetat.", "Game Over");
            CurrentLevel = 1; // Resetează nivelurile la 1 dacă pierde [cite: 86, 87]
            StartNewLevel("Cars");
        }
    }
}