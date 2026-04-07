using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using HangmanGame.Models;

namespace HangmanGame.ViewModels
{
    public class LoadGameViewModel : BaseViewModel
    {
        public ObservableCollection<GameSave> Saves { get; set; }

        private GameSave? _selectedSave;
        public GameSave? SelectedSave
        {
            get => _selectedSave;
            set { _selectedSave = value; OnPropertyChanged(); }
        }

        public ICommand LoadCommand { get; }
        public ICommand CancelCommand { get; }

        public LoadGameViewModel(ObservableCollection<GameSave> saves)
        {
            Saves = saves;
            LoadCommand = new RelayCommand(ExecuteLoad);
            CancelCommand = new RelayCommand(ExecuteCancel);
        }

        private void ExecuteLoad(object? parameter)
        {
            if (SelectedSave == null)
            {
                MessageBox.Show("Te rog să selectezi o salvare din listă!", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (parameter is Window window)
            {
                window.DialogResult = true;
                window.Close();
            }
        }

        private void ExecuteCancel(object? parameter)
        {
            if (parameter is Window window)
            {
                window.DialogResult = false;
                window.Close();
            }
        }
    }
}