namespace HangmanGame.Models
{
    public class User
    {
        public string Name { get; set; } = string.Empty;

        public string ProfileImagePath { get; set; } = string.Empty;

        public User() { }

        public User(string name, string profileImagePath)
        {
            Name = name;
            ProfileImagePath = profileImagePath;
        }
    }
}