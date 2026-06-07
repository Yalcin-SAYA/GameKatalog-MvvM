using System;
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
    public partial class PacManSpielView : Window
    {
        private PacManViewModel _viewModel;
        DispatcherTimer gameTimer =
            new DispatcherTimer();

        // Score now stored in ViewModel
        Rect pacmanHitBox;

        // Login Daten

        private string _loggedInUsername;

        private string _loggedInUserID;

        // Startpositionen

        const double pacmanStartLeft = 50;

        const double pacmanStartTop = 104;

        const double redGuyStartLeft = 173;

        const double redGuyStartTop = 29;

        const double orangeGuyStartLeft = 651;

        const double orangeGuyStartTop = 104;

        const double pinkGuyStartLeft = 173;

        const double pinkGuyStartTop = 404;

        // Konstruktor

        public PacManSpielView(
            string username,
            string userID)
        {
            InitializeComponent();

            _loggedInUsername = username;

            _loggedInUserID = userID;

            _viewModel = new PacManViewModel(_loggedInUsername, _loggedInUserID);
            this.DataContext = _viewModel;

            GameSetUp();
        }

        // Testmodus

        public PacManSpielView() : this("Gast", null)
        {

        }

        // Setup

        private void GameSetUp()
        {
            MyCanvas.Focus();

            gameTimer.Tick += GameLoop;

            gameTimer.Interval = TimeSpan.FromMilliseconds(20);

            gameTimer.Start();

            // Bilder laden

            pacman.Fill = LoadImage("pacman.jpg");

            redGuy.Fill = LoadImage("red.jpg");

            orangeGuy.Fill = LoadImage("orange.jpg");

            pinkGuy.Fill = LoadImage("pink.jpg");
        }

        // Bilder laden

        private ImageBrush LoadImage( string fileName)
        {
            ImageBrush imageBrush = new ImageBrush();

            imageBrush.ImageSource = new BitmapImage(new Uri($"pack://application:,,,/Assets/Bilder/PacMan/{fileName}"));

            return imageBrush;
        }

        // Bewegung Pfeiltasten 

        private void CanvasKeyDown(object sender,KeyEventArgs e)
        {
            if (e.Key == Key.Left && !_viewModel.NoLeft)
            {
                _viewModel.MoveLeft();
                pacman.RenderTransform = new RotateTransform(-180, pacman.Width / 2, pacman.Height / 2);
            }

            if (e.Key == Key.Right && !_viewModel.NoRight)
            {
                _viewModel.MoveRight();
                pacman.RenderTransform = new RotateTransform(0, pacman.Width / 2, pacman.Height / 2);
            }

            if (e.Key == Key.Up && !_viewModel.NoUp)
            {
                _viewModel.MoveUp();
                pacman.RenderTransform = new RotateTransform(-90, pacman.Width / 2, pacman.Height / 2);
            }

            if (e.Key == Key.Down && !_viewModel.NoDown)
            {
                _viewModel.MoveDown();
                pacman.RenderTransform = new RotateTransform(90, pacman.Width / 2, pacman.Height / 2);
            }
        }

        // Haupt Spiel Loop

        private void GameLoop(object sender,EventArgs e)
        {

            // Positionen aktulasieren mit ViewModel Tick
            _viewModel.Tick();

            Canvas.SetLeft(pacman, _viewModel.PacmanLeft);
            Canvas.SetTop(pacman, _viewModel.PacmanTop);

            Canvas.SetLeft(redGuy, _viewModel.RedGuyLeft);
            Canvas.SetTop(redGuy, _viewModel.RedGuyTop);

            Canvas.SetLeft(orangeGuy, _viewModel.OrangeGuyLeft);
            Canvas.SetTop(orangeGuy, _viewModel.OrangeGuyTop);

            Canvas.SetLeft(pinkGuy, _viewModel.PinkGuyLeft);
            Canvas.SetTop(pinkGuy, _viewModel.PinkGuyTop);

            // Hitbox

            pacmanHitBox = new Rect( Canvas.GetLeft(pacman), Canvas.GetTop(pacman), pacman.Width, pacman.Height);

            // Alle Elemente prüfen

            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
                Rect hitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x),x.Width, x.Height);

                // Wände

                if ((string)x.Tag == "wall")
                {
                    CheckWallCollision( x, hitBox);
                }

                // Coins

                if ((string)x.Tag == "coin")
                {
                    if (pacmanHitBox.IntersectsWith(hitBox) && x.Visibility == Visibility.Visible)
                    {
                        x.Visibility = Visibility.Hidden;
                        _viewModel.Score++;
                    }
                }

                // Geister

                if ((string)x.Tag == "ghost")
                {
                    CheckGhostCollision(
                        x,
                        hitBox);
                }
            }

            // Gewinn

            if (_viewModel.Score == 85)
            {
                _viewModel.SaveScore();

                gameTimer.Stop();

                MessageBox.Show("Level 1 geschafft!");

                PacManSpielLV2View lv2 = new PacManSpielLV2View(_loggedInUsername, _loggedInUserID);
                lv2.Show();
                this.Close();
            }
        }

        // Spielfeld Grenzen

        private void CheckBorders()
        {
            // Border in Model prüfen
        }

        // Wand Kollision

        private void CheckWallCollision(
            Rectangle wall,
            Rect hitBox)
        {
            if (_viewModel.MovingLeft && pacmanHitBox.IntersectsWith(hitBox))
            {
                Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) + 10);
                _viewModel.BlockLeft();
            }

            if (_viewModel.MovingRight && pacmanHitBox.IntersectsWith(hitBox))
            {
                Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) - 10);
                _viewModel.BlockRight();
            }

            if (_viewModel.MovingDown && pacmanHitBox.IntersectsWith(hitBox))
            {
                Canvas.SetTop(pacman, Canvas.GetTop(pacman) - 10);
                _viewModel.BlockDown();
            }

            if (_viewModel.MovingUp && pacmanHitBox.IntersectsWith(hitBox))
            {
                Canvas.SetTop(pacman, Canvas.GetTop(pacman) + 10);
                _viewModel.BlockUp();
            }
        }

        // Geist Kollision

        private void CheckGhostCollision(
            Rectangle ghost,
            Rect hitBox)
        {
            // Pacman getroffen

            if (pacmanHitBox
                .IntersectsWith(hitBox))
            {
                _viewModel.SaveScore();

                GameOver("Du wurdest erwischt.");
            }
        }

        // SaveScore wird aufgerufen 

        // Game Over

        private void GameOver(
            string message)
        {
            gameTimer.Stop();

            MessageBox.Show(message);

            PacManMenu menu =
                new PacManMenu(
                    _loggedInUsername,
                    _loggedInUserID);

            menu.Show();

            this.Close();
        }

        // Reset

        private void ResetGame()
        {
            // Reset model positions and state
            _viewModel.ResetPositions();
            _viewModel.Score = 0;

            Canvas.SetLeft(pacman, _viewModel.PacmanLeft);
            Canvas.SetTop(pacman, _viewModel.PacmanTop);

            Canvas.SetLeft(redGuy, _viewModel.RedGuyLeft);
            Canvas.SetTop(redGuy, _viewModel.RedGuyTop);

            Canvas.SetLeft(orangeGuy, _viewModel.OrangeGuyLeft);
            Canvas.SetTop(orangeGuy, _viewModel.OrangeGuyTop);

            Canvas.SetLeft(pinkGuy, _viewModel.PinkGuyLeft);
            Canvas.SetTop(pinkGuy, _viewModel.PinkGuyTop);

            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
                if ((string)x.Tag == "coin") x.Visibility = Visibility.Visible;
            }

            MyCanvas.Focus();
            gameTimer.Start();
        }
    }
}