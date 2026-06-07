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
        //Buttons in WPF benutzen keine "Click"-Events mehr sondern sind auf LoginBefehl gebunden
        public ICommand LoginBefehl { get; set; }

        //selbe beim erstellen von einem Accaount 
        public ICommand BenutzerErstellenBefehl { get; set; }

        public MainViewModel()
        {
            //drücken vom Button starter die Methode 
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
        //Event damit INotify funtioniert
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            //wird geprüft ob jemand aboniert hat wenn j a- View informirt bei änderungen 
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName));
        }
    }
}