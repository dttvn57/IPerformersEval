using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IPerformersEval.DAL;
using IPerformersEval.Models;
using Rotativa;

namespace IPerformersEval.Controllers
{
    public class W9Controller : Controller
    {
        PerformersDB _db = new PerformersDB();

        //
        // GET: /W9/

        public ActionResult Index()
        {
            return View(new SubW9());
        }

        //
        // GET: /Sub W9/Create
        public ActionResult Create(string s = null, string Name = null)
        {
            SubW9 w9 = new SubW9();

            if (s != null)
            {
                // is this a FEIN or SSN? Look into Vendor's Vendor_IsFederalTaxID
                bool isFederalTaxID = IsFederalTaxID(s);

                if (isFederalTaxID)  // SSN
                {
                    w9.EIN1 = s.ElementAt(0).ToString();
                    w9.EIN2 = s.ElementAt(1).ToString();
                    w9.EIN3 = s.ElementAt(2).ToString();
                    w9.EIN4 = s.ElementAt(3).ToString();
                    w9.EIN5 = s.ElementAt(4).ToString();
                    w9.EIN6 = s.ElementAt(5).ToString();
                    w9.EIN7 = s.ElementAt(6).ToString();
                    w9.EIN8 = s.ElementAt(7).ToString();
                    w9.EIN9 = s.ElementAt(8).ToString();
                }
                else
                {
                    w9.SSN1 = s.ElementAt(0).ToString();
                    w9.SSN2 = s.ElementAt(1).ToString();
                    w9.SSN3 = s.ElementAt(2).ToString();
                    w9.SSN4 = s.ElementAt(3).ToString();
                    w9.SSN5 = s.ElementAt(4).ToString();
                    w9.SSN6 = s.ElementAt(5).ToString();
                    w9.SSN7 = s.ElementAt(6).ToString();
                    w9.SSN8 = s.ElementAt(7).ToString();
                    w9.SSN9 = s.ElementAt(8).ToString();
               };

            };

            if (Name != null)
            {
                w9.Name = Name;           
            }
            if (s != null)
                w9.TIN = s;

            return View(w9);
        }

        [HttpPost]
        public ActionResult Create(SubW9 w9)
        {
            if (ModelState.IsValid)
            {
                if (w9.SSN1 != null && w9.SSN1.Count() == 1 &&
                    w9.SSN2 != null && w9.SSN2.Count() == 1 &&
                    w9.SSN3 != null && w9.SSN3.Count() == 1 &&
                    w9.SSN4 != null && w9.SSN4.Count() == 1 &&
                    w9.SSN5 != null && w9.SSN5.Count() == 1 &&
                    w9.SSN6 != null && w9.SSN6.Count() == 1 &&
                    w9.SSN7 != null && w9.SSN7.Count() == 1 &&
                    w9.SSN8 != null && w9.SSN8.Count() == 1 &&
                    w9.SSN9 != null && w9.SSN9.Count() == 1)
                {
                    w9.TIN = w9.SSN1 + w9.SSN2 + w9.SSN3 + w9.SSN4 + w9.SSN5 + w9.SSN6 + w9.SSN7 + w9.SSN8 + w9.SSN9;
                }
                else
                if (w9.EIN1 != null && w9.EIN1.Count() == 1 &&
                    w9.EIN2 != null && w9.EIN2.Count() == 1 &&
                    w9.EIN3 != null && w9.EIN3.Count() == 1 &&
                    w9.EIN4 != null && w9.EIN4.Count() == 1 &&
                    w9.EIN5 != null && w9.EIN5.Count() == 1 &&
                    w9.EIN6 != null && w9.EIN6.Count() == 1 &&
                    w9.EIN7 != null && w9.EIN7.Count() == 1 &&
                    w9.EIN8 != null && w9.EIN8.Count() == 1 &&
                    w9.EIN9 != null && w9.EIN9.Count() == 1)
                {
                    w9.TIN = w9.EIN1 + w9.EIN2 + w9.EIN3 + w9.EIN4 + w9.EIN5 + w9.EIN6 + w9.EIN7 + w9.EIN8 + w9.EIN9;
                }
                var w9InDataBase = _db.SubW9s.Where(w => w.TIN.Equals(w9.TIN));
                try
                {
                    SubW9 w = w9InDataBase.First();     // any record in the DB? throw exception if 

                    if (w.TIN.Equals(w9.TIN)) // already there's a record with the same TIN
                    {
                        ModelState.AddModelError("TIN", "The TIN already exists in the database.");
                        return View(w9);
                    }
                }
                catch (InvalidOperationException)   // it means no TIN exists so we can modify
                {
                    _db.SubW9s.Add(w9);
                    _db.SaveChanges();

                    // update table FormsStatus
                    UpdateFormStatus(w9);

                    //TempData["OBJECT"] = w9;
                    TempData["STATUS"] = "Success!";
                    return RedirectToAction("Index", "FormsStatus");//, new { TIN = w9.TIN });
                }
            }

            return RedirectToAction("Index", "FormsStatus");//, new { TIN = w9.TIN });
        }

        public ActionResult Edit(string s = "")  //string TIN = "AClibAdmin"
        {
            // Taking care of the Admin/Manager who has been given the backdoor
            if (s.Equals("AClibAdmin", StringComparison.OrdinalIgnoreCase))
            {
                return View("ListOfSubW9s", _db.SubW9s.ToList());
            }

            if (s.Length > 0)         // search by TIN
            {
                bool isFederalTaxID = IsFederalTaxID(s);

                var subW9s = _db.SubW9s.Where(w => (w.TIN.Equals(s)));
                int cnt = subW9s.Count();
                if (cnt == 1)
                {
                    SubW9 w = subW9s.First();
                    if (isFederalTaxID)  // EIN
                    {
                        w.EIN1 = s.ElementAt(0).ToString();
                        w.EIN2 = s.ElementAt(1).ToString();
                        w.EIN3 = s.ElementAt(2).ToString();
                        w.EIN4 = s.ElementAt(3).ToString();
                        w.EIN5 = s.ElementAt(4).ToString();
                        w.EIN6 = s.ElementAt(5).ToString();
                        w.EIN7 = s.ElementAt(6).ToString();
                        w.EIN8 = s.ElementAt(7).ToString();
                        w.EIN9 = s.ElementAt(8).ToString();
                    }
                    else
                    {
                        w.SSN1 = s.ElementAt(0).ToString();
                        w.SSN2 = s.ElementAt(1).ToString();
                        w.SSN3 = s.ElementAt(2).ToString();
                        w.SSN4 = s.ElementAt(3).ToString();
                        w.SSN5 = s.ElementAt(4).ToString();
                        w.SSN6 = s.ElementAt(5).ToString();
                        w.SSN7 = s.ElementAt(6).ToString();
                        w.SSN8 = s.ElementAt(7).ToString();
                        w.SSN9 = s.ElementAt(8).ToString();
                    };

                    w.TIN = s;
                    return View(w);
                }
                else
                if (cnt == 0)
                {
                    //TempData["OBJECT"] = null;
                    TempData["STATUS"] = "No Sub W-9 Found";
                    return RedirectToAction("Index", "FormsStatus");//, new { TIN = s });
                }
                else
                {
                    //TempData["OBJECT"] = null;
                    TempData["STATUS"] = "Error: more than one Sub W-9. Please see the Admin.";
                    return RedirectToAction("Index", "FormsStatus");//, new { TIN = TIN });
                }
            }

            //TempData["OBJECT"] = null;
            TempData["STATUS"] = "No Sub W-9 Found";
            return RedirectToAction("Index", "FormsStatus");//, new { TIN = TIN });
        }

        [HttpPost]
        public ActionResult Edit(SubW9 w9)
        {
            _db.Entry(w9).State = System.Data.EntityState.Modified;
            if (ModelState.IsValid)
            {
                var w9sInDataBase = _db.SubW9s.Where(p => p.TIN.Equals(w9.TIN));
                try
                {
                    SubW9 p = w9sInDataBase.First();

                    //--- Still have to check for the case where user wants to modify the email which is a duplicate
                    if (!p.TIN.Equals(w9.TIN))
                    {
                        //ModelState.AddModelError("PerformerEmail", "Modification is not saved: The Email already exists in the database.");
                        TempData["STATUS"] = "Modification is not saved: the TIN already exists in the database.";
                        return RedirectToAction("Index", "FormsStatus");//, new { TIN = w9.TIN });
                    }
                    _db.SaveChanges();

                    UpdateFormStatus(w9);

                    //TempData["OBJECT"] = w9;
                    TempData["STATUS"] = "Modification is saved successfully.";
                    return RedirectToAction("Index", "FormsStatus");//, new { TIN = w9.TIN });
                }
                catch (InvalidOperationException e)   // it means modifying no record with the TIN (TIN of an existing record has been modified): we should let the user update to new email
                {
                    string err = e.ToString();

                    _db.SaveChanges();

                    //TempData["OBJECT"] = performer;
                    TempData["STATUS"] = "Modification is saved successfully.";
                    return RedirectToAction("Index", "FormsStatus");//, new { TIN = w9.TIN });
                }
            }
            else
            {
                return View(w9);
            }
        }

        public ActionResult Details(string s = "")
        {
            if (s.Length > 0)
            {
                bool isFederalTaxID = IsFederalTaxID(s);

                var subW9s = _db.SubW9s.Where(p => (p.TIN.Equals(s)));
                int cnt = subW9s.Count();
                if (cnt == 1)
                {
                    SubW9 w = subW9s.First();
                    if (isFederalTaxID)  // EIN
                    {
                        w.EIN1 = s.ElementAt(0).ToString();
                        w.EIN2 = s.ElementAt(1).ToString();
                        w.EIN3 = s.ElementAt(2).ToString();
                        w.EIN4 = s.ElementAt(3).ToString();
                        w.EIN5 = s.ElementAt(4).ToString();
                        w.EIN6 = s.ElementAt(5).ToString();
                        w.EIN7 = s.ElementAt(6).ToString();
                        w.EIN8 = s.ElementAt(7).ToString();
                        w.EIN9 = s.ElementAt(8).ToString();
                    }
                    else
                    {
                        w.SSN1 = s.ElementAt(0).ToString();
                        w.SSN2 = s.ElementAt(1).ToString();
                        w.SSN3 = s.ElementAt(2).ToString();
                        w.SSN4 = s.ElementAt(3).ToString();
                        w.SSN5 = s.ElementAt(4).ToString();
                        w.SSN6 = s.ElementAt(5).ToString();
                        w.SSN7 = s.ElementAt(6).ToString();
                        w.SSN8 = s.ElementAt(7).ToString();
                        w.SSN9 = s.ElementAt(8).ToString();
                    };

                    w.TIN = s;
                    return View("Details", w);
                }
                else
                    if (cnt == 0)
                    {
                        TempData["STATUS"] = "No Sub W-9 Found";
                        return RedirectToAction("Index", "FormsStatus");
                    }
                    else
                    {
                        TempData["STATUS"] = "Error: more than one Performer. Please see the Admin.";
                        return RedirectToAction("Index", "FormsStatus");
                    }
            }
            TempData["STATUS"] = "No Sub W-9 Found";
            return RedirectToAction("Index", "FormsStatus");
        }

        private bool IsFederalTaxID(string TIN)
        {
            bool isFederalTaxID = true;
            var vendors = _db.Vendors.Where(v => (v.Vendor_FederalTaxID.Equals(TIN)));
            int cnt = vendors.Count();
            if (cnt == 1)
            {
                Vendor v = vendors.First();
                isFederalTaxID = v.Vendor_IsFederalTaxID;
            }
            return isFederalTaxID;
        }

        private void UpdateFormStatus(SubW9 w9)
        {
            var formsStatus = _db.AllFormsStatus.Where(w => w.TIN.Equals(w9.TIN));  // any record that had been create by Vendor (check by using TIN)
            try
            {
                FormsStatus f = formsStatus.First();     // any record in the DB? throw exception if 

                if (f.TIN.Equals(w9.TIN)) // already there's a record with the same TIN
                {
                   // FormsStatus formStatus = new FormsStatus();
                    f.SubW9_TIN = w9.TIN;
                    _db.Entry(f).State = System.Data.EntityState.Modified;
                    _db.SaveChanges();
                }
            }
            catch (InvalidOperationException)   // it means no TIN exists so we can modify
            {
                FormsStatus formStatus = new FormsStatus();
                formStatus.TIN = w9.TIN;
                _db.AllFormsStatus.Add(formStatus);
                _db.SaveChanges();
            }
        }

        public ActionResult Print(int id = 0)
        {
            SubW9 w9 = _db.SubW9s.Find(id);
            if (w9 == null)
            {
                TempData["STATUS"] = "Sub W-9 not Found.";
                return RedirectToAction("Index", "FormsStatus");//, new { TIN = 0 });
            }
            return View("Print", w9);
        }

        public ActionResult PrintW9(int spec = 0)
        {
            return new ActionAsPdf(
                           "Print",
                           new { id = spec }) { FileName = spec.ToString() + "_SubW9.pdf" };

        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}
