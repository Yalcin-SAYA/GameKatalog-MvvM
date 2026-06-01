using System;
using System.Collections.Generic;
using System.Text;

namespace GameKatalog_MvvM.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public int TotalScore { get; set; }

        public int MaxScore { get; set; }
    }

}
