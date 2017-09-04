using Abantu_System.Models;
using AbantuTech.Helpers;
using AbantuTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AbantuTech.Controllers
{
    public class ApplyController : Controller
    {
        public ApplyController() : this (new ApplicationImplementation())
        { }

        private IApplication _apply;
        private readonly ApplicationDbContext _context = new ApplicationDbContext();
        public ApplyController(IApplication _apply)
        {
            this._apply = _apply;
        }
      //[HttpPost]
      //  public ActionResult PhoneFormat(AbantuMember tephone)
      //  {
      //      string result=tephone.PhoneNumber;
      //      if ((!string.IsNullOrEmpty(result)) && (result.Length >= 10))
      //          result = string.Format("{0:(123)456-" + new string('7', result.Length - 6) + "}",
      //              Convert.ToInt64(result));
      //      return RedirectToAction("Apply");
      //  }
      
       [HttpGet]
        //public ActionResult PhoneFormat(AbantuMember telphone)
        //{
        //    String phone = telphone.PhoneNumber;
        //    string countrycode = phone.Substring(0, 3);
        //    string areacode = phone.Substring(0, 3);
        //    string number = phone.Substring(6, phone.Length);
        //    phone = "(" + "countrycode" + ")" + areacode + "-" + number;
        //     //return RedirectToAction("AltContact", new { id = member.Member_ID });
        //}

        public ActionResult Create()
        {
            ViewBag.Branch_ID = new SelectList(_context.Branches, "Branch_ID", "Branch_Name");
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Member_ID,Surname,FirstName,DateOfBirth,Gender,Email,PhoneNumber,Type,ZipCode,Province,City,Branch_ID,Address,")] AbantuMember member)
        {
            if (!ModelState.IsValid)
            {
                return View(member);
            }
            _apply.postApplication(member);
            ViewBag.Branch_ID = new SelectList(_context.Branches, "Branch_ID", "Branch_Name");
            return RedirectToAction("AltContact", new { id = member.Member_ID });
        }
        public ActionResult AltContact(int id)
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult AltContact([Bind(Include = "contactID,name,relationship,homePhone,alternativePhone")] EmergencyContact contact, int id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            _apply.postEmergiencyContact(contact, id);
            return RedirectToAction("Application", new { id = contact.Member_ID });
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Application(int id)
        {
            TempData["Message"] = "Your application has been completed. Feedback will be emailed to you soon.";
            var application = _apply.viewApplication(id);
            if (application == null)
                return HttpNotFound();
            return View(application);
        }
        [HttpGet]
        [Authorize(Roles ="Admin")]
        public ActionResult AllApplications()
        {
            return View(_apply.getApplications());
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult AcceptApplication(int id)
        {
            var application = _apply.viewApplication(id);
            if(application != null)
                _apply.acceptApplication(application.Member_ID);
            return RedirectToAction("Edit", "Members", new { id = application.Member_ID});
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult RejectApplication(int id)
        {
            var application = _apply.viewApplication(id);
            if (application != null)
                _apply.rejectApplication(application.Member_ID);
            return RedirectToAction("AllApplications");
        }
    }
}