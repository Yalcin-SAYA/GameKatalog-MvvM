using System;
using GameKatalog_MvvM.Data;
using GameKatalog_MvvM.Models;

namespace GameKatalog_MvvM.ViewModels
{
    internal class PacManViewModel : BaseViewModel
    {
        private int _score;
        public int Score
        {
            get => _score;
            set
            {
                // SetProperty ändert den Wert und meldet es automatisch der UI
                if (SetProperty(ref _score, value))
                {
                    OnPropertyChanged(nameof(ScoreText));
                }
            }
        }

        public string ScoreText => "Score: " + Score;
        //Spieler Daten nach erstellung Fix
        public string Username { get; }

        public string? UserId { get; }

        public PacManViewModel(string username, string? userId)
        {
            Username = username;
            UserId = userId;
            Score = 0;
        }

        // Position und Bewegung von Spieler udn Gegner
        private double _pacmanLeft = 50;
        public double PacmanLeft { get => _pacmanLeft; set => SetProperty(ref _pacmanLeft, value); }

        private double _pacmanTop = 104;
        public double PacmanTop { get => _pacmanTop; set => SetProperty(ref _pacmanTop, value); }
        //Geit ROT
        private double _redGuyLeft = 173;
        public double RedGuyLeft { get => _redGuyLeft; set => SetProperty(ref _redGuyLeft, value); }
        private double _redGuyTop = 29;
        public double RedGuyTop { get => _redGuyTop; set => SetProperty(ref _redGuyTop, value); }
        //Geist ORANGE
        private double _orangeGuyLeft = 651;
        public double OrangeGuyLeft { get => _orangeGuyLeft; set => SetProperty(ref _orangeGuyLeft, value); }

        private double _orangeGuyTop = 104;
        public double OrangeGuyTop { get => _orangeGuyTop; set => SetProperty(ref _orangeGuyTop, value); }
        //Geist PINK
        private double _pinkGuyLeft = 173;
        public double PinkGuyLeft { get => _pinkGuyLeft; set => SetProperty(ref _pinkGuyLeft, value); }

        private double _pinkGuyTop = 404;
        public double PinkGuyTop { get => _pinkGuyTop; set => SetProperty(ref _pinkGuyTop, value); }

        // zusätzlciehe gegner LV2
        private double _violetGuyLeft;
        public double VioletGuyLeft { get => _violetGuyLeft; set => SetProperty(ref _violetGuyLeft, value); }
        private double _violetGuyTop;
        public double VioletGuyTop { get => _violetGuyTop; set => SetProperty(ref _violetGuyTop, value); }

        private double _greyGuyLeft;
        public double GreyGuyLeft { get => _greyGuyLeft; set => SetProperty(ref _greyGuyLeft, value); }
        private double _greyGuyTop;
        public double GreyGuyTop { get => _greyGuyTop; set => SetProperty(ref _greyGuyTop, value); }

        private double _hotpinkGuyLeft;
        public double HotpinkGuyLeft { get => _hotpinkGuyLeft; set => SetProperty(ref _hotpinkGuyLeft, value); }
        private double _hotpinkGuyTop;
        public double HotpinkGuyTop { get => _hotpinkGuyTop; set => SetProperty(ref _hotpinkGuyTop, value); }

        private double _aquaGuyLeft;
        public double AquaGuyLeft { get => _aquaGuyLeft; set => SetProperty(ref _aquaGuyLeft, value); }
        private double _aquaGuyTop;
        public double AquaGuyTop { get => _aquaGuyTop; set => SetProperty(ref _aquaGuyTop, value); }

        private double _tanGuyLeft;
        public double TanGuyLeft { get => _tanGuyLeft; set => SetProperty(ref _tanGuyLeft, value); }
        private double _tanGuyTop;
        public double TanGuyTop { get => _tanGuyTop; set => SetProperty(ref _tanGuyTop, value); }

        private double _greenGuyLeft;
        public double GreenGuyLeft { get => _greenGuyLeft; set => SetProperty(ref _greenGuyLeft, value); }
        private double _greenGuyTop;
        public double GreenGuyTop { get => _greenGuyTop; set => SetProperty(ref _greenGuyTop, value); }

        private double _blueGuyLeft;
        public double BlueGuyLeft { get => _blueGuyLeft; set => SetProperty(ref _blueGuyLeft, value); }
        private double _blueGuyTop;
        public double BlueGuyTop { get => _blueGuyTop; set => SetProperty(ref _blueGuyTop, value); }

        private bool _movingLeft;
        public bool MovingLeft { get => _movingLeft; private set => SetProperty(ref _movingLeft, value); }
        private bool _movingRight;
        public bool MovingRight { get => _movingRight; private set => SetProperty(ref _movingRight, value); }
        private bool _movingUp;
        public bool MovingUp { get => _movingUp; private set => SetProperty(ref _movingUp, value); }
        private bool _movingDown;
        public bool MovingDown { get => _movingDown; private set => SetProperty(ref _movingDown, value); }

        private bool _noLeft;
        public bool NoLeft { get => _noLeft; private set => SetProperty(ref _noLeft, value); }
        private bool _noRight;
        public bool NoRight { get => _noRight; private set => SetProperty(ref _noRight, value); }
        private bool _noUp;
        public bool NoUp { get => _noUp; private set => SetProperty(ref _noUp, value); }
        private bool _noDown;
        public bool NoDown { get => _noDown; private set => SetProperty(ref _noDown, value); }

        private int speed = 8;
        public int Speed => speed;

        private int ghostSpeed = 10;
        private int ghostMoveStep = 160;
        private int currentGhostStep = 160;
        public bool UseViewModelGhostMovement { get; set; } = true;

        // Bewegungen der Gegner von Position A->B/B->A
        private (double Left, double Top)[] orangeWaypoints = new[] { (651.0, 104.0), (173.0, 104.0) };
        private int orangeTargetIndex = 1;

        private (double Left, double Top)[] redWaypoints = new[] { (173.0, 29.0), (651.0, 29.0) };
        private int redTargetIndex = 1;

        private (double Left, double Top)[] pinkWaypoints = new[] { (173.0, 404.0), (651.0, 404.0) };
        private int pinkTargetIndex = 1;

        public void MoveLeft()
        {
            if (NoLeft) return;
            MovingLeft = true; MovingRight = MovingUp = MovingDown = false;
            NoRight = NoUp = NoDown = false;
        }

        public void MoveRight()
        {
            if (NoRight) return;
            MovingRight = true; MovingLeft = MovingUp = MovingDown = false;
            NoLeft = NoUp = NoDown = false;
        }

        public void MoveUp()
        {
            if (NoUp) return;
            MovingUp = true; MovingLeft = MovingRight = MovingDown = false;
            NoLeft = NoRight = NoDown = false;
        }

        public void MoveDown()
        {
            if (NoDown) return;
            MovingDown = true; MovingLeft = MovingRight = MovingUp = false;
            NoLeft = NoRight = NoUp = false;
        }

        public void BlockLeft() { NoLeft = true; MovingLeft = false; }
        public void BlockRight() { NoRight = true; MovingRight = false; }
        public void BlockUp() { NoUp = true; MovingUp = false; }
        public void BlockDown() { NoDown = true; MovingDown = false; }

        public void Tick()
        {
            if (MovingRight) PacmanLeft += speed;
            if (MovingLeft) PacmanLeft -= speed;
            if (MovingUp) PacmanTop -= speed;
            if (MovingDown) PacmanTop += speed;
            if (UseViewModelGhostMovement)
            {
                // geister bewegungen 
                // Orange
                {
                    var target = orangeWaypoints[orangeTargetIndex];
                    double dx = target.Left - OrangeGuyLeft;
                    double dy = target.Top - OrangeGuyTop;
                    double dist = Math.Sqrt(dx * dx + dy * dy);
                    if (dist <= ghostSpeed || dist == 0)
                    {
                        OrangeGuyLeft = target.Left;
                        OrangeGuyTop = target.Top;
                        orangeTargetIndex = (orangeTargetIndex + 1) % orangeWaypoints.Length;
                    }
                    else
                    {
                        OrangeGuyLeft += dx / dist * ghostSpeed;
                        OrangeGuyTop += dy / dist * ghostSpeed;
                    }
                }

                // Rot
                {
                    var target = redWaypoints[redTargetIndex];
                    double dx = target.Left - RedGuyLeft;
                    double dy = target.Top - RedGuyTop;
                    double dist = Math.Sqrt(dx * dx + dy * dy);
                    if (dist <= ghostSpeed || dist == 0)
                    {
                        RedGuyLeft = target.Left;
                        RedGuyTop = target.Top;
                        redTargetIndex = (redTargetIndex + 1) % redWaypoints.Length;
                    }
                    else
                    {
                        RedGuyLeft += dx / dist * ghostSpeed;
                        RedGuyTop += dy / dist * ghostSpeed;
                    }
                }

                // Pink
                {
                    var target = pinkWaypoints[pinkTargetIndex];
                    double dx = target.Left - PinkGuyLeft;
                    double dy = target.Top - PinkGuyTop;
                    double dist = Math.Sqrt(dx * dx + dy * dy);
                    if (dist <= ghostSpeed || dist == 0)
                    {
                        PinkGuyLeft = target.Left;
                        PinkGuyTop = target.Top;
                        pinkTargetIndex = (pinkTargetIndex + 1) % pinkWaypoints.Length;
                    }
                    else
                    {
                        PinkGuyLeft += dx / dist * ghostSpeed;
                        PinkGuyTop += dy / dist * ghostSpeed;
                    }
                }
            }
        }
        //Bei Game over oder neustart Positionen reset
        public void ResetPositions()
        {
            PacmanLeft = 50; PacmanTop = 104;
            RedGuyLeft = 173; RedGuyTop = 29;
            OrangeGuyLeft = 651; OrangeGuyTop = 104;
            PinkGuyLeft = 173; PinkGuyTop = 404;

            MovingLeft = MovingRight = MovingUp = MovingDown = false;
            NoLeft = NoRight = NoUp = NoDown = false;

            ghostSpeed = 10;
            ghostMoveStep = 160;
            currentGhostStep = ghostMoveStep;
            //geister positionen und Bewegungen zurück setzetn 
            orangeTargetIndex = 1;
            redTargetIndex = 1;
            pinkTargetIndex = 1;
            // zusätzliche gegner zurücksetzen(lv2)
            VioletGuyLeft = 0; VioletGuyTop = 0;
            GreyGuyLeft = 0; GreyGuyTop = 0;
            HotpinkGuyLeft = 0; HotpinkGuyTop = 0;
            AquaGuyLeft = 0; AquaGuyTop = 0;
            TanGuyLeft = 0; TanGuyTop = 0;
            GreenGuyLeft = 0; GreenGuyTop = 0;
            BlueGuyLeft = 0; BlueGuyTop = 0;
        }

        public void SaveScore()
        {
            using (AppDbContext db = new AppDbContext())
            {
                Spielstand neuerSpielstand = new Spielstand
                {
                    Benutzername = Username,
                    SpielName = "PacMan",
                    Punkte = Score
                };

                db.Spielstaende.Add(neuerSpielstand);
                db.SaveChanges();
            }
        }
    }
}
