using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GameKatalog_MvvM.Views
{
    public partial class RPGMenuView : Window
    {
        private string _loggedInUsername;

        private string _loggedInUserID;

        // Testmodus

        public RPGMenuView()
            : this("Gast", null)
        {

        }

        // Login Daten übernehmen

        public RPGMenuView(
            string username,
            string userID)
        {
            InitializeComponent();

            _loggedInUsername = username;

            _loggedInUserID = userID;
        }

        // Spiel starten

        private void PlayButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            RPGSpielView gameWindow =
                new RPGSpielView(
                    _loggedInUsername,
                    _loggedInUserID);

            gameWindow.Show();

            this.Close();
        }

        // Zurück zum Katalog

        private void QuitButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            KatalogView katalog =
                new KatalogView(
                    _loggedInUsername);

            katalog.Show();

            this.Close();
        }
    }
}