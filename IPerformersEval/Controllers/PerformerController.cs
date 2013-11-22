using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IPerformersEval.DAL;
using IPerformersEval.Models;


namespace IPerformersEval.Controllers
{
    public class PerformerController : Controller
    {
        PerformersDB _db = new PerformersDB();

        [HttpGet]
        //public virtual JsonResult IsUserNameAvailable(string contactEmailPrimary, int userAccountId)
        public JsonResult IsUserNameAvailable(string contactEmailPrimary)//, int userAccountId)
        {
            return Json(true, JsonRequestBehavior.AllowGet);// return true -> pass validation. Anything else will cause validation to fail.
            //return Json("This Email Address has already been registered", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            //var model = from 
            //return View(_db.Performers.ToList());
            return View(new Performer());
        }


        //
        // POST: /Performer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Performer performer)
        {
            if (ModelState.IsValid)
            {
                var performerInDataBase = _db.Performers.Where(p => p.Performer_TIN.Equals(performer.Performer_TIN));
                try
                {
                    Performer p = performerInDataBase.First();     // any record in the DB? throw exception if 

                    if (p.Performer_TIN.Equals(performer.Performer_TIN, StringComparison.OrdinalIgnoreCase)) // already there's a record with the same Email
                    {
                        ModelState.AddModelError("", "The Service Agreement exists in the database.");
                        return View(performer);
                    }
                }
                catch (InvalidOperationException)   // it means no email exists so we can modify
                {
                    _db.Performers.Add(performer);
                    _db.SaveChanges();

                    // update table FormsStatus
                    UpdateFormStatus(performer);

                    //TempData["OBJECT"] = performer;
                    TempData["STATUS"] = "Success!";
                    return RedirectToAction("Index", "FormsStatus");//, new { TIN = performer.Performer_TIN });
                }
           }

            return RedirectToAction("Index", "FormsStatus");//, new { TIN = performer.Performer_TIN });
        }

        //
        // GET: /Performer/Create
        public ActionResult Create(string FullName = null, string PerformerEmail = null, string s = null)
        {
            Performer performer = new Performer();

            //{ 
            //                FullName = "", 
            //                FullName2 = "", 
            //                Addr1 = "", 
            //                Addr2 = "", 
            //                City = "", 
            //                State = "", 
            //                Zip = "",
            //                PerformerEmail = "",
            //                ProgramName = "", 
            //                ContractDate = DateTime.Now,
            //                Payment = 0,
            //                PersonalInfoAckSig = false,
            //                OfficialAckSig = false,
            //                SignedDate = DateTime.Now, 
            //                PrintName = "", 
            //                Title = ""
            //};

            if (FullName != null)
                performer.FullName = FullName;
            if (PerformerEmail != null)
                performer.PerformerEmail = PerformerEmail;
            if (s != null)
                performer.Performer_TIN = s;
            return View(performer);
        }

        public ActionResult Details(string s = "")
        {
            if (s.Length > 0)
            {
                var performers = _db.Performers.Where(p => (p.Performer_TIN.Equals(s)));
                int cnt = performers.Count();
                if (cnt == 1)
                {
                    Performer performer = performers.First();
                    return View("Details", performer);
                }
                else
                if (cnt == 0)
                {
                    //TempData["OBJECT"] = null;
                    TempData["STATUS"] = "No Performer Found";
                    return RedirectToAction("Index", "FormsStatus");
                }
                else
                {
                    //TempData["OBJECT"] = null;
                    TempData["STATUS"] = "Error: more than one Performer. Please see the Admin.";
                    return RedirectToAction("Index", "FormsStatus");
                }
            }
            //TempData["OBJECT"] = null;
            TempData["STATUS"] = "No Performer Found";
            return RedirectToAction("Index", "FormsStatus");
       }

        private void UpdateFormStatus(Performer performer)
        {
            var formsStatus = _db.AllFormsStatus.Where(w => w.TIN.Equals(performer.Performer_TIN));  // any record that had been create by Vendor (check by using TIN)
            try
            {
                FormsStatus f = formsStatus.First();     // any record in the DB? throw exception if 

                if (f.TIN.Equals(performer.Performer_TIN)) // already there's a record with the same TIN
                {
                    // FormsStatus formStatus = new FormsStatus();
                    f.Performer_TIN = performer.Performer_TIN;
                    _db.Entry(f).State = System.Data.EntityState.Modified;
                    _db.SaveChanges();
                }
            }
            catch (InvalidOperationException)   // it means no TIN exists so we can modify
            {
                FormsStatus f = new FormsStatus();
                f.Performer_TIN = performer.Performer_TIN;
                _db.AllFormsStatus.Add(f);
                _db.SaveChanges();
           }
        }

        //[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSave(Performer performer)
        {
            _db.Entry(performer).State = System.Data.EntityState.Modified;
            if (ModelState.IsValid)
            {
                var performerInDataBase = _db.Performers.Where(p => p.PerformerEmail.Equals(performer.PerformerEmail));
                try
                {
                    Performer p = performerInDataBase.First();

                    //--- Still have to check for the case where user wants to modify the email which is a duplicate
                    if (!p.PerformerID.Equals(performer.PerformerID))     
                    {
                        //ModelState.AddModelError("PerformerEmail", "Modification is not saved: The Email already exists in the database.");
                        TempData["STATUS"] = "Modification is not saved: the Email already exists in the database.";
                        return RedirectToAction("Index", "FormsStatus");//, new { TIN = performer.Performer_TIN });
                    }

                    _db.SaveChanges();

                    // update table FormsStatus
                    UpdateFormStatus(performer);

                    //TempData["OBJECT"] = performer;
                    TempData["STATUS"] = "Modification is saved successfully.";
                    return RedirectToAction("Index", "FormsStatus");//, new { TIN = performer.Performer_TIN });
                }
                catch (InvalidOperationException e)   // it means modifying no record with the email (email of an existing record has been modified): we should let the user update to new email
                {
                    string err = e.ToString();

                    _db.SaveChanges();

                    //TempData["OBJECT"] = performer;
                    TempData["STATUS"] = "Modification is saved successfully.";
                    return RedirectToAction("Index", "FormsStatus");//, new { TIN = performer.Performer_TIN });
                }
            }
            
            return View("Edit");//performer);
        }

        // this is get method
        public ActionResult Edit(string FullName = "", string PerformerEmail = "", string s = "")  //string PerformerName = "AClibAdmin", string PerformerEmail = "AClibAdmin")
        {
            // Taking care of the Admin/Manager who has been given the backdoor Name & email
            //if (FullName.Equals("AClibAdmin", StringComparison.OrdinalIgnoreCase) && PerformerEmail.Equals("AClibAdmin", StringComparison.OrdinalIgnoreCase))

            if (s.Equals("AClibAdmin", StringComparison.OrdinalIgnoreCase))
            {
                return View("ListOfPerformers", _db.Performers.ToList());
            }

            if (FullName.Equals("AClibAdmin", StringComparison.OrdinalIgnoreCase) && PerformerEmail.Equals("AClibAdmin", StringComparison.OrdinalIgnoreCase))
            {
                return View("ListOfPerformers", _db.Performers.ToList());
                //return View("ListOfPerformers", _db.Performers.ToList());
            }

            if (s.Length > 0)
            {
                var performers = _db.Performers.Where(p => (p.Performer_TIN.Equals(s)));
                int cnt = performers.Count();
                if (cnt == 1)
                {
                    Performer performer = performers.First();
                    return View(performer);
                }
                else
                if (cnt == 0)
                {
                    //TempData["OBJECT"] = null;
                    TempData["STATUS"] = "No Performer Found";
                    return RedirectToAction("Index", "FormsStatus");
                }
                else
                {
                    TempData["OBJECT"] = null;
                    TempData["STATUS"] = "Error: more than one Performer. Please see the Admin.";
                    return RedirectToAction("Index", "FormsStatus");
                }

            }

            if (FullName != null && PerformerEmail != null)         // search by FullName AND Email
            {
                var performers = _db.Performers.Where(p => (p.FullName.Equals(FullName) && p.PerformerEmail.Equals(PerformerEmail)));
                int cnt = performers.Count();
                if (cnt == 1)
                {
                    Performer performer = performers.First();
                    return View(performer);
                }
                else
                if (cnt == 0)
                {
                    TempData["OBJECT"] = null;
                    TempData["STATUS"] = "No Performer Found";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["OBJECT"] = null;
                    TempData["STATUS"] = "Error: more than one Performer. Please see the Admin.";
                    return RedirectToAction("Index");
                }
            }

            if (FullName != null && FullName.Count() > 0)         // search by FullName
            {
                var performers = _db.Performers.Where(p => p.FullName.Equals(FullName));
                int cnt = performers.Count();
                if (cnt == 1)
                {
                    Performer performer = performers.First();
                    return View(performer);
                }
                else
                    if (cnt == 0)     //no record by fullname then search by email
                    {
                        return LocateRecordByEmail(PerformerEmail);
                    }
                    else
                    {
                        TempData["OBJECT"] = performers;
                        TempData["STATUS"] = "More than 1 record for " + FullName + ". Please enter both Name and Email";
                        return RedirectToAction("Index");
                    }
            }
            else                                   // search by Email
            {
                if (PerformerEmail != null)
                    return LocateRecordByEmail(PerformerEmail);
                else
                {
                    //var performerError = new Performer
                    //{
                    //    FullName = "Please fill in Name/Email"
                    //};
                    TempData["STATUS"] = "Please fill in Name/Email";
                    return RedirectToAction("Index");
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }

        #region Helpers
        private ActionResult LocateRecordByEmail(string Email) //throw System.ArgumentNullException, throw System.InvalidOperationException
        {
            var performers = _db.Performers.Where(p => p.PerformerEmail.Equals(Email));
            int cnt = performers.Count();
            if (cnt == 1)
                return View(performers.First());
            else
            if (cnt == 0)
            {
                TempData["OBJECT"] = null;
                TempData["STATUS"] = "No Performer Found";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["OBJECT"] = performers.First();
                TempData["STATUS"] = "More than 1 record for " + Email + ". Please enter both Name and Email.";

                // redirect to the view that shows a list of Performers.
                return RedirectToAction("Index");
                //return View("ListOfPeformers", performers);
            }
        }
        #endregion
    }
}
