using karnaCrud.Models;
using Microsoft.AspNetCore.Mvc;

namespace karnaCrud.Controllers
{
    public class HomeController : Controller
    {
        private static List<UserModel> userList = new List<UserModel>();
        private static int nextId = 0;
        public IActionResult Index()
        {
            return View(userList);
        }
        public IActionResult Form() //Form 
        {
            return View();
        }
        [HttpPost]
        public IActionResult Form(UserModel user)
        {
            if(ModelState.IsValid)
            {
                user.ID = ++nextId;
                if (user.ID != 0)
                {
                    var existingUser = userList.FirstOrDefault(u => u.ID == user.ID);
                    if (existingUser != null)
                    {
                        existingUser.Name = user.Name;
                        existingUser.Email = user.Email;
                        existingUser.Phone = user.Phone;
                        existingUser.Gender = user.Gender;
                        existingUser.Address = user.Address;
                        return RedirectToAction("Index");
                    }
                }
                userList.Add(user);
                return RedirectToAction("Index");
            }
            else
            {
                return View("Form", user);
            }
            
        }
        public IActionResult Delete(int id)
        {

            var userToDelete = userList.FirstOrDefault(u => u.ID == id);
            if (userToDelete != null)
            {
                userList.Remove(userToDelete);
            }
            return RedirectToAction("Index");

        }
        
        public IActionResult Edit(int id)
        {
            var existingUser=userList.FirstOrDefault(u => u.ID == id);
            if(existingUser != null) 
            {
                return View("Form",existingUser);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Edit(UserModel model)
        {
            var existingUser=userList.FirstOrDefault(u => u.ID ==model.ID);
            if (existingUser != null)
            {
                existingUser.Name=model.Name;
                existingUser.Email=model.Email;
                existingUser.Phone=model.Phone;
                existingUser.Gender = model.Gender;
                existingUser.Address=model.Address;
                return RedirectToAction("Index");
            }
            return NotFound();
        }

    }
}
