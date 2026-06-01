using System.Linq;
using System.Windows;

using GameKatalog_MvvM.Data;

namespace GameKatalog_MvvM.Views
{
    public partial class KatalogView : Window
    {
        private string _loggedInUsername;

        public KatalogView(string username)
        {
            InitializeComponent();

            _loggedInUsername = username;

            UsernameLabel.Content =
                "Willkommen, " + username;

            LoadGesamtpunkte();
        }

        // Gesamtpunkte laden

        private void LoadGesamtpunkte()
        {
            using (AppDbContext db =
                new AppDbContext())
            {
                int gesamtpunkte =
                    db.Spielstaende
                    .Where(x =>
                        x.Benutzername ==
                        _loggedInUsername)
                    .Sum(x => x.Punkte);

                GesamtpunkteLabel.Content =
                    "Gesamtpunkte: "
                    + gesamtpunkte;
            }
        }

        // Logout

        private void LogoutButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            MainWindow mainWindow =
                new MainWindow();

            mainWindow.Show();

            this.Close();
        }

        // Space Shooter öffnen

        private void SpaceShooterButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            SpaceShooterMenuView menu =
                new SpaceShooterMenuView(
                    _loggedInUsername,
                    "1");

            menu.Show();

            this.Close();
        }

        private void PacManButton_Click(object sender, RoutedEventArgs e)
        {
            PacManMenu menu =
                new PacManMenu(
                    _loggedInUsername,
                    "1");

            menu.Show();

            this.Close();
        }

        private void RPGButton_Click(object sender, RoutedEventArgs e)
        {
            RPGMenuView menu =
                new RPGMenuView(
                    _loggedInUsername,
                    "1");

            menu.Show();

            this.Close();
        }

    }
}