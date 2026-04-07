using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using HangmanGame.Models;

namespace HangmanGame.Services
{
    public class UserService
    {
        private readonly string _dataFolder;
        private readonly string _filePath;

        public UserService()
        {
            _dataFolder = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Data"));
            _filePath = Path.Combine(_dataFolder, "users.json");
        }

        private void EnsureDirectoryExists()
        {
            if (!Directory.Exists(_dataFolder))
            {
                Directory.CreateDirectory(_dataFolder);
            }
        }

        public List<User> LoadUsers()
        {
            if (!File.Exists(_filePath))
            {
                return new List<User>();
            }

            string json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        }

        public void SaveUsers(List<User> users)
        {
            EnsureDirectoryExists();
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(users, options);
            File.WriteAllText(_filePath, json);
        }
    }
}