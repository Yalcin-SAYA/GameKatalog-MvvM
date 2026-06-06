using System;
using GameKatalog_MvvM.Data;
using GameKatalog_MvvM.Models;

namespace GameKatalog_MvvM.ViewModels
{
    internal class SpaceShooterViewModel : BaseViewModel
    {
        private int _score;
        public int Score { get => _score; set { if (SetProperty(ref _score, value)) OnPropertyChanged(nameof(ScoreText)); } }

        public string ScoreText => "Score: " + Score;

        private int _damage;
        public int Damage { get => _damage; private set { if (SetProperty(ref _damage, value)) OnPropertyChanged(nameof(DamageText)); } }

        public string DamageText => "Damage: " + Damage;

        public string Username { get; }
        public string? UserId { get; }

        public SpaceShooterViewModel(string username, string? userId)
        {
            Username = username;
            UserId = userId;
            Score = 0;
            Damage = 0;
        }

        public void IncreaseScore(int amount = 1)
        {
            Score += amount;
        }

        public void IncreaseDamage(int amount = 1)
        {
            Damage += amount;
        }

        public void ResetState()
        {
            Score = 0;
            Damage = 0;
        }

        public void SaveScore()
        {
            using (AppDbContext db = new AppDbContext())
            {
                Spielstand spiel = new Spielstand
                {
                    Benutzername = Username,
                    SpielName = "SpaceShooter",
                    Punkte = Score
                };

                db.Spielstaende.Add(spiel);
                db.SaveChanges();
            }
        }
    }
}
