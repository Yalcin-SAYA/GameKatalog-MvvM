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

namespace GameKatalog_MvvM.Views
{
    public partial class PacManSpielLV2View : Window
    {
        DispatcherTimer gameTimer =
            new DispatcherTimer();

        // Bewegung

        bool goLeft;
        bool goRight;
        bool goDown;
        bool goUp;

        bool noLeft;
        bool noRight;
        bool noDown;
        bool noUp;

        int speed = 8;

        Rect pacmanHitBox;

        int score = 0;

        // Login Daten

        private string _loggedInUsername;

        private string _loggedInUserID;

        // Startpositionen

        const double pacmanStartLeft = 25;

        const double pacmanStartTop = 104;

        // Geister Bewegung

        const int ghostMoveStep = 160;

        int orangeGuySpeed = 10;
        int redGuySpeed = 10;
        int pinkGuySpeed = 10;
        int greenGuySpeed = 10;
        int blueGuySpeed = 10;
        int aquaGuySpeed = 10;
        int tanGuySpeed = 10;
        int hotpinkGuySpeed = 10;
        int greyGuySpeed = 10;
        int violetGuySpeed = 10;

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

            GameSetUp();
        }

        // Testmodus

        public PacManSpielLV2View()
            : this("Gast", null)
        {

        }

        // Tastatur

        private void CanvasKeyDown(
            object sender,
            KeyEventArgs e)
        {
            MyCanvas.Focus();

            if (e.Key == Key.Left
                && !noLeft)
            {
                goLeft = true;

                goRight = false;
                goUp = false;
                goDown = false;

                noRight = false;
                noUp = false;
                noDown = false;

                pacman.RenderTransform =
                    new RotateTransform(
                        -180,
                        pacman.Width / 2,
                        pacman.Height / 2);
            }

            if (e.Key == Key.Right
                && !noRight)
            {
                goRight = true;

                goLeft = false;
                goUp = false;
                goDown = false;

                noLeft = false;
                noUp = false;
                noDown = false;

                pacman.RenderTransform =
                    new RotateTransform(
                        0,
                        pacman.Width / 2,
                        pacman.Height / 2);
            }

            if (e.Key == Key.Up
                && !noUp)
            {
                goUp = true;

                goLeft = false;
                goRight = false;
                goDown = false;

                noLeft = false;
                noRight = false;
                noDown = false;

                pacman.RenderTransform =
                    new RotateTransform(
                        -90,
                        pacman.Width / 2,
                        pacman.Height / 2);
            }

            if (e.Key == Key.Down
                && !noDown)
            {
                goDown = true;

                goLeft = false;
                goRight = false;
                goUp = false;

                noLeft = false;
                noRight = false;
                noUp = false;

                pacman.RenderTransform =
                    new RotateTransform(
                        90,
                        pacman.Width / 2,
                        pacman.Height / 2);
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
            txtScore.Content =
                "Score: " + score;

            // Pacman Bewegung

            if (goRight)
            {
                Canvas.SetLeft(
                    pacman,
                    Canvas.GetLeft(pacman)
                    + speed);
            }

            if (goLeft)
            {
                Canvas.SetLeft(
                    pacman,
                    Canvas.GetLeft(pacman)
                    - speed);
            }

            if (goUp)
            {
                Canvas.SetTop(
                    pacman,
                    Canvas.GetTop(pacman)
                    - speed);
            }

            if (goDown)
            {
                Canvas.SetTop(
                    pacman,
                    Canvas.GetTop(pacman)
                    + speed);
            }

            // Grenzen prüfen

            CheckBorders();

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
                        x.Visibility =
                            Visibility.Hidden;

                        score++;
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

            if (score == 164)  //164 Münzen 
            {
                SaveScore();

                GameOver(
                    "Du kannst ja doch was.",
                    true);
            }
        }

        // Grenzen

        private void CheckBorders()
        {
            double pacmanLeft =
                Canvas.GetLeft(pacman);

            double pacmanTop =
                Canvas.GetTop(pacman);

            double canvasWidth =
                MyCanvas.Width;

            double canvasHeight =
                MyCanvas.Height;

            // Unten

            if (goDown
                && pacmanTop
                + pacman.Height
                + speed > canvasHeight)
            {
                noDown = true;

                goDown = false;

                Canvas.SetTop(
                    pacman,
                    canvasHeight
                    - pacman.Height);
            }
            else
            {
                noDown = false;
            }

            // Oben

            if (goUp
                && pacmanTop
                - speed < 0)
            {
                noUp = true;

                goUp = false;

                Canvas.SetTop(
                    pacman,
                    0);
            }
            else
            {
                noUp = false;
            }

            // Links

            if (goLeft
                && pacmanLeft
                - speed < 0)
            {
                noLeft = true;

                goLeft = false;

                Canvas.SetLeft(
                    pacman,
                    0);
            }
            else
            {
                noLeft = false;
            }

            // Rechts

            if (goRight
                && pacmanLeft
                + pacman.Width
                + speed > canvasWidth)
            {
                noRight = true;

                goRight = false;

                Canvas.SetLeft(
                    pacman,
                    canvasWidth
                    - pacman.Width);
            }
            else
            {
                noRight = false;
            }
        }

        // Wand Kollision

        private void CheckWallCollision(
            Rectangle wall,
            Rect hitBox)
        {
            if (pacmanHitBox
                .IntersectsWith(hitBox))
            {
                if (goLeft)
                {
                    Canvas.SetLeft(
                        pacman,
                        Canvas.GetLeft(pacman)
                        + speed);

                    noLeft = true;

                    goLeft = false;
                }

                if (goRight)
                {
                    Canvas.SetLeft(
                        pacman,
                        Canvas.GetLeft(pacman)
                        - speed);

                    noRight = true;

                    goRight = false;
                }

                if (goDown)
                {
                    Canvas.SetTop(
                        pacman,
                        Canvas.GetTop(pacman)
                        - speed);

                    noDown = true;

                    goDown = false;
                }

                if (goUp)
                {
                    Canvas.SetTop(
                        pacman,
                        Canvas.GetTop(pacman)
                        + speed);

                    noUp = true;

                    goUp = false;
                }
            }
        }

        // Geister

        private void CheckGhostCollision(
            Rectangle ghost,
            Rect hitBox)
        {
            // Pacman getroffen

            if (pacmanHitBox
                .IntersectsWith(hitBox))
            {
                SaveScore();

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

            double newLeft =
                left + speedValue;

            if (newLeft <= min
                || newLeft >= max)
            {
                speedValue =
                    -speedValue;

                newLeft =
                    left + speedValue;
            }

            Canvas.SetLeft(
                ghost,
                newLeft);
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

            double newTop =
                top + speedValue;

            if (newTop <= min
                || newTop >= max)
            {
                speedValue =
                    -speedValue;

                newTop =
                    top + speedValue;
            }

            Canvas.SetTop(
                ghost,
                newTop);
        }

        // Score speichern

        private void SaveScore()
        {
            using (AppDbContext db =
                new AppDbContext())
            {
                Spielstand neuerSpielstand =
                    new Spielstand
                    {
                        Benutzername =
                            _loggedInUsername,

                        SpielName =
                            "PacMan LV2",

                        Punkte =
                            score
                    };

                db.Spielstaende.Add(
                    neuerSpielstand);

                db.SaveChanges();
            }
        }

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

            goLeft = false;
            goRight = false;
            goUp = false;
            goDown = false;

            noLeft = false;
            noRight = false;
            noUp = false;
            noDown = false;

            score = 0;

            txtScore.Content =
                "Score: 0";

            foreach (var x in
                MyCanvas.Children.OfType<Rectangle>())
            {
                if ((string)x.Tag == "coin")
                {
                    x.Visibility =
                        Visibility.Visible;
                }
            }

            orangeGuySpeed = 10;
            redGuySpeed = 10;
            pinkGuySpeed = 10;
            greenGuySpeed = 10;
            blueGuySpeed = 10;
            aquaGuySpeed = 10;
            tanGuySpeed = 10;
            hotpinkGuySpeed = 10;
            greyGuySpeed = 10;
            violetGuySpeed = 10;

            MyCanvas.Focus();

            gameTimer.Start();
        }
    }
}