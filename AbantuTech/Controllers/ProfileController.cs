using AbantuTech.Helpers;
using AbantuTech.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace AbantuTech.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        public ProfileController() : this(new DeactImplementation())
        { }

        private IProfileDeactivation _profile;
        private readonly ApplicationDbContext _context = new ApplicationDbContext();
        private ApplicationUserManager _userManager;
        private DeactImplementation deactImplementation;

        public ProfileController(IProfileDeactivation _profile)
        {
            this._profile = _profile;
        }

        [HttpPost]
        public async Task<ActionResult> UploadPhoto(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                ApplicationDbContext db = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                ApplicationUser currentUser = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                var e = db.Members.Where(m => m.Email == currentUser.Email).FirstOrDefault();
                var username = e.FirstName + "_" + e.Surname;
                var fileExt = Path.GetExtension(file.FileName);
                var fnm = username + ".png";
                if (fileExt.ToLower().EndsWith(".png") || fileExt.ToLower().EndsWith(".jpg") || fileExt.ToLower().EndsWith(".gif"))
                {
                    var filePath = HostingEnvironment.MapPath("~/Content/images/profile/" + fnm);
                    var directory = new DirectoryInfo(HostingEnvironment.MapPath("~/Content/images/profile"));
                    if (directory.Exists == false)
                    {
                        directory.Create();
                    }
                    ViewBag.FilePath = filePath.ToString();
                    file.SaveAs(filePath);
                    return RedirectToAction("ViewProfile", new { Message = ProfileMessage.PhotoUploadSuccess });
                }
                else
                {
                    return RedirectToAction("ViewProfile", new { Message = ProfileMessage.FileExtensionError });
                }
            }
            return RedirectToAction("ViewProfile", new { Message = ProfileMessage.Error });
        }
        [Authorize(Roles = "Admin")]
        public ActionResult ViewDeactRequest(int id)
        {
            var deactivation = _profile.viewDeactRequest(id);
            if (deactivation == null)
                return HttpNotFound();
            return View(deactivation);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult ViewReactRequest(int id)
        {
            var reactivation = _profile.viewReactRequest(id);
            if (reactivation == null)
                return HttpNotFound();
            return View(reactivation);
        }
        public ActionResult ViewProfile(ProfileMessage? message)
        {
            ViewBag.StatusMessage =
                message == ProfileMessage.Error ? "An error has occurred."
                : message == ProfileMessage.PhotoUploadSuccess ? "Your photo has been uploaded."
                : message == ProfileMessage.FileExtensionError ? "Only jpg, png and gif file formats are allowed."
                : "";
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult AllDeactivations()
        {
            return View(_profile.getDeactRequests());
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult InitDeactivation(int id)
        {
            var deactivation = _profile.viewDeactRequest(id);
            if (deactivation != null)
            {
                _profile.initDeactivation(deactivation.Member_ID);
            }
            return RedirectToAction("AllDeactivations");
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult AllReactivations()
        {
            return View(_profile.getReactRequests());
        }
        public ActionResult InitReactivation(int id)
        {
            var reactivation = _profile.viewReactRequest(id);
            if (reactivation != null)
            {
                _profile.initReactivation(reactivation.Member_ID);
            }
            return RedirectToAction("AllReactivations");
        }
        public enum ProfileMessage
        {
            Error,
            PhotoUploadSuccess,
            FileExtensionError
        }
        private async Task<ApplicationUser> GetCurrentUserAsync()
        {
            return await UserManager.FindByIdAsync(User.Identity.GetUserId());
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
    }
}
