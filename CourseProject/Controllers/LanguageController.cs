using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace CourseProject.Controllers
{
    public class LanguageController : Controller
    {

        public ActionResult Change(string LanguageAbbrevation)
        {
            switch (LanguageAbbrevation)
            {
                case "Russian":
                    LanguageAbbrevation = "ru";
                    break;
                case "Englsh":
                    LanguageAbbrevation = "en";
                    break;
                    default:
                    LanguageAbbrevation="en";
                    break;
            }

            if (LanguageAbbrevation != null)
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(LanguageAbbrevation);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(LanguageAbbrevation);
            }
            HttpCookie cookie = new HttpCookie("Language");
            cookie.Value = LanguageAbbrevation;
            Response.Cookies.Add(cookie);
            return RedirectToAction("MainPage", "Home");
        }
    }
}