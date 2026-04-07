using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HangmanGame.Services
{
    public class WordService
    {
        private readonly string _dataFolderPath = "Data";

        public Dictionary<string, List<string>> LoadWordBank()
        {
            var wordBank = new Dictionary<string, List<string>>();

            if (!Directory.Exists(_dataFolderPath))
            {
                Directory.CreateDirectory(_dataFolderPath);
                return wordBank;
            }

            var txtFiles = Directory.GetFiles(_dataFolderPath, "*.txt");

            foreach (var file in txtFiles)
            {
                string categoryName = Path.GetFileNameWithoutExtension(file);

                var words = File.ReadAllLines(file)
                                .Where(line => !string.IsNullOrWhiteSpace(line))
                                .Select(line => line.Trim().ToUpper())
                                .ToList();

                wordBank[categoryName] = words;
            }

            return wordBank;
        }
    }
}