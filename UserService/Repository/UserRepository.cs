using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UserWebAPIService.Models;

namespace UserWebAPIService.Repository
{
    public class UserRepository
    {
        public List<User> GetAllUsers()
        {
            using(var dbContext = new UserContext())
            {
                var users = dbContext.User;
                return users.ToList();
            }
        }

        //public List<User> GetAllActiveUsers()
        //{
        //    using (var dbContext = new UserContext())
        //    {
        //        var users = dbContext.User.Where(u => u.Status == true);
        //        return users.ToList();
        //    }
        //}

        public User GetUserbyId(int id = 0)
        {
            if (id == 0)
                throw new ArgumentNullException("Invalid user id");

            using (var dbContext = new UserContext())
            {
                var users = dbContext.User.Where(u => u.Id == id);
                return users.FirstOrDefault();
            }
        }

        public void InsertUser(User userObj)
        {
            if (userObj == null)
                throw new ArgumentNullException("Invalid user");

            using (var dbContext = new UserContext())
            {
                dbContext.User.Add(userObj);
                dbContext.SaveChanges();
            }
        }

        public void UpdateUser(User userObj)
        {
            if (userObj == null)
                throw new ArgumentNullException("Invalid user");

            using (var dbContext = new UserContext())
            {
                var userToUpdate = dbContext.User.Find(userObj.Id);
                userToUpdate.FirstName = userObj.FirstName;
                userToUpdate.LastName = userObj.LastName;
                userToUpdate.Email = userObj.Email;
                userToUpdate.Password = userObj.Password;
                userToUpdate.Status = userObj.Status;
                dbContext.SaveChanges();
            }
        }

        public void DeleteUser(int userId)
        {
            if (userId == 0)
                throw new ArgumentNullException("Invalid user");

            using (var dbContext = new UserContext())
            {
                var user = dbContext.User.Find(userId);
                dbContext.User.Remove(user);
                dbContext.SaveChanges();
            }
        }
    }
}