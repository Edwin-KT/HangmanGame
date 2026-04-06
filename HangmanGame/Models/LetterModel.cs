using HangmanGame.ViewModels;

namespace HangmanGame.Models
{
    public class LetterModel : BaseViewModel
    {
        public char Character { get; set; }

        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get => _isEnabled;
            set { _isEnabled = value; OnPropertyChanged(); }
        }

        public LetterModel(char character)
        {
            Character = character;
        }
    }
}