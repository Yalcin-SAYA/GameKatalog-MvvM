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
    public partial class PacManSpielView : Window
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

        int ghostSpeed = 10;

        int ghostMoveStep = 160;

        int currentGhostStep;

        int score = 0;

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

            GameSetUp();
        }

        // Testmodus

        public PacManSpielView()
            : this("Gast", null)
        {

        }

        // Setup

        private void GameSetUp()
        {
            MyCanvas.Focus();

            gameTimer.Tick += GameLoop;

            gameTimer.Interval =
                TimeSpan.FromMilliseconds(20);

            gameTimer.Start();

            currentGhostStep =
                ghostMoveStep;

            // Bilder laden

            pacman.Fill =
                LoadImage("pacman.jpg");

            redGuy.Fill =
                LoadImage("red.jpg");

            orangeGuy.Fill =
                LoadImage("orange.jpg");

            pinkGuy.Fill =
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

        // Tastatur

        private void CanvasKeyDown(
            object sender,
            KeyEventArgs e)
        {
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

        // Haupt Spiel Loop

        private void GameLoop(
            object sender,
            EventArgs e)
        {
            // Score anzeigen

            txtScore.Content =
                "Score: " + score;

            // Bewegung

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

            // Alle Elemente prüfen

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

            if (score == 85)
            {
                SaveScore();

                gameTimer.Stop();

                MessageBox.Show(
                    "Level 1 geschafft!");

                PacManSpielLV2View lv2 =
                    new PacManSpielLV2View(
                        _loggedInUsername,
                        _loggedInUserID);

                lv2.Show();

                this.Close();
            }
        }

        // Spielfeld Grenzen

        private void CheckBorders()
        {
            if (goDown
                && Canvas.GetTop(pacman)
                + 80 >
                Application.Current.MainWindow.Height)
            {
                noDown = true;

                goDown = false;
            }

            if (goUp
                && Canvas.GetTop(pacman) < 1)
            {
                noUp = true;

                goUp = false;
            }

            if (goLeft
                && Canvas.GetLeft(pacman)
                - 10 < 1)
            {
                noLeft = true;

                goLeft = false;
            }

            if (goRight
                && Canvas.GetLeft(pacman)
                + 70 >
                Application.Current.MainWindow.Width)
            {
                noRight = true;

                goRight = false;
            }
        }

        // Wand Kollision

        private void CheckWallCollision(
            Rectangle wall,
            Rect hitBox)
        {
            if (goLeft
                && pacmanHitBox
                .IntersectsWith(hitBox))
            {
                Canvas.SetLeft(
                    pacman,
                    Canvas.GetLeft(pacman)
                    + 10);

                noLeft = true;

                goLeft = false;
            }

            if (goRight
                && pacmanHitBox
                .IntersectsWith(hitBox))
            {
                Canvas.SetLeft(
                    pacman,
                    Canvas.GetLeft(pacman)
                    - 10);

                noRight = true;

                goRight = false;
            }

            if (goDown
                && pacmanHitBox
                .IntersectsWith(hitBox))
            {
                Canvas.SetTop(
                    pacman,
                    Canvas.GetTop(pacman)
                    - 10);

                noDown = true;

                goDown = false;
            }

            if (goUp
                && pacmanHitBox
                .IntersectsWith(hitBox))
            {
                Canvas.SetTop(
                    pacman,
                    Canvas.GetTop(pacman)
                    + 10);

                noUp = true;

                goUp = false;
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
                SaveScore();

                GameOver(
                    "Du wurdest erwischt.");
            }

            // Bewegung der Geister

            if (ghost.Name ==
                "orangeGuy")
            {
                Canvas.SetLeft(
                    ghost,
                    Canvas.GetLeft(ghost)
                    - ghostSpeed);
            }
            else
            {
                Canvas.SetLeft(
                    ghost,
                    Canvas.GetLeft(ghost)
                    + ghostSpeed);
            }

            // Richtungswechsel

            currentGhostStep--;

            if (currentGhostStep < 1)
            {
                currentGhostStep =
                    ghostMoveStep;

                ghostSpeed =
                    -ghostSpeed;
            }
        }

        // Score speichern mit EF

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
                            "PacMan",

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
            // Positionen

            Canvas.SetLeft(
                pacman,
                pacmanStartLeft);

            Canvas.SetTop(
                pacman,
                pacmanStartTop);

            Canvas.SetLeft(
                redGuy,
                redGuyStartLeft);

            Canvas.SetTop(
                redGuy,
                redGuyStartTop);

            Canvas.SetLeft(
                orangeGuy,
                orangeGuyStartLeft);

            Canvas.SetTop(
                orangeGuy,
                orangeGuyStartTop);

            Canvas.SetLeft(
                pinkGuy,
                pinkGuyStartLeft);

            Canvas.SetTop(
                pinkGuy,
                pinkGuyStartTop);

            // Bewegung zurücksetzen

            goLeft = false;
            goRight = false;
            goUp = false;
            goDown = false;

            noLeft = false;
            noRight = false;
            noUp = false;
            noDown = false;

            // Score zurücksetzen

            score = 0;

            txtScore.Content =
                "Score: 0";

            // Coins wieder sichtbar

            foreach (var x in
                MyCanvas.Children.OfType<Rectangle>())
            {
                if ((string)x.Tag == "coin")
                {
                    x.Visibility =
                        Visibility.Visible;
                }
            }

            // Ghostspeed reset

            ghostSpeed = 10;

            ghostMoveStep = 160;

            currentGhostStep =
                ghostMoveStep;

            MyCanvas.Focus();

            gameTimer.Start();
        }
    }
}