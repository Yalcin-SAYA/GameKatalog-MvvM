using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

using GameKatalog_MvvM.Data;
using GameKatalog_MvvM.Models;
using GameKatalog_MvvM.ViewModels;

namespace GameKatalog_MvvM.Views
{
    public partial class SpaceShooterSpielView : Window
    {
        DispatcherTimer gameTimer =
            new DispatcherTimer();

        // Sounds

        MediaPlayer shootSound =
            new MediaPlayer();

        MediaPlayer hintergrundMusik =
            new MediaPlayer();

        bool moveLeft;
        bool moveRight;
        bool moveUp;
        bool moveDown;

        List<Rectangle> itemRemover =
            new List<Rectangle>();

        Random random = new Random();

        int enemyCount = 100;

        int playerSpeed = 10;

        int limit = 50;

        private SpaceShooterViewModel _viewModel;

        // damage is stored in ViewModel now

        int enemySpeed = 10;

        Rect playerHitBox;

        private string _loggedInUsername;

        private string _loggedInUserID;

        public SpaceShooterSpielView(
            string username,
            string userID)
        {
            InitializeComponent();

            _loggedInUsername = username;

            _loggedInUserID = userID;

            _viewModel = new SpaceShooterViewModel(_loggedInUsername, _loggedInUserID);
            this.DataContext = _viewModel;

            gameTimer.Interval =
                TimeSpan.FromMilliseconds(20);

            gameTimer.Tick += GameLoop;

            gameTimer.Start();

            MyCanvas.Focus();

            // Hintergrundbild

            ImageBrush bg =
                new ImageBrush();

            bg.ImageSource =
                new BitmapImage(
                    new Uri(
                        "pack://application:,,,/Assets/Bilder/SpaceShooter/Hintergrund.png"));

            MyCanvas.Background = bg;

            // Player Bild

            ImageBrush playerImage =
                new ImageBrush();

            playerImage.ImageSource =
                new BitmapImage(
                    new Uri(
                        "pack://application:,,,/Assets/Bilder/SpaceShooter/Player.png"));

            player.Fill = playerImage;

            // Shoot Sound

            // PLATZHALTER:
            // Bullet.wav später wieder einfügen

            /*
            shootSound.Open(
                new Uri(
                    "pack://application:,,,/Assets/Sounds/SpaceShooter/Bullet.wav"));

            shootSound.Volume = 0.05;
            */

            // Hintergrundmusik

            // PLATZHALTER:
            // GameMusic.mp3 später wieder einfügen

            /*
            hintergrundMusik.Open(
                new Uri(
                    "pack://application:,,,/Assets/Sounds/SpaceShooter/GameMusic.mp3"));

            hintergrundMusik.Volume = 0.05;

            hintergrundMusik.Play();
            */
        }

        // Testmodus ohne Login

        public SpaceShooterSpielView()
            : this("Gast", null)
        {

        }

        // Taste gedrückt

        private void onKeyDown(
            object sender,
            KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                moveLeft = true;
            }

            if (e.Key == Key.Right)
            {
                moveRight = true;
            }

            if (e.Key == Key.Up)
            {
                moveUp = true;
            }

            if (e.Key == Key.Down)
            {
                moveDown = true;
            }
        }

        // Taste losgelassen

        private void onKeyUp(
            object sender,
            KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                moveLeft = false;
            }

            if (e.Key == Key.Right)
            {
                moveRight = false;
            }

            if (e.Key == Key.Up)
            {
                moveUp = false;
            }

            if (e.Key == Key.Down)
            {
                moveDown = false;
            }

            // Schießen

            if (e.Key == Key.Space)
            {
                Rectangle newBullet =
                    new Rectangle
                    {
                        Tag = "bullet",
                        Height = 20,
                        Width = 5,
                        Fill = Brushes.White,
                        Stroke = Brushes.Red
                    };

                Canvas.SetTop(
                    newBullet,
                    Canvas.GetTop(player)
                    - newBullet.Height);

                Canvas.SetLeft(
                    newBullet,
                    Canvas.GetLeft(player)
                    + player.Width / 2);

                MyCanvas.Children.Add(newBullet);

                // Shoot Sound

                /*
                shootSound.Position =
                    TimeSpan.Zero;

                shootSound.Play();
                */
            }
        }

        // Gegner erstellen

        private void MakeEnemies()
        {
            ImageBrush enemySprite =
                new ImageBrush();

            int enemyType =
                random.Next(1, 5);

            switch (enemyType)
            {
                case 1:

                    enemySprite.ImageSource =
                        new BitmapImage(
                            new Uri(
                                "pack://application:,,,/Assets/Bilder/SpaceShooter/Enemy.png"));

                    break;

                case 2:

                    enemySprite.ImageSource =
                        new BitmapImage(
                            new Uri(
                                "pack://application:,,,/Assets/Bilder/SpaceShooter/Enemy2.png"));

                    break;

                case 3:

                    enemySprite.ImageSource =
                        new BitmapImage(
                            new Uri(
                                "pack://application:,,,/Assets/Bilder/SpaceShooter/Enemy3.png"));

                    break;

                case 4:

                    enemySprite.ImageSource =
                        new BitmapImage(
                            new Uri(
                                "pack://application:,,,/Assets/Bilder/SpaceShooter/Enemy4.png"));

                    break;
            }

            Rectangle newEnemy =
                new Rectangle
                {
                    Tag = "enemy",
                    Height = 50,
                    Width = 56,
                    Fill = enemySprite
                };

            Canvas.SetTop(newEnemy, -100);

            Canvas.SetLeft(
                newEnemy,
                random.Next(30, 430));

            MyCanvas.Children.Add(newEnemy);
        }

        // Haupt GameLoop

        private void GameLoop(
            object sender,
            EventArgs e)
        {
            playerHitBox =
                new Rect(
                    Canvas.GetLeft(player),
                    Canvas.GetTop(player),
                    player.Width,
                    player.Height);

            // Score and damage text bound to ViewModel
            damageText.Content = _viewModel.DamageText;

            enemyCount--;

            // Gegner spawnen

            if (enemyCount < 0)
            {
                MakeEnemies();

                enemyCount = limit;
            }

            // Bewegung links

            if (moveLeft &&
                Canvas.GetLeft(player) > 0)
            {
                Canvas.SetLeft(
                    player,
                    Canvas.GetLeft(player)
                    - playerSpeed);
            }

            // Bewegung rechts

            if (moveRight &&
                Canvas.GetLeft(player)
                + player.Width
                < MyCanvas.ActualWidth)
            {
                Canvas.SetLeft(
                    player,
                    Canvas.GetLeft(player)
                    + playerSpeed);
            }

            // Bewegung oben

            if (moveUp &&
                Canvas.GetTop(player) > 0)
            {
                Canvas.SetTop(
                    player,
                    Canvas.GetTop(player)
                    - playerSpeed);
            }

            // Bewegung unten

            if (moveDown &&
                Canvas.GetTop(player)
                + player.Height
                < MyCanvas.ActualHeight)
            {
                Canvas.SetTop(
                    player,
                    Canvas.GetTop(player)
                    + playerSpeed);
            }

            // Alle Objekte prüfen

            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
                // Bullet Bewegung

                if (x is Rectangle &&
                    (string)x.Tag == "bullet")
                {
                    Canvas.SetTop(
                        x,
                        Canvas.GetTop(x) - 20);

                    Rect bulletHitBox =
                        new Rect(
                            Canvas.GetLeft(x),
                            Canvas.GetTop(x),
                            x.Width,
                            x.Height);

                    // Bullet außerhalb

                    if (Canvas.GetTop(x) < 0)
                    {
                        itemRemover.Add(x);
                    }

                    // Gegner Treffer prüfen

                    foreach (var y in MyCanvas.Children.OfType<Rectangle>())
                    {
                        if (y is Rectangle &&
                            (string)y.Tag == "enemy")
                        {
                            Rect enemyHitBox =
                                new Rect(
                                    Canvas.GetLeft(y),
                                    Canvas.GetTop(y),
                                    y.Width,
                                    y.Height);

                            // Treffer

                            if (bulletHitBox.IntersectsWith(enemyHitBox))
                            {
                                itemRemover.Add(x);

                                itemRemover.Add(y);
                                _viewModel.Score++;
                            }
                        }
                    }
                }

                // Gegner Bewegung

                if (x is Rectangle &&
                    (string)x.Tag == "enemy")
                {
                    Canvas.SetTop(
                        x,
                        Canvas.GetTop(x) + enemySpeed);

                    Rect enemyHitBox =
                        new Rect(
                            Canvas.GetLeft(x),
                            Canvas.GetTop(x),
                            x.Width,
                            x.Height);

                    // Gegner unten angekommen

                    if (Canvas.GetTop(x) > 700)
                    {
                        itemRemover.Add(x);

                        _viewModel.IncreaseDamage(10);
                    }

                    // Spieler getroffen

                    if (playerHitBox.IntersectsWith(enemyHitBox))
                    {
                        itemRemover.Add(x);

                        _viewModel.IncreaseDamage(5);
                    }
                }
            }

            // Schwierigkeit erhöhen

            if (_viewModel.Score > 5)
            {
                limit = 20;
            }

            if (_viewModel.Score > 20)
            {
                enemySpeed = 15;
            }

            if (_viewModel.Score > 30)
            {
                enemySpeed = 20;

                playerSpeed = 15;
            }

            // Game Over

            if (_viewModel.Damage >= 100)
            {
                gameTimer.Stop();

                _viewModel.SaveScore();

                // Hintergrundmusik stoppen

                /*
                hintergrundMusik.Stop();
                */

                MessageBoxResult result =
                    MessageBox.Show(
                        "Game Over!\n\nNeu starten?",
                        "Verloren",
                        MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    ResetGame();
                }
                else
                {
                    SpaceShooterMenuView menu =
                        new SpaceShooterMenuView(
                            _loggedInUsername,
                            _loggedInUserID);

                    menu.Show();

                    this.Close();
                }
            }

            // Objekte entfernen

            foreach (Rectangle x in itemRemover)
            {
                MyCanvas.Children.Remove(x);
            }

            itemRemover.Clear();
        }

            // Score persistence moved to SpaceShooterViewModel

        // Spiel zurücksetzen

        private void ResetGame()
        {
            _viewModel.ResetState();

            limit = 50;

            enemySpeed = 10;

            playerSpeed = 10;

            List<UIElement> removeItems =
                new List<UIElement>();

            // Gegner und Bullets entfernen

            foreach (var item in MyCanvas.Children)
            {
                if (item is Rectangle rect
                    && rect.Tag != null)
                {
                    if ((string)rect.Tag == "enemy"
                        || (string)rect.Tag == "bullet")
                    {
                        removeItems.Add(rect);
                    }
                }
            }

            foreach (var item in removeItems)
            {
                MyCanvas.Children.Remove(item);
            }

            // Spieler zurücksetzen

            Canvas.SetLeft(player, 222);

            Canvas.SetTop(player, 495);

            // scoreText and damageText bound to ViewModel
            damageText.Content = _viewModel.DamageText;

            // Musik neu starten

            /*
            hintergrundMusik.Position =
                TimeSpan.Zero;

            hintergrundMusik.Play();
            */

            gameTimer.Start();

            MyCanvas.Focus();
        }
    }
}