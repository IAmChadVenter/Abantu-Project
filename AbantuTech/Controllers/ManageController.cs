using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using AbantuTech.Models;
using Abantu_System.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net;
using System.Data;
using System.Data.Entity;
//using SelectPdf;

namespace AbantuTech.Controllers
{

    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        public ManageController() { }
        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;

        }
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
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

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : message == ManageMessageId.DeactProfileSuccess ? "Your request for profile deactivation has been sent."
                : message == ManageMessageId.ReactProfileSuccess ? "Your request for profile reactivation has been sent."
                : message == ManageMessageId.DeactCancelSuccess ? "You have successfully cancelled the profile deactivation request."
                : message == ManageMessageId.ReactCancelSuccess ? "You have successfully cancelled the profile reactivation request."
                : "";

            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId),
                isProfileActive = HasActiveProfile(),
                isDeactRequested = HasRequestedDeact(),
                isReactRequested = HasRequestedReact(),
                isDeactApproved = isDeactApproved(),
                isReactApproved = isReactApproved()
            };
            return View(model);
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }
        private async Task<ApplicationUser> GetCurrentUserAsync()
        {
            return await UserManager.FindByIdAsync(User.Identity.GetUserId());
        }
        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }

        //
        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }
        public ActionResult RequestDeact()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RequestDeact(IndexViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                var mem = _context.Members.Where(m => m.Email == user.Email).FirstOrDefault();
                if (mem != null && model.isProfileActive == true)
                {
                    postDeactRequest(mem);
                    return RedirectToAction("Index", new { Message = ManageMessageId.DeactProfileSuccess });
                }
                else
                {
                    return RedirectToAction("Index", new { Message = ManageMessageId.Error });
                }
            }
            return View(model);
        }
        public ActionResult RequestReact()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RequestReact(AbantuMember member)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                var mem = _context.Members.Where(m => user.Email == member.Email).FirstOrDefault();
                if (mem != null && HasActiveProfile() == false)
                {
                    postReactRequest(mem);
                    return RedirectToAction("Index", new { Message = ManageMessageId.ReactProfileSuccess });
                }
                else
                {
                    return RedirectToAction("Index", new { Message = ManageMessageId.Error });
                }
            }
            return View(member);

        }
        public async Task<ActionResult> CancelDeact(AbantuMember member)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                var mem = _context.Members.Where(x => x.Email == user.Email && x.deactApproved == false).FirstOrDefault();
                if (mem != null && HasRequestedDeact() && HasActiveProfile())
                {
                    member.isDeactRequested = false;
                    mem.isDeactRequested = member.isDeactRequested;
                    _context.SaveChanges();
                    return RedirectToAction("Index", new { Message = ManageMessageId.DeactCancelSuccess });
                }
                else
                {
                    return RedirectToAction("Index", new { Message = ManageMessageId.Error });
                }
            }
            return View(member);
        }
        public async Task<ActionResult> CancelReact(AbantuMember member)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                var mem = _context.Members.Where(x => x.Email == user.Email).FirstOrDefault();
                if (mem != null && HasRequestedReact() && HasActiveProfile() == false)
                {
                    member.isReactRequested = false;
                    mem.isReactRequested = member.isReactRequested;
                    _context.SaveChanges();
                    return RedirectToAction("Index", new { Message = ManageMessageId.ReactCancelSuccess });
                }
                else
                {
                    return RedirectToAction("Index", new { Message = ManageMessageId.Error });
                }
            }
            return View(member);
        }
        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasActiveProfile()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            ApplicationUser currentUser = UserManager.FindById(User.Identity.GetUserId());
            var mem = _context.Members.FirstOrDefault(m => m.Email == currentUser.Email);
            if (mem != null && mem.isProfileActive == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool HasRequestedDeact()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            ApplicationUser currentUser = UserManager.FindById(User.Identity.GetUserId());
            var mem = _context.Members.FirstOrDefault(m => m.Email == currentUser.Email);
            if (mem != null && mem.isDeactRequested == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool HasRequestedReact()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            ApplicationUser currentUser = UserManager.FindById(User.Identity.GetUserId());
            var mem = _context.Members.FirstOrDefault(m => m.Email == currentUser.Email);
            if (mem != null && mem.isReactRequested == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool isDeactApproved()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            ApplicationUser currentUser = UserManager.FindById(User.Identity.GetUserId());
            var mem = _context.Members.FirstOrDefault(m => m.Email == currentUser.Email);
            if (mem != null && mem.deactApproved == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool isReactApproved()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            ApplicationUser currentUser = UserManager.FindById(User.Identity.GetUserId());
            var mem = _context.Members.FirstOrDefault(m => m.Email == currentUser.Email);
            if (mem != null && mem.reactApproved == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void postDeactRequest(AbantuMember member)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            ApplicationUser currentUser = UserManager.FindById(User.Identity.GetUserId());
            AbantuMember mem = _context.Members.Where(m => m.Email == currentUser.Email && m.isDeactRequested == false).FirstOrDefault();
            if (mem != null)
            {
                member.isDeactRequested = true;
                mem.isDeactRequested = member.isDeactRequested;
                Save();
            }
        }

        public void postReactRequest(AbantuMember member)
        {
            AbantuMember mem = _context.Members.Where(m => m.Email == member.Email && m.isProfileActive == false && m.isReactRequested == false).FirstOrDefault();
            member.isReactRequested = true;
            mem.isReactRequested = member.isReactRequested;
            Save();
        }
        public void Save()
        {
            _context.SaveChanges();
        }
        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            DeactProfileSuccess,
            ReactProfileSuccess,
            DeactCancelSuccess,
            ReactCancelSuccess,
            Error
        }

        #endregion

        [Authorize(Roles = "Admin")]
        public ActionResult AdminMenu()
        {
            return View();
        }

        [Authorize(Roles = "Intern")]
        public ActionResult InternMenu()
        {
            return View();
        }

        [Authorize(Roles = "Volunteer")]
        public ActionResult VolunteerMenu()
        {
            return View();
        }




        private ApplicationDbContext p = new ApplicationDbContext();
        
        public ActionResult report()
        {
            return View();
        }
        public ActionResult BudgetReport(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budget budget = p.Budgets.Find(id);
            if (budget == null)
            {
                return HttpNotFound();
            }
            var i = System.Web.HttpContext.Current.Request.Url.PathAndQuery;
            return View(budget);
        }
        
        public ActionResult DonReport(int? id)
        {
            return View(p.DonationAmount.ToList());
        }

        public ActionResult eventReport(int id)
        {
            var @event = p.Events.FirstOrDefault(p => p.Event_ID == id);
            if (@event != null)
            {
                var eventMember = p.EventMembers
                    .Include(p => p.AbantuMember)
                    .Where(x => x.Event_ID == @event.Event_ID)
                    .ToList();
                return View(eventMember);
            }
            return View();
        }        
        //public ActionResult SubmitAction(int? id)
        //{
        //    // instantiate a html to pdf converter object
        //    HtmlToPdf converter1 = new HtmlToPdf();

        //    var i = HttpContext.Request.UrlReferrer.ToString();
        //    // create a new pdf document converting an url
        //    PdfDocument doc1 = converter1.ConvertUrl(i);

        //    // save pdf document
        //    byte[] pdf = doc1.Save();

        //    // close pdf document
        //    doc1.Close();

        //    // return resulted pdf document
        //    FileResult fileResult = new FileContentResult(pdf, "application/pdf");
        //    fileResult.FileDownloadName = "Document.pdf";
        //    return fileResult;
        //}

    }
}