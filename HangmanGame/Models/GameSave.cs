using System;

namespace HangmanGame.Models
{
    public class GameSave
    {
        // Pentru a ști a cui este salvarea
        public string UserName { get; set; } = string.Empty;

        // Datele jocului conform cerințelor
        public string Category { get; set; } = string.Empty;
        public string HiddenWord { get; set; } = string.Empty;
        public string DisplayedWord { get; set; } = string.Empty;
        public int Mistakes { get; set; }
        public int TimeLeft { get; set; }
        public int CurrentLevel { get; set; }

        // Salvăm și o dată calendaristică pentru a le putea ordona mai târziu
        public DateTime SaveDate { get; set; }

        public GameSave() { } // Constructor gol necesar pentru JSON
    }
}