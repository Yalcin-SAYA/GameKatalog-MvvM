using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using GameKatalog_MvvM.Data;
using GameKatalog_MvvM.Models;

namespace GameKatalog_MvvM.Services
{
    public class UserService
    {
        public void CreateUser(User user)
        {
            using (GameDbContext db = new GameDbContext())
            {
                db.Users.Add(user);

                db.SaveChanges();
            }
        }

        public User GetUser(string username, string password)
        {
            using (GameDbContext db = new GameDbContext())
            {
                User user = db.Users.FirstOrDefault(x => x.Username == username && x.Password == password);

                return user;
            }
        }
    }
}