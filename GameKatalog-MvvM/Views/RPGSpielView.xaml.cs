using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using GameKatalog_MvvM.Data;
using GameKatalog_MvvM.Models;
using GameKatalog_MvvM.ViewModels;

namespace GameKatalog_MvvM.Views
{
    public partial class RPGSpielView : Window
    {
        int[,] map = new int[20, 20];

        int playerX = 0;

        int playerY = 0;

        private RPGViewModel _viewModel;

        Random rnd = new Random();

        private string _loggedInUsername;

        private string _loggedInUserID;

        // Konstruktor

        public RPGSpielView(
            string username,
            string userID)
        {
            InitializeComponent();

            _loggedInUsername = username;
            _loggedInUserID = userID;

            _viewModel = new RPGViewModel(_loggedInUsername, _loggedInUserID);
            this.DataContext = _viewModel;

            InitializeMap();
            DrawMap();

            // initial binding will show status
        }

        // Testmodus

        public RPGSpielView()
            : this("Gast", null)
        {

        }

        // Map erstellen

        private void InitializeMap()
        {
            // Alles leer

            for (int y = 0; y < 20; y++)
            {
                for (int x = 0; x < 20; x++)
                {
                    map[y, x] = 0;
                }
            }

            // Wände

            int wallCount = 110;

            for (int i = 0; i < wallCount; i++)
            {
                int wx = rnd.Next(20);

                int wy = rnd.Next(20);

                if ((wx == 0 && wy == 0)
                    || (wx == 19 && wy == 19))
                {
                    continue;
                }

                if (map[wy, wx] == 0)
                {
                    map[wy, wx] = 1;
                }
            }

            // Gegner

            int enemyCount = 50;

            for (int i = 0; i < enemyCount; i++)
            {
                int gx = rnd.Next(20);

                int gy = rnd.Next(20);

                if (map[gy, gx] == 0)
                {
                    map[gy, gx] = 2;
                }
            }

            // Zufall Events

            int eventCount = 40;

            for (int i = 0; i < eventCount; i++)
            {
                int ex = rnd.Next(20);

                int ey = rnd.Next(20);

                if (map[ey, ex] == 0)
                {
                    map[ey, ex] = 3;
                }
            }

            // Boss

            while (true)
            {
                int bx = rnd.Next(20);

                int by = rnd.Next(20);

                if (map[by, bx] == 0)
                {
                    map[by, bx] = 6;

                    break;
                }
            }

            // Heilfelder

            for (int i = 0; i < 2; i++)
            {
                while (true)
                {
                    int hx = rnd.Next(20);

                    int hy = rnd.Next(20);

                    if (map[hy, hx] == 0
                        && !(hx == 0 && hy == 0)
                        && !(hx == 19 && hy == 19))
                    {
                        map[hy, hx] = 7;

                        break;
                    }
                }
            }

            // Ziel

            map[19, 19] = 4;

            // Spieler Start

            playerX = 0;

            playerY = 0;

            map[playerY, playerX] = 5;
        }

        // Map zeichnen

        private void DrawMap()
        {
            MapGrid.Children.Clear();

            for (int y = 0; y < 20; y++)
            {
                for (int x = 0; x < 20; x++)
                {
                    Button btn = new Button
                    {
                        IsEnabled = false,

                        Margin = new Thickness(1),

                        Background = Brushes.White
                    };

                    Image img = new Image
                    {
                        Width = 50,

                        Height = 50,

                        Stretch = Stretch.Uniform
                    };

                    switch (map[y, x])
                    {
                        case 0:

                            btn.Background =
                                Brushes.White;

                            btn.Content = null;

                            break;

                        case 1:

                            img.Source =
                                LoadImage("Wand.png");

                            btn.Content = img;

                            break;

                        case 2:

                            img.Source =
                                LoadImage("RPGBoss.png");

                            btn.Content = img;

                            break;

                        case 3:

                            img.Source =
                                LoadImage("Zufall.png");

                            btn.Content = img;

                            break;

                        case 4:

                            img.Source =
                                LoadImage("Ziel.avif");

                            btn.Content = img;

                            break;

                        case 5:

                            img.Source =
                                LoadImage("RPGSpieler.png");

                            btn.Content = img;

                            break;

                        case 6:

                            img.Source =
                                LoadImage("RPGGegner.png");

                            btn.Content = img;

                            break;

                        case 7:

                            img.Source =
                                LoadImage("Heal.png");

                            btn.Content = img;

                            break;
                    }

                    MapGrid.Children.Add(btn);
                }
            }
        }

        // Bild laden

        private BitmapImage LoadImage(
            string fileName)
        {
            return new BitmapImage(
                new Uri(
                    $"pack://application:,,,/Assets/Bilder/RPG/{fileName}"));
        }

        // Tastatur

        private void Window_KeyDown(
            object sender,
            KeyEventArgs e)
        {
            int dx = 0;

            int dy = 0;

            if (e.Key == Key.Up)
            {
                dy = -1;
            }

            else if (e.Key == Key.Down)
            {
                dy = 1;
            }

            else if (e.Key == Key.Left)
            {
                dx = -1;
            }

            else if (e.Key == Key.Right)
            {
                dx = 1;
            }

            MovePlayer(dx, dy);
        }

        // Spieler bewegen

        private void MovePlayer(
            int dx,
            int dy)
        {
            int newX = playerX + dx;

            int newY = playerY + dy;

            // Grenzen

            if (newX < 0
                || newY < 0
                || newX >= 20
                || newY >= 20)
            {
                return;
            }

            // Wand

            if (map[newY, newX] == 1)
            {
                MessageBox.Show(
                    "Hier ist eine Wand.");

                return;
            }

            // Felder prüfen

            switch (map[newY, newX])
            {
                case 0:

                    MoveTo(newX, newY);

                    break;

                case 2:

                    FightEnemy(false);

                    MoveTo(newX, newY);

                    break;

                case 3:

                    HandleQuestionMarkEvent(
                        newX,
                        newY);

                    MoveTo(newX, newY);

                    break;

                case 4:

                    _viewModel.SaveGold();

                    MessageBoxResult win = MessageBox.Show($"Du hast gewonnen!\nGold: {_viewModel.PlayerGold}\nNeustarten?", "RPG", MessageBoxButton.YesNo);

                    if (win == MessageBoxResult.Yes)
                    {
                        InitializeMap();

                        DrawMap();

                        UpdateStatus();
                    }
                    else
                    {
                        RPGMenuView menu =
                            new RPGMenuView(
                                _loggedInUsername,
                                _loggedInUserID);

                        menu.Show();

                        this.Close();
                    }

                    break;

                case 6:

                    FightEnemy(true);

                    MoveTo(newX, newY);

                    break;

                    case 7:

                    _viewModel.Heal(50);

                    MessageBox.Show("Du bekommst 50 HP.");

                    map[newY, newX] = 0;

                    MoveTo(newX, newY);

                    break;
            }

            UpdateStatus();

            // Tod

            if (_viewModel.PlayerHp <= 0)
            {
                _viewModel.SaveGold();

                MessageBoxResult lose = MessageBox.Show($"Du bist gestorben!\nGold: {_viewModel.PlayerGold}\nNeustarten?", "RPG", MessageBoxButton.YesNo);

                if (lose == MessageBoxResult.Yes)
                {
                    InitializeMap();

                    DrawMap();

                    UpdateStatus();
                }
                else
                {
                    RPGMenuView menu =
                        new RPGMenuView(
                            _loggedInUsername,
                            _loggedInUserID);

                    menu.Show();

                    this.Close();
                }
            }
        }

        // Gegner Kampf

        private void FightEnemy(
            bool isBoss)
        {
            int damage =
                isBoss
                ? rnd.Next(15, 31)
                : rnd.Next(5, 16);

            int gold =
                isBoss
                ? rnd.Next(100, 201)
                : rnd.Next(10, 51);


            _viewModel.ApplyDamage(damage);
            _viewModel.AddGold(gold);

            string enemyName =
                isBoss
                ? "Boss"
                : "Gegner";

            MessageBox.Show(
                $"Du kämpfst gegen einen {enemyName}!\n-{damage} HP\n+{gold} Gold");
        }

        // Zufalls Event

        private void HandleQuestionMarkEvent(
            int x,
            int y)
        {
            int choice = rnd.Next(4);

            switch (choice)
            {
                case 0:

                    int goldFound =
                        rnd.Next(20, 61);

                            _viewModel.AddGold(goldFound);

                            MessageBox.Show($"Du findest {goldFound} Gold!");

                    break;

                    case 1:

                    int heal =
                        rnd.Next(10, 31);

                    _viewModel.Heal(heal);

                    MessageBox.Show($"Du heilst {heal} HP!");

                    break;

                case 2:

                    int damage =
                        rnd.Next(5, 16);

                    _viewModel.ApplyDamage(damage);

                    MessageBox.Show($"Falle! -{damage} HP");

                    break;

                case 3:

                    FightEnemy(false);

                    break;
            }

            map[y, x] = 0;
        }

        // Spieler bewegen

        private void MoveTo(
            int newX,
            int newY)
        {
            map[playerY, playerX] = 0;

            playerX = newX;

            playerY = newY;

            map[playerY, playerX] = 5;

            DrawMap();
        }

        // UI aktualisieren

        private void UpdateStatus()
        {
            HpText.Text = _viewModel.HpText;
            GoldText.Text = _viewModel.GoldText;
        }

        // Gold speichern mit EF
        // Persistence moved to RPGViewModel.SaveGold()
    }
}