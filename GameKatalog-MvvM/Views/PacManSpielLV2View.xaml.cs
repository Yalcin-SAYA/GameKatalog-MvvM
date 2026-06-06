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
    public partial class PacManSpielLV2View : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer();
        Rect pacmanHitBox;
        private PacManViewModel _viewModel;

        // Login Daten

        private string _loggedInUsername;

        private string _loggedInUserID;

        // Startpositionen

        const double pacmanStartLeft = 25;

        const double pacmanStartTop = 104;

        // Geister Bewegung

        const int ghostMoveStep = 160;

        int orangeGuySpeed = 6;
        int redGuySpeed = 6;
        int pinkGuySpeed = 6;
        int greenGuySpeed = 6;
        int blueGuySpeed = 6;
        int aquaGuySpeed = 6;
        int tanGuySpeed = 6;
        int hotpinkGuySpeed = 6;
        int greyGuySpeed = 6;
        int violetGuySpeed = 6;

        // Schrittzähler

        int orangeGuySteps = ghostMoveStep;
        int redGuySteps = ghostMoveStep;
        int pinkGuySteps = ghostMoveStep;
        int greenGuySteps = ghostMoveStep;
        int blueGuySteps = ghostMoveStep;
        int aquaGuySteps = ghostMoveStep;
        int tanGuySteps = ghostMoveStep;
        int hotpinkGuySteps = ghostMoveStep;
        int greyGuySteps = ghostMoveStep;
        int violetGuySteps = ghostMoveStep;

        // Konstruktor

        public PacManSpielLV2View(
            string username,
            string userID)
        {
            InitializeComponent();

            _loggedInUsername = username;
            _loggedInUserID = userID;

            _viewModel = new PacManViewModel(_loggedInUsername, _loggedInUserID);
            _viewModel.UseViewModelGhostMovement = false;
            this.DataContext = _viewModel;

            GameSetUp();
        }

        // Testmodus

        public PacManSpielLV2View()
            : this("Gast", null)
        {

        }

        // Tastatur

        private void CanvasKeyDown(object sender, KeyEventArgs e)
        {
            MyCanvas.Focus();

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

        // Setup

        private void GameSetUp()
        {
            MyCanvas.Focus();

            gameTimer.Tick += GameLoop;

            gameTimer.Interval =
                TimeSpan.FromMilliseconds(20);

            gameTimer.Start();

            // Schrittzähler reset

            orangeGuySteps = ghostMoveStep;
            redGuySteps = ghostMoveStep;
            pinkGuySteps = ghostMoveStep;
            greenGuySteps = ghostMoveStep;
            blueGuySteps = ghostMoveStep;
            aquaGuySteps = ghostMoveStep;
            tanGuySteps = ghostMoveStep;
            hotpinkGuySteps = ghostMoveStep;
            greyGuySteps = ghostMoveStep;
            violetGuySteps = ghostMoveStep;

            // Bilder laden

            pacman.Fill =
                LoadImage("pacman.jpg");

            redGuy.Fill =
                LoadImage("red.jpg");

            orangeGuy.Fill =
                LoadImage("orange.jpg");

            pinkGuy.Fill =
                LoadImage("pink.jpg");

            greyGuy.Fill =
                LoadImage("red.jpg");

            violetGuy.Fill =
                LoadImage("orange.jpg");

            aquaGuy.Fill =
                LoadImage("pink.jpg");

            tanGuy.Fill =
                LoadImage("red.jpg");

            hotpinkGuy.Fill =
                LoadImage("red.jpg");

            greenGuy.Fill =
                LoadImage("pink.jpg");
        }

        // Bilder laden

        private ImageBrush LoadImage(
            string fileName)
        {
            ImageBrush imageBrush =
                new ImageBrush();

            imageBrush.ImageSource =
                new BitmapImage(
                    new Uri(
                        $"pack://application:,,,/Assets/Bilder/PacMan/{fileName}"));

            return imageBrush;
        }

        // Haupt Spiel Loop

        private void GameLoop(
            object sender,
            EventArgs e)
        {
            // txtScore is bound to ViewModel.ScoreText

            // Update positions via ViewModel
            _viewModel.Tick();

            Canvas.SetLeft(pacman, _viewModel.PacmanLeft);
            Canvas.SetTop(pacman, _viewModel.PacmanTop);

            Canvas.SetLeft(redGuy, _viewModel.RedGuyLeft);
            Canvas.SetTop(redGuy, _viewModel.RedGuyTop);

            Canvas.SetLeft(orangeGuy, _viewModel.OrangeGuyLeft);
            Canvas.SetTop(orangeGuy, _viewModel.OrangeGuyTop);

            Canvas.SetLeft(pinkGuy, _viewModel.PinkGuyLeft);
            Canvas.SetTop(pinkGuy, _viewModel.PinkGuyTop);

            // Ensure ghosts move every frame (independent of collision checks)
            MoveHorizontalGhost(redGuy, ref redGuySpeed, 140, 697);
            MoveHorizontalGhost(orangeGuy, ref orangeGuySpeed, 100, 770);
            MoveHorizontalGhost(pinkGuy, ref pinkGuySpeed, 140, 690);
            MoveHorizontalGhost(tanGuy, ref tanGuySpeed, 138, 661);
            MoveHorizontalGhost(hotpinkGuy, ref hotpinkGuySpeed, 138, 661);
            MoveHorizontalGhost(aquaGuy, ref aquaGuySpeed, 859, 1138);
            MoveHorizontalGhost(violetGuy, ref violetGuySpeed, 122, 697);
            MoveVerticalGhost(greenGuy, ref greenGuySpeed, 272, 632);
            MoveVerticalGhost(blueGuy, ref blueGuySpeed, 272, 751);
            MoveHorizontalGhost(greyGuy, ref greyGuySpeed, 15, 1142);

            // Hitbox

            pacmanHitBox =
                new Rect(
                    Canvas.GetLeft(pacman),
                    Canvas.GetTop(pacman),
                    pacman.Width,
                    pacman.Height);

            // Kamera auf Pacman

            double desiredScrollX =
                Canvas.GetLeft(pacman)
                + (pacman.Width / 2)
                - (gameScrollViewer.ActualWidth / 2);

            double desiredScrollY =
                Canvas.GetTop(pacman)
                + (pacman.Height / 2)
                - (gameScrollViewer.ActualHeight / 2);

            desiredScrollX =
                Math.Max(
                    0,
                    Math.Min(
                        desiredScrollX,
                        MyCanvas.Width
                        - gameScrollViewer.ActualWidth));

            desiredScrollY =
                Math.Max(
                    0,
                    Math.Min(
                        desiredScrollY,
                        MyCanvas.Height
                        - gameScrollViewer.ActualHeight));

            gameScrollViewer
                .ScrollToHorizontalOffset(
                    desiredScrollX);

            gameScrollViewer
                .ScrollToVerticalOffset(
                    desiredScrollY);

            // Kollisionen

            foreach (var x in
                MyCanvas.Children.OfType<Rectangle>())
            {
                Rect hitBox =
                    new Rect(
                        Canvas.GetLeft(x),
                        Canvas.GetTop(x),
                        x.Width,
                        x.Height);

                // Wände

                if ((string)x.Tag == "wall")
                {
                    CheckWallCollision(
                        x,
                        hitBox);
                }

                // Coins

                if ((string)x.Tag == "coin")
                {
                    if (pacmanHitBox
                        .IntersectsWith(hitBox)
                        && x.Visibility ==
                        Visibility.Visible)
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

            if (_viewModel.Score == 164)  //164 Münzen 
            {
                _viewModel.SaveScore();
                GameOver("Du kannst ja doch was.", true);
            }
        }

        // Grenzen prüfen via ViewModel
        private void CheckBorders()
        {
            double pacmanLeft = Canvas.GetLeft(pacman);
            double pacmanTop = Canvas.GetTop(pacman);

            double canvasWidth = MyCanvas.Width;
            double canvasHeight = MyCanvas.Height;

            // Unten
            if (_viewModel.MovingDown && pacmanTop + pacman.Height + _viewModel.Speed > canvasHeight)
            {
                _viewModel.BlockDown();
                Canvas.SetTop(pacman, canvasHeight - pacman.Height);
            }

            // Oben
            if (_viewModel.MovingUp && pacmanTop - _viewModel.Speed < 0)
            {
                _viewModel.BlockUp();
                Canvas.SetTop(pacman, 0);
            }

            // Links
            if (_viewModel.MovingLeft && pacmanLeft - _viewModel.Speed < 0)
            {
                _viewModel.BlockLeft();
                Canvas.SetLeft(pacman, 0);
            }

            // Rechts
            if (_viewModel.MovingRight && pacmanLeft + pacman.Width + _viewModel.Speed > canvasWidth)
            {
                _viewModel.BlockRight();
                Canvas.SetLeft(pacman, canvasWidth - pacman.Width);
            }
        }

        // Wand Kollision

        private void CheckWallCollision(
            Rectangle wall,
            Rect hitBox)
        {
                if (pacmanHitBox.IntersectsWith(hitBox))
                {
                    if (_viewModel.MovingLeft)
                    {
                        double newLeft = Canvas.GetLeft(pacman) + _viewModel.Speed;
                        Canvas.SetLeft(pacman, newLeft);
                        _viewModel.BlockLeft();
                        _viewModel.PacmanLeft = newLeft;
                    }

                    if (_viewModel.MovingRight)
                    {
                        double newLeft = Canvas.GetLeft(pacman) - _viewModel.Speed;
                        Canvas.SetLeft(pacman, newLeft);
                        _viewModel.BlockRight();
                        _viewModel.PacmanLeft = newLeft;
                    }

                    if (_viewModel.MovingDown)
                    {
                        double newTop = Canvas.GetTop(pacman) - _viewModel.Speed;
                        Canvas.SetTop(pacman, newTop);
                        _viewModel.BlockDown();
                        _viewModel.PacmanTop = newTop;
                    }

                    if (_viewModel.MovingUp)
                    {
                        double newTop = Canvas.GetTop(pacman) + _viewModel.Speed;
                        Canvas.SetTop(pacman, newTop);
                        _viewModel.BlockUp();
                        _viewModel.PacmanTop = newTop;
                    }
                }
        }

        // Geister

        private void CheckGhostCollision(
            Rectangle ghost,
            Rect hitBox)
        {
            // Pacman getroffen

            if (pacmanHitBox.IntersectsWith(hitBox))
            {
                _viewModel.SaveScore();

                GameOver(
                    "Du wurdest erwischt.",
                    false);
            }

            double left =
                Canvas.GetLeft(ghost);

            double top =
                Canvas.GetTop(ghost);

            // Ghost Bewegungen

            if (ghost.Name == "redGuy")
            {
                MoveHorizontalGhost(
                    ghost,
                    ref redGuySpeed,
                    140,
                    697);
            }

            else if (ghost.Name == "orangeGuy")
            {
                MoveHorizontalGhost(
                    ghost,
                    ref orangeGuySpeed,
                    100,
                    770);
            }

            else if (ghost.Name == "pinkGuy")
            {
                MoveHorizontalGhost(
                    ghost,
                    ref pinkGuySpeed,
                    140,
                    690);
            }

            else if (ghost.Name == "tanGuy")
            {
                MoveHorizontalGhost(
                    ghost,
                    ref tanGuySpeed,
                    138,
                    661);
            }

            else if (ghost.Name == "hotpinkGuy")
            {
                MoveHorizontalGhost(
                    ghost,
                    ref hotpinkGuySpeed,
                    138,
                    661);
            }

            else if (ghost.Name == "aquaGuy")
            {
                MoveHorizontalGhost(
                    ghost,
                    ref aquaGuySpeed,
                    859,
                    1138);
            }

            else if (ghost.Name == "violetGuy")
            {
                MoveHorizontalGhost(
                    ghost,
                    ref violetGuySpeed,
                    122,
                    697);
            }

            else if (ghost.Name == "greenGuy")
            {
                MoveVerticalGhost(
                    ghost,
                    ref greenGuySpeed,
                    272,
                    632);
            }

            else if (ghost.Name == "blueGuy")
            {
                MoveVerticalGhost(
                    ghost,
                    ref blueGuySpeed,
                    272,
                    751);
            }

            else if (ghost.Name == "greyGuy")
            {
                MoveHorizontalGhost(
                    ghost,
                    ref greyGuySpeed,
                    15,
                    1142);
            }
        }

        // Horizontaler Geist

        private void MoveHorizontalGhost(
            Rectangle ghost,
            ref int speedValue,
            double min,
            double max)
        {
            double left =
                Canvas.GetLeft(ghost);

            double newLeft = left + speedValue;

            if (newLeft <= min || newLeft >= max)
            {
                speedValue = -speedValue;
                newLeft = left + speedValue;
            }

            Canvas.SetLeft(ghost, newLeft);

            Console.WriteLine($"MoveHorizontalGhost: {ghost.Name} -> {newLeft}");

            // update viewmodel positions so they're authoritative
            if (ghost.Name == "redGuy") _viewModel.RedGuyLeft = newLeft;
            else if (ghost.Name == "orangeGuy") _viewModel.OrangeGuyLeft = newLeft;
            else if (ghost.Name == "pinkGuy") _viewModel.PinkGuyLeft = newLeft;
            else if (ghost.Name == "violetGuy") _viewModel.VioletGuyLeft = newLeft;
            else if (ghost.Name == "greyGuy") _viewModel.GreyGuyLeft = newLeft;
            else if (ghost.Name == "hotpinkGuy") _viewModel.HotpinkGuyLeft = newLeft;
            else if (ghost.Name == "aquaGuy") _viewModel.AquaGuyLeft = newLeft;
            else if (ghost.Name == "tanGuy") _viewModel.TanGuyLeft = newLeft;
            else if (ghost.Name == "greenGuy") _viewModel.GreenGuyLeft = newLeft;
            else if (ghost.Name == "blueGuy") _viewModel.BlueGuyLeft = newLeft;
        }

        // Vertikaler Geist

        private void MoveVerticalGhost(
            Rectangle ghost,
            ref int speedValue,
            double min,
            double max)
        {
            double top =
                Canvas.GetTop(ghost);

            double newTop = top + speedValue;

            if (newTop <= min || newTop >= max)
            {
                speedValue = -speedValue;
                newTop = top + speedValue;
            }

            Canvas.SetTop(ghost, newTop);

            Console.WriteLine($"MoveVerticalGhost: {ghost.Name} -> {newTop}");

            if (ghost.Name == "greenGuy") _viewModel.GreenGuyTop = newTop;
            else if (ghost.Name == "blueGuy") _viewModel.BlueGuyTop = newTop;
            else if (ghost.Name == "pinkGuy") _viewModel.PinkGuyTop = newTop;
            else if (ghost.Name == "redGuy") _viewModel.RedGuyTop = newTop;
            else if (ghost.Name == "orangeGuy") _viewModel.OrangeGuyTop = newTop;
        }

        // Score persistence moved to PacManViewModel

        // Game Over

        private void GameOver(
            string message,
            bool isWin)
        {
            gameTimer.Stop();

            MessageBoxResult result =
                MessageBox.Show(
                    message + "\nNeustarten?",
                    "Pacman",
                    MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                PacManSpielLV2View game =
                    new PacManSpielLV2View(
                        _loggedInUsername,
                        _loggedInUserID);

                game.Show();

                this.Close();
            }
            else
            {
                PacManMenu menu =
                    new PacManMenu(
                        _loggedInUsername,
                        _loggedInUserID);

                menu.Show();

                this.Close();
            }
        }

        // Reset

        private void ResetGame()
        {
            Canvas.SetLeft(
                pacman,
                pacmanStartLeft);

            Canvas.SetTop(
                pacman,
                pacmanStartTop);

            _viewModel.Score = 0;
            _viewModel.ResetPositions();

            Canvas.SetLeft(pacman, _viewModel.PacmanLeft);
            Canvas.SetTop(pacman, _viewModel.PacmanTop);

            Canvas.SetLeft(redGuy, _viewModel.RedGuyLeft);
            Canvas.SetTop(redGuy, _viewModel.RedGuyTop);

            Canvas.SetLeft(orangeGuy, _viewModel.OrangeGuyLeft);
            Canvas.SetTop(orangeGuy, _viewModel.OrangeGuyTop);

            Canvas.SetLeft(pinkGuy, _viewModel.PinkGuyLeft);
            Canvas.SetTop(pinkGuy, _viewModel.PinkGuyTop);

            foreach (var x in
                MyCanvas.Children.OfType<Rectangle>())
            {
                if ((string)x.Tag == "coin")
                {
                    x.Visibility =
                        Visibility.Visible;
                }
            }

            orangeGuySpeed = 6;
            redGuySpeed = 6;
            pinkGuySpeed = 6;
            greenGuySpeed = 6;
            blueGuySpeed = 6;
            aquaGuySpeed = 6;
            tanGuySpeed = 6;
            hotpinkGuySpeed = 6;
            greyGuySpeed = 6;
            violetGuySpeed = 6;

            MyCanvas.Focus();

            gameTimer.Start();
        }
    }
}