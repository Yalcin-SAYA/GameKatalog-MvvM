using GameKatalog_MvvM.Commands;
using GameKatalog_MvvM.Models;
using GameKatalog_MvvM.Services;
using GameKatalog_MvvM.ViewModels;
using GameKatalog_MvvM.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace GameKatalog_MvvM.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string username;
        private string password;

        public string Username
        {
            get { return username; }

            set
            {
                username = value;

                OnPropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get { return password; }

            set
            {
                password = value;

                OnPropertyChanged(nameof(Password));
            }
        }

        public ICommand LoginBefehl { get; set; }
        public ICommand BenutzerErstellenBefehl { get; set; }

        public MainViewModel()
        {
            LoginBefehl = new ButtonBefehl(Login);

            BenutzerErstellenBefehl =
                new ButtonBefehl(BenutzerErstellen);
        }

        private void Login()
        {
            UserService userService = new UserService();

            User user = userService.GetUser(Username, Password);

            if (user != null)
            {
                MessageBox.Show("Login erfolgreich!");

                KatalogView katalog = new KatalogView(user.Username);

                katalog.Show();
                Application.Current.MainWindow.Close();
            }
            else
            {
                MessageBox.Show("Benutzername oder Passwort falsch!");
            }
        }

        private void BenutzerErstellen()
        {
            UserService userService = new UserService();

            User neuerUser = new User();

            neuerUser.Username = Username;
            neuerUser.Password = Password;
            neuerUser.TotalScore = 0;
            neuerUser.MaxScore = 0;

            userService.CreateUser(neuerUser);

            MessageBox.Show("Benutzer wurde erstellt!");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName));
        }
    }
}