using karnaCrud.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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
        public IActionResult Form(int? id) //Form 
        {
            if(id.HasValue)
            {
                var existingUser = userList.FirstOrDefault(u => u.ID == id.Value);
                if (existingUser != null)
                {
                    return View(existingUser);
                }
                return NotFound();
            }

            return View(new UserModel());
        }
        [HttpPost]
        public IActionResult Form(UserModel user)
        {
            if(ModelState.IsValid)
            {
               if(user.ID==0)
               {
                    user.ID = ++nextId;
                    userList.Add(user);
               }
               else
               {
                    var existingUser = userList.FirstOrDefault(u => u.ID == user.ID);
                    if (existingUser != null)
                    {
                        existingUser.Name = user.Name;
                        existingUser.Email = user.Email;
                        existingUser.Phone = user.Phone;
                        existingUser.Gender = user.Gender;
                        existingUser.Address = user.Address;
                        existingUser.Designation = user.Designation;
                        
                    }
                }
                return RedirectToAction("Index");
            }
            else
            {
                return View(user);
            }           
        }
        public JsonResult IsEmailAvailable(string email,int? id)
        {
            bool isAvailable;
            if(id.HasValue)
            {
                isAvailable=!userList.Any(u => u.Email == email && u.ID !=id.Value);
            }
            else
            {
                isAvailable = !userList.Any(u => u.Email == email);
            }
            return Json(isAvailable);
        }

        public JsonResult isPhoneAvailable(string phone,int? id)
        {
            bool isAvailable;
            if(id.HasValue)
            {
                isAvailable=!userList.Any(u => u.Phone == phone && u.ID !=id.Value);
            }
            else
            {
                isAvailable =!userList.Any(u =>u.Phone == phone);
            }
            return Json(isAvailable);
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
            return RedirectToAction("Form", new { id });
        }

        [HttpPost]
        public IActionResult Edit(UserModel model)
        {
            return RedirectToAction("Form",new {id=model.ID});
        }

    }
}
