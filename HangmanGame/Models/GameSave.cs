using System;
using System.Collections.Generic;

namespace HangmanGame.Models
{
    public class GameSave
    {
        public string UserName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string HiddenWord { get; set; } = string.Empty;
        public string DisplayedWord { get; set; } = string.Empty;
        public int Mistakes { get; set; }
        public int TimeLeft { get; set; }
        public int CurrentLevel { get; set; }
        public DateTime SaveDate { get; set; }

        // Fix: Lista literelor deja apăsate
        public List<char> PressedLetters { get; set; } = new List<char>();

        public GameSave() { }
    }
}