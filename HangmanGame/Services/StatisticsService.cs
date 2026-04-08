using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using HangmanGame.Models;

namespace HangmanGame.Services
{
    public class StatisticsService
    {
        private readonly string _filePath;

        public StatisticsService()
        {
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "statistics.json");
        }

        public List<UserStatistics> GetAllStatistics()
        {
            if (!File.Exists(_filePath)) return new List<UserStatistics>();
            string json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<UserStatistics>>(json) ?? new List<UserStatistics>();
        }

        public void RecordGame(string userName, string category, bool isWin)
        {
            var stats = GetAllStatistics();
            var userStat = stats.FirstOrDefault(s => s.UserName == userName && s.Category == category);

            if (userStat == null)
            {
                userStat = new UserStatistics { UserName = userName, Category = category, GamesPlayed = 0, GamesWon = 0 };
                stats.Add(userStat);
            }

            userStat.GamesPlayed++;
            if (isWin) userStat.GamesWon++;

            SaveStatistics(stats);
        }

        public void DeleteStatisticsForUser(string userName)
        {
            var stats = GetAllStatistics();
            stats.RemoveAll(s => s.UserName == userName);
            SaveStatistics(stats);
        }

        private void SaveStatistics(List<UserStatistics> stats)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(_filePath, JsonSerializer.Serialize(stats, options));
        }
    }
}