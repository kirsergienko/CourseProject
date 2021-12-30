using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CourseProject.Models;
using CourseProject.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CourseProject.Controllers
{
    public class HomeController : Controller
    {
        private DbManager db = new DbManager();

        private LoginManager loginManager = new LoginManager();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string login, string password)
        {
            if (loginManager.IsRegistred(login, password) && !loginManager.IsBlocked(db.Find(login)))
            {
                TempData["CurrentUser"] = login;
                SetCurrentUser();
                return View("MainPage");
            }
            else
            {
                ViewBag.ErrorMessage = loginManager.IsRegistred(login, password) ? "You are blocked." : "Wrong password or login.";
                return View("Login");
            }
        }

        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registration(RegistrationModel user)
        {
            if (ModelState.IsValid)
            {
                db.AddUser(new UserModel
                {
                    UserName = user.UserName,
                    EMail = user.EMail,
                    Password = user.Password
                });
                TempData["CurrentUser"] = user.UserName;
                SetCurrentUser();
                return View("MainPage");
            }
            else return View();
        }

        public ActionResult ListOfUsers()
        {
            var user = SetCurrentUser();
            if (CurrentUserCheck(user))
            {
                return View("ListOfUsers", db.ReturnUsers());
            }
            else
            {
                return View("Login");
            }
        }

        public ActionResult MainPage()
        {
            SetCurrentUser();
            return View("MainPage");
        }

        public ActionResult AddCollection()
        {
            return GenerateAddCollectinoView();
        }

        [HttpPost]
        public ActionResult AddCollection(Collection collection)
        {
            if(ModelState.IsValid)
            {
                UserModel user = SetCurrentUser();
                if (!CurrentUserCheck(user) || !user.IsAdmin)
                {
                    return View("Login");
                }
                collection.UserId = user.Id;
                db.AddCollection(collection);
                return View("ShowCollection");
            }
            else
            {
                return GenerateAddCollectinoView();
            }
        }


        private ActionResult GenerateAddCollectinoView()
        {
            UserModel user = SetCurrentUser();
            if (!CurrentUserCheck(user) || !user.IsAdmin)
            {
                return View("Login");
            }
            ViewBag.UserId = user.Id;
            ViewBag.Themes = new List<string>
            {
                "Books",
                "Movies",
                "Games",
                "Videos",
                "Photos",
                "Cars",
                "Else"
            };
            return View();
        }

        [HttpPost]
        public ActionResult Upload()
        {
            HttpFileCollectionBase files = Request.Files;
            HttpPostedFileBase file = files[0];
            string _apiKey = ConfigurationManager.AppSettings["CloudinaryAPIKey"];
            string _apiSecret = ConfigurationManager.AppSettings["CloudinarySecretKey"];
            string _cloud = ConfigurationManager.AppSettings["CloudinaryAccount"];
            var myAccount = new Account { ApiKey = _apiKey, ApiSecret = _apiSecret, Cloud = _cloud };
            Cloudinary _cloudinary = new Cloudinary(myAccount);
            using (Image img = Image.FromStream(file.InputStream))
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, file.InputStream),
                    Folder = "MyImages",
                    Transformation = new Transformation().Width(img.Width).Height(img.Height).Crop("thumb").Gravity("face")
                };
                var uploadResult = _cloudinary.UploadLarge(uploadParams);
                string uploadedImageUrl = uploadResult.SecureUri.AbsoluteUri;
                return Json(new
                {
                    Result = true,
                    Count = 1,
                    Message = uploadedImageUrl
                });
            }
        }

        [HttpPost]
        public ActionResult Block(List<UserModel> model)
        {
            return EditUser(model, "Block");

        }

        [HttpPost]
        public ActionResult Unblock(List<UserModel> model)
        {
            return EditUser(model, "Unblock");
        }

        [HttpPost]
        public ActionResult Delete(List<UserModel> model)
        {
            return EditUser(model, "Delete");
        }

        [HttpPost]
        public ActionResult ChangeRole(List<UserModel> model)
        {
            return EditUser(model, "ChangeRole");
        }

        private ActionResult EditUser(List<UserModel> model, string method)
        {
            UserModel user = SetCurrentUser();
            if (!CurrentUserCheck(user) || !user.IsAdmin)
            {
                return View("Login");
            }
            switch (method)
            {
                case "Block":
                    foreach (var u in model.Where(x => x.IsChecked))
                    {
                        db.Block(u.Id);
                    }
                    break;
                case "Unblock":
                    foreach (var u in model.Where(x => x.IsChecked))
                    {
                        db.Unblock(u.Id);
                    }
                    break;
                case "Delete":
                    foreach (var u in model.Where(x => x.IsChecked))
                    {
                        db.RemoveUser(u.Id);
                    }
                    break;
                case "ChangeRole":
                    {
                        foreach (var u in model.Where(x => x.IsChecked))
                        {
                            db.ChangeRole(u.Id);
                        }
                    }
                    break;
            }
            if (!CurrentUserCheck(user))
            {
                return View("Login");
            }
            return View("ListOfUsers", db.ReturnUsers().ToList());
        }

        private UserModel SetCurrentUser()
        {
            UserModel user = db.Find((string)TempData["CurrentUser"]);
            TempData.Keep("CurrentUser");
            if (user == null)
            {
                ViewBag.IsAdmin = false;
                ViewBag.CurrentUser = "guest";
                ViewBag.IsNotGuest = false;
                return null;
            }
            ViewBag.IsAdmin = user.IsAdmin;
            ViewBag.CurrentUser = user.UserName;
            ViewBag.IsNotGuest = true;
            return user;
        }

        private bool CurrentUserCheck(UserModel user)
        {
            if (user == null || !loginManager.IsRegistred(user) || user.IsBlocked)
            {
                return false;
            }
            return true;
        }

    }
}