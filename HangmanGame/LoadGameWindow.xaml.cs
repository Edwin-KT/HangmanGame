using System.Windows;
using HangmanGame.Models;

namespace HangmanGame
{
    public partial class LoadGameWindow : Window
    {
        // Această proprietate va reține salvarea pe care o alege jucătorul
        public GameSave? SelectedGameSave { get; private set; }

        public LoadGameWindow()
        {
            InitializeComponent();
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            // Dacă a selectat ceva din listă, salvăm selecția și închidem fereastra cu "succes" (true)
            if (SavesListBox.SelectedItem is GameSave save)
            {
                SelectedGameSave = save;
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Te rog să selectezi o salvare din listă!", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Închidem fereastra fără să facem nimic
            DialogResult = false;
        }
    }
}