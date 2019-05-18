using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using UserWebAPIService.Models;
using UserWebAPIService.Repository;

namespace UserWebAPIService.Controllers
{
    public class UserController : ApiController
    {
        public readonly UserRepository userRepository;

        public UserController()
        {
            userRepository = new UserRepository();
        }
        [HttpGet]
        public JsonResult<List<User>> GetAllUsers()
        {
            var users = userRepository.GetAllUsers();
            return Json(users);
        }

        [HttpGet]
        [ActionName("GetUserById")]
        public JsonResult<User> GetUserById([FromUri]int id)
        {
            var users = userRepository.GetUserbyId(id);
            return Json(users);
        }

        [HttpPost]
        [ActionName("InsertUser")]
        public void InsertUser(User user)
        {
            userRepository.InsertUser(user);
        }

        [HttpPut]
        [ActionName("UpdateUser")]
        public void UpdateUser(User user)
        {
            userRepository.UpdateUser(user);
        }

        [HttpDelete]
        [ActionName("DeleteUser")]
        public void DeleteUser([FromUri]int id)
        {
            userRepository.DeleteUser(id);
        }


    }
}
