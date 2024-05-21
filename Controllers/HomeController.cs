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
        public IActionResult Form(UserModel user, string[] hobbies)
        {
            if(ModelState.IsValid)
            {
                user.Hobbies = hobbies.ToList();
                user.Email = user.Email.ToLower();
                if (user.ID==0)
               {
                    if (!IsEmailAvailable(user.Email, null))
                    {
                        ModelState.AddModelError(nameof(user.Email), "Email already exists!");
                        return View(user);
                    }
                    user.ID = ++nextId;
                    userList.Add(user);
               }
               else
               {
                    var existingUser = userList.FirstOrDefault(u => u.ID == user.ID);
                    if (existingUser != null)
                    {
                        if (existingUser.Email.ToLower() != user.Email)
                        {
                            // Email has changed, perform validation
                            if (!IsEmailAvailable(user.Email, user.ID))
                            {
                                ModelState.AddModelError(nameof(user.Email), "Email already exists!");
                                return View(user);
                            }
                        }
                        existingUser.Name = user.Name;
                        existingUser.Email = user.Email;
                        existingUser.Phone = user.Phone;
                        existingUser.Gender = user.Gender;
                        existingUser.Address = user.Address;
                        existingUser.Designation = user.Designation;
                        existingUser.Hobbies = user.Hobbies;
                    }
                }
                return RedirectToAction("Index");
            }
            else
            {
                return View(user);
            }           
        }
        public bool IsEmailAvailable(string email, int? id)
        {
            return !userList.Any(u => u.Email.ToLower() == email.ToLower() && u.ID != id.Value);
        }

        public JsonResult isPhoneAvailable(string phone,int? id)
        {
            bool isAvailable;
            isAvailable=!userList.Any(u => u.Phone == phone && u.ID !=id.Value);
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
