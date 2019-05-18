using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using UserAudit.Web.Models;
using UserAudit.Web.Service;

namespace UserAudit.Web.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        public UserService userService = null;
        public ActionResult List()
        {
            try
            {
                userService = new UserService();
                var response = userService.GetResponseMessage("api/user/GetAllUsers");
                response.EnsureSuccessStatusCode();
                var users = response.Content.ReadAsAsync<List<UserModel>>().Result;
                return View(users);
            }
            catch(Exception)
            {
                throw;
            }
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        public ActionResult Create(UserModel model)
        {
            try
            {
                // TODO: Add insert logic here
                userService = new UserService();
                var response = userService.Insert("api/user/InsertUser", model);
                response.EnsureSuccessStatusCode();
                return RedirectToAction("List");
            }
            catch
            {
                return View();
            }
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int id)
        {
            userService = new UserService();
            var response = userService.GetResponseMessage("api/user/GetUserById?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            var user = response.Content.ReadAsAsync<UserModel>().Result;
            if (user == null)
                RedirectToAction("List");

            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        public ActionResult Edit(UserModel model)
        {
            try
            {
                // TODO: Add update logic here
                userService = new UserService();
                var response = userService.Update("api/user/UpdateUser", model);
                response.EnsureSuccessStatusCode();
                return RedirectToAction("List");
            }
            catch
            {
                return View();
            }
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int id)
        {

            userService = new UserService();
            var response = userService.GetResponseMessage("api/user/GetUserById?id=" + id.ToString());
            response.EnsureSuccessStatusCode();
            var user = response.Content.ReadAsAsync<UserModel>().Result;
            if (user == null)
                RedirectToAction("List");

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost]
        public ActionResult Delete(UserModel model)
        {
            try
            {
                // TODO: Add delete logic here
                userService = new UserService();
                var response = userService.Delete("api/user/DeleteUser?id=" + model.Id.ToString(), model);
                response.EnsureSuccessStatusCode();
                return RedirectToAction("List");
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
                //return View();
            }
        }
    }
}
