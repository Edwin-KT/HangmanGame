using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using HangmanGame.Models;
using HangmanGame.Services;

namespace HangmanGame.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly UserService _userService;

        private ObservableCollection<User> _users;
        public ObservableCollection<User> Users
        {
            get => _users;
            set { _users = value; OnPropertyChanged(); }
        }

        private User? _selectedUser;
        public User? SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private string _newUserName = string.Empty;
        public string NewUserName
        {
            get => _newUserName;
            set { _newUserName = value; OnPropertyChanged(); }
        }

        private ObservableCollection<string> _availableAvatars;
        public ObservableCollection<string> AvailableAvatars
        {
            get => _availableAvatars;
            set { _availableAvatars = value ?? new ObservableCollection<string>(); OnPropertyChanged(); }
        }

        private string _selectedAvatar = string.Empty;
        public string SelectedAvatar
        {
            get => _selectedAvatar;
            set { _selectedAvatar = value ?? string.Empty; OnPropertyChanged(); }
        }

        public ICommand NewUserCommand { get; }
        public ICommand DeleteUserCommand { get; }
        public ICommand PlayCommand { get; }
        public ICommand ExitCommand { get; }

        public LoginViewModel()
        {
            _userService = new UserService();

            var loadedUsers = _userService.LoadUsers();
            Users = new ObservableCollection<User>(loadedUsers);

            LoadAvatars();

            NewUserCommand = new RelayCommand(ExecuteNewUser);
            DeleteUserCommand = new RelayCommand(ExecuteDeleteUser, canExecute => SelectedUser != null);
            PlayCommand = new RelayCommand(ExecutePlay, canExecute => SelectedUser != null);
            ExitCommand = new RelayCommand(ExecuteExit);
        }

        private void LoadAvatars()
        {
            _availableAvatars = new ObservableCollection<string>
            {
                "/Images/avatar1.png",
                "/Images/avatar2.png",
                "/Images/avatar3.png",
                "/Images/avatar4.png",
                "/Images/avatar5.png",
                "/Images/avatar6.png",
            };

            if (_availableAvatars.Any())
            {
                SelectedAvatar = _availableAvatars.First();
            }
        }

        private void ExecuteNewUser(object? parameter)
        {
            if (string.IsNullOrWhiteSpace(NewUserName) || NewUserName.Contains(" "))
            {
                MessageBox.Show("Numele de utilizator trebuie să fie format dintr-un singur cuvânt!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Users.Any(u => u.Name.Equals(NewUserName, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Acest utilizator există deja!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(SelectedAvatar))
            {
                MessageBox.Show("Te rog să alegi un avatar din listă!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newUser = new User(NewUserName, SelectedAvatar);

            Users.Add(newUser);
            _userService.SaveUsers(Users.ToList());

            NewUserName = string.Empty;
            SelectedUser = newUser;
        }

        private void ExecuteDeleteUser(object? parameter)
        {
            if (SelectedUser != null)
            {
                var result = MessageBox.Show($"Sigur vrei să ștergi utilizatorul {SelectedUser.Name}?", "Confirmare", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    Users.Remove(SelectedUser);
                    _userService.SaveUsers(Users.ToList());
                    SelectedUser = null;
                }
            }
        }

        private void ExecutePlay(object? parameter)
        {
            if (SelectedUser != null)
            {
                var gameWindow = new GameWindow(SelectedUser);

                var oldWindow = Application.Current.MainWindow;

                Application.Current.MainWindow = gameWindow;

                gameWindow.Show();

                oldWindow?.Close();
            }
        }
        private void ExecuteExit(object? parameter)
        {
            Application.Current.Shutdown();
        }
    }
}