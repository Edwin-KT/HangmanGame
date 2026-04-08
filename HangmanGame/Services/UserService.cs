using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using HangmanGame.Models;

namespace HangmanGame.Services
{
    public class UserService
    {
        private readonly string _filePath;

        public UserService()
        {
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "users.json");
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
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(users, options);
            File.WriteAllText(_filePath, json);
        }
    }
}