using System.Collections.ObjectModel;
using HangmanGame.Models;
using HangmanGame.Services;

namespace HangmanGame.ViewModels
{
    public class StatisticsViewModel : BaseViewModel
    {
        public ObservableCollection<UserStatistics> Statistics { get; set; }

        public StatisticsViewModel()
        {
            var statService = new StatisticsService();
            Statistics = new ObservableCollection<UserStatistics>(statService.GetAllStatistics());
        }
    }
}