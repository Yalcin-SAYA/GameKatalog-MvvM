using System;
using System.Collections.Generic;
using System.Text;

namespace GameKatalog_MvvM.Models
{
    public class Spielstand
    {
        public int Id { get; set; }

        public string Benutzername { get; set; }

        public string SpielName { get; set; }

        public int Punkte { get; set; }
    }
}
