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
using CourseProject.Resources;

namespace CourseProject.Controllers
{
    public class HomeController : Controller
    {
        private DbManager db = new DbManager();

        private LoginManager loginManager = new LoginManager();

        public ActionResult Theme(bool _checked)
        {
            HttpCookie cookie = new HttpCookie("Theme");
            cookie.Value = _checked.ToString();
            Response.Cookies.Add(cookie);
            return MainPage();
        }

        public ActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        public ActionResult Login(string login, string password)
        {
            if (loginManager.IsRegistred(login, password) && !loginManager.IsBlocked(db.Find(login)))
            {
                HttpCookie cookie = new HttpCookie("User");
                cookie.Value = login;
                Response.Cookies.Add(cookie);
                SetCurrentUser();
                return MainPage();
            }
            else
            {
                ViewBag.ErrorMessage = loginManager.IsRegistred(login, password) ? Language.You_are_blocked : Language.Wrong_password_or_login;
                return Login();
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
                HttpCookie cookie = new HttpCookie("User");
                cookie.Value = user.UserName;
                Response.Cookies.Add(cookie);
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
                return Login();
            }
        }

        public ActionResult MainPage()
        {
            SetCurrentUser();
            return View("MainPage");
        }

        public ActionResult LastAddedItems()
        {
            return PartialView(db.GetLastAddedItems());
        }

        public ActionResult TagCloud()
        {
            return PartialView(db.GetTagCloud());
        }

        public ActionResult BiggestCollections()
        {
            return PartialView(db.GetBiggestCollections());
        }

        public ActionResult ShowUser(int id)
        {
            SetCurrentUser();
            return View("ShowUser", db.Find(id));
        }

        public ActionResult MyProfile()
        {
            var user = SetCurrentUser();
            if (CurrentUserCheck(user))
            {
                return ShowUser(user.Id);
            }
            else
            {
                return Login();
            }
        }

        public ActionResult GetCollections(int userid)
        {
            return PartialView("GetCollections", db.GetCollections(userid));
        }

        public ActionResult AddCollection()
        {
            return GenerateAddCollectionView();
        }

        public ActionResult FoundItems(string search)
        {
            return PartialView("FoundItems", db.GetItems(search));
        }
        public ActionResult SortedFoundItems(string search, string sort)
        {
            var items = db.GetItems(search);
            Sorting(ref items, sort);
            return PartialView("FoundItems", items);
        }

        public ActionResult FoundItemsTag(string search)
        {
            return PartialView("FoundItems", db.GetItemsByTag(search));
        }
        public ActionResult SortedFoundItemsTag(string search, string sort)
        {
            var items = db.GetItemsByTag(search);
            Sorting(ref items, sort);
            return PartialView("FoundItems", items);
        }

        [HttpPost]
        public ActionResult Search(string search)
        {
            return View("SearchResult", model: search);
        }

   
        public ActionResult SearchTag(string search)
        {
            return View("SearchResultTag", model: search);
        }
        public ActionResult AddItem(int id)
        {
            var collection = db.GetCollection(id);
            UserModel user = SetCurrentUser();
            if (!CurrentUserCheck(user) || user.Id != collection.UserId)
            {
                if (user == null || user.IsAdmin == false)
                    return Login();
            }
            return View(InitialAddItemModel(collection));
        }

        [HttpPost]
        public ActionResult AddItem(Item item)
        {
            UserModel user = SetCurrentUser();
            var collection = db.GetCollection(item.CollectionId);
            if (!CurrentUserCheck(user) || user.Id != collection.UserId)
            {
                if (user == null || user.IsAdmin == false)
                    return Login();
            }
            db.AddItem(item);
            db.AddTags(item.Tags, item.Id);
            return RedirectToAction("ShowCollection", new { id = item.CollectionId });
        }

        public ActionResult EditItem(int id)
        {
            var item = db.GetItem(id);
            var collection = db.GetCollection(item.CollectionId);
            UserModel user = SetCurrentUser();
            if (!CurrentUserCheck(user) || user.Id != collection.UserId)
            {
                if (user == null || user.IsAdmin == false)
                    return Login();
            }
            return View(item);
        }

        public ActionResult ShowItem(int id)
        {
            var item = db.GetItem(id);
            var collection = db.GetCollection(item.CollectionId);
            UserModel user = SetCurrentUser();
            ViewBag.Title = collection.Title;
            if (user != null)
            {
                if (item.Likes.Any(x => x.UserId == user.Id))
                {
                    ViewBag.Image = "https://res.cloudinary.com/de7r0q8df/image/upload/v1641633066/MyImages/Screenshot_3_ihqjtg.png";
                }
                else
                {
                    ViewBag.Image = "https://res.cloudinary.com/de7r0q8df/image/upload/v1641633066/MyImages/Screenshot_2_ki0css.png";
                }
            }
            return View("ShowItem", item);
        }

        [HttpPost]
        public ActionResult Comment(string comment, int id)
        {
            var user = SetCurrentUser();
            if (!CurrentUserCheck(user))
            {
                return Login();
            }
            db.AddComment(new Comment { ItemId = id, Text = comment, UserName = user.UserName, Commented = DateTime.Now });
            return ShowItem(id);
        }

        [HttpPost]
        public ActionResult Like(int id)
        {
            var user = SetCurrentUser();
            db.AddLike(new Like { ItemId = id, UserId = user.Id });
            return ShowItem(id);
        }

        public ActionResult Comments(int id)
        {
            return PartialView("Comments", db.GetItem(id));
        }

        [HttpPost]
        public ActionResult EditItem(Item item)
        {
            UserModel user = SetCurrentUser();
            if (!CurrentUserCheck(user) || user.Id != item.CollectionId)
            {
                if (user == null || user.IsAdmin == false)
                    return Login();
            }
            db.EditItem(item);
            return RedirectToAction("ShowCollection", new { id = item.CollectionId });
        }

        private Item InitialAddItemModel(Collection collection)
        {
            Item item = new Item();
            item.CollectionId = collection.Id;
            item.BoolValues = new List<BoolValue>();
            for (int i = 0; i < collection.BoolValuesCount; i++)
            {
                item.BoolValues.Add(new BoolValue { });
            }
            item.StringValues = new List<StringValue>();
            for (int i = 0; i < collection.StringValuesCount; i++)
            {
                item.StringValues.Add(new StringValue { });
            }
            item.IntValues = new List<IntValue>();
            for (int i = 0; i < collection.IntValuesCount; i++)
            {
                item.IntValues.Add(new IntValue { });
            }
            item.DateValues = new List<DateValue>();
            for (int i = 0; i < collection.DateValuesCount; i++)
            {
                item.DateValues.Add(new DateValue { });
            }
            ViewBag.Title = collection.Title;
            return item;
        }

        public ActionResult DeleteCollection(int id)
        {
            UserModel user = SetCurrentUser();
            if (!CurrentUserCheck(user) || user.Id != id)
            {
                if (user == null || user.IsAdmin == false)
                    return Login();
            }
            db.RemoveCollection(id);
            return MainPage();
        }

        public ActionResult DeleteItem(int id)
        {
            var item = db.GetItem(id);
            var collection = db.GetCollection(item.CollectionId);
            UserModel user = SetCurrentUser();
            if (!CurrentUserCheck(user) || user.Id != collection.UserId)
            {
                if (user == null || user.IsAdmin == false)
                    return Login();
            }
            db.RemoveItem(id);
            ViewBag.CurrentUserId = user.Id;
            ViewBag.Items = db.GetItems(collection.Id);
            return RedirectToAction("ShowCollection", new { id = item.CollectionId });
        }

        public ActionResult ShowCollection(int id)
        {
            var user = SetCurrentUser();
            if (user != null)
            {
                ViewBag.CurrentUserId = user.Id;
            }
            else ViewBag.CurrentUserId = -1;
            ViewBag.Items = db.GetItems(id);
            return View("ShowCollection", db.GetCollection(id));
        }

        public ActionResult Items(int collectionId)
        {
            var user = SetCurrentUser();
            if (user != null)
            {
                ViewBag.CurrentUserId = user.Id;
            }
            else ViewBag.CurrentUserId = -1;
            ViewBag.CollectionId = db.GetCollection(collectionId).UserId;
            var items = db.GetItems(collectionId);
            return PartialView("Items", items);
        }

        public ActionResult SortedItems(int collectionId, string sort)
        {
            var user = SetCurrentUser();
            if (user != null)
            {
                ViewBag.CurrentUserId = user.Id;
            }
            else ViewBag.CurrentUserId = -1;
            ViewBag.CollectionId = collectionId;
            var items = db.GetItems(collectionId);
            Sorting(ref items, sort);
            return PartialView("Items", items);
        }


        private void Sorting(ref List<Item> items, string sort)
        {
            switch (sort)
            {
                case "Bydate":
                    items = items.OrderBy(x => x.LastChanged).ToList();
                    break;
                case "Bylikes":
                    items = items.OrderByDescending(x => x.Likes.Count).ToList();
                    break;
                case "Bydatedescending":
                    items = items.OrderByDescending(x => x.LastChanged).ToList();
                    break;
                case "Bylikesdescending":
                    items = items.OrderBy(x => x.Likes.Count).ToList();
                    break;
                case "Подате":
                    items = items.OrderBy(x => x.LastChanged).ToList();
                    break;
                case "Полайкам":
                    items = items.OrderByDescending(x => x.Likes.Count).ToList();
                    break;
                case "Податепоубыванию":
                    items = items.OrderByDescending(x => x.LastChanged).ToList();
                    break;
                case "Полайкампоубыванию":
                    items = items.OrderBy(x => x.Likes.Count).ToList();
                    break;
                default:
                    break;
            }
        }

        public ActionResult EditCollection(int id)
        {
            UserModel user = SetCurrentUser();
            var collection = db.GetCollection(id);
            if (!CurrentUserCheck(user) || user.Id != id)
            {
                if (user == null || user.IsAdmin == false)
                    return Login();
            }
            InitalViewBagforAddCollection(user);
            return View("EditCollection", collection);
        }

        [HttpPost]
        public ActionResult EditCollection(Collection collection)
        {
            ModelState.Remove("Title");
            if (ModelState.IsValid)
            {
                UserModel user = SetCurrentUser();
                if (!CurrentUserCheck(user) || user.Id != collection.Id)
                {
                    if (user == null || user.IsAdmin == false)
                        return Login();
                }
                db.EditCollection(collection);
                ViewBag.CurrentUserId = user.Id > 0 ? user.Id : -1;
                ViewBag.Items = db.GetItems(collection.Id);
                return View("ShowCollection", db.GetCollection(collection.Id));
            }
            else
            {
                return EditCollection(collection.Id);
            }
        }

        [HttpPost]
        public ActionResult AddCollection(Collection collection)
        {
            if (ModelState.IsValid)
            {
                UserModel user = SetCurrentUser();
                if (!CurrentUserCheck(user))
                {
                    if (user == null || user.IsAdmin == false)
                        return Login();
                }
                db.AddCollection(collection);
                ViewBag.CurrentUserId = user.Id > 0 ? user.Id : -1;
                ViewBag.Items = db.GetItems(collection.Id);
                return RedirectToAction("ShowCollection", new { id = collection.Id });
            }
            else
            {
                return GenerateAddCollectionView();
            }
        }

        public ActionResult AddCollectionFromProfile(int userid)
        {
            var user = db.Find(userid);
            InitalViewBagforAddCollection(user);
            return View("AddCollection");
        }

        private ActionResult GenerateAddCollectionView()
        {
            UserModel user = SetCurrentUser();
            if (!CurrentUserCheck(user))
            {
                return Login();
            }
            InitalViewBagforAddCollection(user);
            return View("AddCollection");
        }

        private void InitalViewBagforAddCollection(UserModel user)
        {
            ViewBag.UserId = user.Id;
            ViewBag.Themes = db.GetThemes();
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
                string uploadedImageUrl = uploadResult.SecureUrl.AbsoluteUri;
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
                return Login();
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

        public ActionResult BlockSingle(int id)
        {
            UserModel user = SetCurrentUser();
            if (!CurrentUserCheck(user) || !user.IsAdmin)
            {
                return Login();
            }
            db.Block(id);
            return ShowUser(id);
        }

        public ActionResult UnblockSingle(int id)
        {
            UserModel user = SetCurrentUser();
            if (!CurrentUserCheck(user) || !user.IsAdmin)
            {
                return Login();
            }
            db.Unblock(id);
            return ShowUser(id);
        }

        public ActionResult DeleteSingle(int id)
        {
            UserModel user = SetCurrentUser();
            if (!CurrentUserCheck(user) || !user.IsAdmin)
            {
                return Login();
            }
            db.RemoveUser(id);
            return MainPage();
        }

        public ActionResult ChangeRoleSingle(int id)
        {
            UserModel user = SetCurrentUser();
            if (!CurrentUserCheck(user) || !user.IsAdmin)
            {
                return Login();
            }
            db.ChangeRole(id);
            return ShowUser(id);
        }

        private UserModel SetCurrentUser()
        {
            HttpCookie cookie = HttpContext.Request.Cookies.Get("User");
            UserModel user;
            if (cookie != null)
            {
                user = db.Find(cookie.Value);
            }
            else
            {
                user = null;
            }
            if (user == null)
            {
                ViewBag.IsAdmin = false;
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

        public JsonResult Tags(string search)
        {
            List<string> vs = search.Split(' ').ToList();
            if (vs.Last() != String.Empty)
            {
                var Tags = db.GetTagList().Where(x => x.TagName.StartsWith(vs.Last())).ToList();
                vs.Remove(vs.Last());
                var Item = new { Tag = Tags, Input = String.Join(" ", vs.ToArray()) };
                return new JsonResult { Data = Item, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else return new JsonResult { Data = null, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

    }
}