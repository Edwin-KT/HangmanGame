using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
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

        // Banca noastră de cuvinte pe categorii
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

        public ObservableCollection<LetterModel> Keyboard { get; set; }
        public ICommand GuessLetterCommand { get; }

        public GameViewModel(User selectedUser)
        {
            CurrentUser = selectedUser;
            Keyboard = new ObservableCollection<LetterModel>();
            GuessLetterCommand = new RelayCommand(ExecuteGuessLetter);

            StartNewLevel("Cars"); 
        }

        private void StartNewLevel(string category)
        {
            var random = new Random();
            var words = _wordBank[category];
            _hiddenWord = words[random.Next(words.Count)];

            DisplayedWord = string.Join(" ", _hiddenWord.Select(c => "_"));

            Mistakes = 0; 
            ResetKeyboard(); 
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
                }
                else
                {
                    Mistakes++;
                }
            }
        }
    }
}