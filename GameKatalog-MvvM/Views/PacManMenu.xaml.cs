using System.Windows;

namespace GameKatalog_MvvM.Views
{
    public partial class PacManMenu : Window
    {
        private string _loggedInUsername;

        private string _loggedInUserID;

        // Testmodus

        public PacManMenu(): this("Gast", null)
        {

        }

        // Login Daten übernehmen

        public PacManMenu(
            string username,
            string userID)
        {
            InitializeComponent();

            _loggedInUsername = username;

            _loggedInUserID = userID;
        }

        // Spiel starten

        private void PlayButton_Click( object sender,RoutedEventArgs e)
        {
            PacManSpielView game = new PacManSpielView(_loggedInUsername,_loggedInUserID);

            game.Show();

            this.Close();
        }

        // Zurück zum Katalog

        private void QuitButton_Click( object sender, RoutedEventArgs e)
        {
            KatalogView katalog = new KatalogView( _loggedInUsername);

            katalog.Show();

            this.Close();
        }
    }
}