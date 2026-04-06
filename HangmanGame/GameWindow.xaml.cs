using System.Windows;
using HangmanGame.Models;
using HangmanGame.ViewModels;

namespace HangmanGame
{
    public partial class GameWindow : Window
    {
        public GameWindow(User selectedUser)
        {
            InitializeComponent();

            DataContext = new GameViewModel(selectedUser);
        }
    }
}