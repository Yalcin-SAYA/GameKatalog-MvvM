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
using System.Windows.Media;

namespace GameKatalog_MvvM.Views
{
    public partial class SpaceShooterMenuView : Window
    {
        MediaPlayer musik = new MediaPlayer();

        private string _loggedInUsername;
        private string _loggedInUserID;

        public SpaceShooterMenuView(
            string username,
            string userID)
        {
            InitializeComponent();

            _loggedInUsername = username;
            _loggedInUserID = userID;

            // Musik starten

            musik.Open(
                new Uri(
                    "pack://application:,,,/Assets/Sounds/SpaceShooter/menu_music.mp3"));

            musik.Volume = 0.01;

            musik.Play();
        }

        // Testmodus ohne Login

        public SpaceShooterMenuView()
            : this("Gast", null)
        {

        }

        // Spiel starten

        private void PlayButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            musik.Stop();

            SpaceShooterSpielView game =
                new SpaceShooterSpielView(
                    _loggedInUsername,
                    _loggedInUserID);

            game.Show();

            this.Close();
        }

        // Zurück zum Katalog

        private void QuitButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            musik.Stop();

            KatalogView katalog =
                new KatalogView(
                    _loggedInUsername);

            katalog.Show();

            this.Close();
        }
    }
}