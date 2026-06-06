using GameKatalog_MvvM.Data;
using GameKatalog_MvvM.Models;

namespace GameKatalog_MvvM.ViewModels
{
    internal class RPGViewModel : BaseViewModel
    {
        private int _playerGold;
        public int PlayerGold { get => _playerGold; set { if (SetProperty(ref _playerGold, value)) OnPropertyChanged(nameof(GoldText)); } }

        private int _playerHp;
        public int PlayerHp { get => _playerHp; set { if (SetProperty(ref _playerHp, value)) OnPropertyChanged(nameof(HpText)); } }

        public string GoldText => "Gold: " + PlayerGold;
        public string HpText => "HP: " + PlayerHp;

        public string Username { get; }
        public string? UserId { get; }

        public RPGViewModel(string username, string? userId)
        {
            Username = username;
            UserId = userId;
            PlayerGold = 0;
            PlayerHp = 100;
        }

        public void AddGold(int amount)
        {
            PlayerGold += amount;
        }

        public void ApplyDamage(int amount)
        {
            PlayerHp -= amount;
        }

        public void Heal(int amount)
        {
            PlayerHp += amount;
        }

        public void SaveGold()
        {
            using (AppDbContext db = new AppDbContext())
            {
                Spielstand s = new Spielstand
                {
                    Benutzername = Username,
                    SpielName = "RPG",
                    Punkte = PlayerGold
                };

                db.Spielstaende.Add(s);
                db.SaveChanges();
            }
        }
    }
}
