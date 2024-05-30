using karnaCrud.Models;
using karnaCrud.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using System.Reflection;


namespace karnaCrud.Controllers
{
    public class HomeController : Controller
    {
        private static List<UserModel> userList = new List<UserModel>();
        private static int nextId = 0;
        private readonly IWebHostEnvironment _environment;
        private readonly StaticDataService _staticDataService;

        public HomeController(IWebHostEnvironment environment, StaticDataService staticDataService)
        {
            _environment = environment;
            _staticDataService = staticDataService;
        }
        public IActionResult Index()
        {
            return View(userList);
        }
        public IActionResult Form(int? id) //Form 
        {
            var viewModel = new UserFormViewModel
            {
                User = id.HasValue ? userList.FirstOrDefault(u => u.ID == id.Value) ?? new UserModel() : new UserModel(),
                Countries =_staticDataService.GetCountries(),
                States=_staticDataService.GetStates(),
                Cities=_staticDataService.GetCities()
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Form(UserFormViewModel viewModel, string[] hobbies, IFormFile imageFile)
        {
            var user = viewModel.User;
            if (ModelState.IsValid)
            {
                var existingUser = userList.FirstOrDefault(u => u.ID == user.ID);
                if (existingUser != null && !string.IsNullOrEmpty(existingUser.ImagePath))
                {
                    string oldImagePath = Path.Combine(_environment.WebRootPath, existingUser.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                if (imageFile != null && imageFile.Length > 0)
                {
                    var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();                    
                    if (extension != ".jpg" && extension != ".jpeg")
                    {
                        ModelState.AddModelError(nameof(user.ImageFile), "Only JPG or JPEG files are allowed.");
                        return View(viewModel);
                    }
                    if (imageFile.Length > 2 * 1024 * 1024) // 2MB in bytes
                    {
                        ModelState.AddModelError(nameof(user.ImageFile), "File size must not exceed 2MB.");
                        return View(viewModel);
                    }
                    
                    // Save the uploaded image file
                    string uploadsFolder = Path.Combine(_environment.WebRootPath, "images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        imageFile.CopyTo(fileStream);
                    }
                    user.ImagePath = "/images/" + uniqueFileName;
                }
                else if(existingUser != null)
                {
                    user.ImagePath = existingUser.ImagePath; //Preserve the existing image path if no new image is uploaded
                }
                user.Hobbies = hobbies.ToList();
                user.Email = user.Email.ToLower();

                if (user.ID==0)
                {
                    if (!IsEmailAvailable(user.Email, null))
                    {
                        ModelState.AddModelError(nameof(user.Email), "Email already exists!");
                        return View(viewModel);
                    }
                    user.ID = ++nextId;
                    userList.Add(user);
                }
               else
               {
                    if (existingUser != null)
                    {
                        if (existingUser.Email.ToLower() != user.Email)
                        {
                            // Email has changed, perform validation
                            if (!IsEmailAvailable(user.Email, user.ID))
                            {
                                ModelState.AddModelError(nameof(user.Email), "Email already exists!");
                                return View(viewModel);
                            }
                        }
                        existingUser.Name = user.Name;
                        existingUser.Email = user.Email;
                        existingUser.Phone = user.Phone;
                        existingUser.Gender = user.Gender;
                        existingUser.Address = user.Address;
                        existingUser.Designation = user.Designation;
                        existingUser.Hobbies = user.Hobbies;
                        existingUser.ImagePath = user.ImagePath;
                        existingUser.CountryId=user.CountryId;
                        existingUser.StateId=user.StateId;
                        existingUser.CityId=user.CityId;
                    }
                }
                return RedirectToAction("Index");
            }
            else
            {
                return View(viewModel);
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
                if (!string.IsNullOrEmpty(userToDelete.ImagePath))
                {
                    string filePath = Path.Combine(_environment.WebRootPath, userToDelete.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
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
