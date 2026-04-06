using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using HangmanGame.Models;

namespace HangmanGame.Services
{
    public class GameService
    {
        private readonly string _filePath = "gamesaves.json";

        private List<GameSave> GetAllSaves()
        {
            if (!File.Exists(_filePath))
            {
                return new List<GameSave>();
            }

            string json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<GameSave>>(json) ?? new List<GameSave>();
        }

        public List<GameSave> LoadSavesForUser(string userName)
        {
            var allSaves = GetAllSaves();
            return allSaves.Where(s => s.UserName == userName).ToList();
        }

        public void SaveCurrentGame(GameSave newSave)
        {
            var allSaves = GetAllSaves();
            allSaves.Add(newSave); 

            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(allSaves, options);
            File.WriteAllText(_filePath, json);
        }
    }
}