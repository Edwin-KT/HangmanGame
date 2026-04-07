using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using HangmanGame.Models;

namespace HangmanGame.Services
{
    public class GameService
    {
        private readonly string _dataFolder;
        private readonly string _filePath;

        public GameService()
        {
            // Preluăm calea absolută către folderul unde rulează aplicația
            _dataFolder = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Data"));
            _filePath = Path.Combine(_dataFolder, "gamesaves.json");
        }

        private void EnsureDirectoryExists()
        {
            if (!Directory.Exists(_dataFolder))
            {
                Directory.CreateDirectory(_dataFolder);
            }
        }

        private List<GameSave> GetAllSaves()
        {
            if (!File.Exists(_filePath)) return new List<GameSave>();
            string json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<GameSave>>(json) ?? new List<GameSave>();
        }

        public List<GameSave> LoadSavesForUser(string userName)
        {
            return GetAllSaves().Where(s => s.UserName == userName).ToList();
        }

        public void SaveCurrentGame(GameSave newSave)
        {
            EnsureDirectoryExists();
            var allSaves = GetAllSaves();
            allSaves.Add(newSave);
            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(_filePath, JsonSerializer.Serialize(allSaves, options));
        }

        public void DeleteSavesForUser(string userName)
        {
            EnsureDirectoryExists();
            var allSaves = GetAllSaves();
            allSaves.RemoveAll(s => s.UserName == userName);
            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(_filePath, JsonSerializer.Serialize(allSaves, options));
        }
    }
}