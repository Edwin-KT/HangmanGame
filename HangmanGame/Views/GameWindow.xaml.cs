using HangmanGame.Models;
using HangmanGame.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace HangmanGame
{
    public partial class GameWindow : Window
    {
        public GameWindow(User selectedUser)
        {
            InitializeComponent();

            DataContext = new GameViewModel(selectedUser);

            this.Focus();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.A && e.Key <= Key.Z)
            {
                var vm = DataContext as ViewModels.GameViewModel;

                if (vm != null)
                {
                    vm.GuessLetterFromKeyCommand.Execute(e.Key.ToString());
                }
            }
        }
    }
}