using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IPerformersEval.DAL;
using IPerformersEval.Models;
//using WebMatrix.WebData;

namespace IPerformersEval.Controllers
{
    public class VendorController : Controller
    {
        PerformersDB _db = new PerformersDB();

        //
        // GET: /Vendor/

        public ActionResult NA()
        {
            return RedirectToAction("Index", "Report");
        }

        public ActionResult Index()
        {
            return View(new Vendor());
        }

        //
        // GET: /Vendor/Create
        public ActionResult Create(string s, bool isChecked = true)    //s1: FEIN, s2: SSN
                    //Vendor model, bool dummy = true)    //string TIN = null, string Name = null)
        {
            Vendor vendor = new Vendor();
            //if (TIN != null)
            //if (model.Vendor_FederalTaxID != null)
            {
                vendor.Vendor_FederalTaxID = s; // model.Vendor_FederalTaxID;
            }
            //if (model.Vendor_FullName != null)   //Name != null)
            //{
            //    vendor.Vendor_FullName = model.Vendor_FullName;           
            //}
            vendor.Vendor_IsFederalTaxID = isChecked;   // true;        // default is the Federal Tax ID
            TempData["EDITMODE"] = "create";
            return View("VendorForm", vendor);
            //return View(vendor);
        }

        //
        // POST: /Vendor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Vendor vendor)
        {
            if (ModelState.IsValid)
            {
                var vendorInDataBase = _db.Vendors.Where(v => v.Vendor_FederalTaxID.Equals(vendor.Vendor_FederalTaxID));
                try
                {
                    Vendor v = vendorInDataBase.First();     // any record in the DB? throw exception if 

                    if (v.Vendor_FederalTaxID.Equals(vendor.Vendor_FederalTaxID)) // already there's a record with the same FederalTaxID
                    {
                        ModelState.AddModelError("Vendor_FederalTaxID", "The Federal Tax ID already exists in the database.");
                        return View(vendor);
                    }
                }
                catch (InvalidOperationException)   // it means no TaxID exists so we can modify
                {
                    _db.Vendors.Add(vendor);

                    // update the UserProfile's VendorCreate
                    //UserProfile profile = _db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == WebSecurity.CurrentUserName);
                    //if (profile != null)
                    //{
                    //    _db.Entry(profile).State = System.Data.EntityState.Modified;
                    //}
                    _db.SaveChanges();


                    // update table FormsStatus
                    UpdateFormStatus(vendor);

                    //TempData["OBJECT"] = vendor;
                    TempData["STATUS"] = "Success!";
                   // FormsStatus form = new FormsStatus();
                   // form.TIN = vendor.Vendor_FederalTaxID;
                    //return RedirectToAction("Index", "FormsStatus", form);
                    return RedirectToAction("Index", "FormsStatus");//, new { TIN = vendor.Vendor_FederalTaxID });
                }
            }

            return RedirectToAction("Index", "FormsStatus");//, new { TIN = vendor.Vendor_FederalTaxID });
        }

        public ActionResult Details(string s)// Vendor_FederalTaxID = "")
        {
            if (s.Length > 0)
            {
                var vendors = _db.Vendors.Where(p => (p.Vendor_FederalTaxID.Equals(s)));
                int cnt = vendors.Count();
                if (cnt == 1)
                {
                    Vendor v = vendors.First();
                    TempData["EDITMODE"] = "view";
                    return View("VendorForm", v);
                }
                else
                if (cnt == 0)
                {
                    TempData["STATUS"] = "No Vendor Found";
                    return RedirectToAction("Index", "FormsStatus");//, new { TIN = Vendor_FederalTaxID });
                }
                else
                {
                    TempData["STATUS"] = "Error: more than one vendor. Please see the Admin.";
                    return RedirectToAction("Index", "FormsStatus");//, new { TIN = Vendor_FederalTaxID });
                }
            }
            TempData["STATUS"] = "No Vendor Found";
            return RedirectToAction("Index", "FormsStatus");//, new { TIN = Vendor_FederalTaxID });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Vendor vendor)
        {
            _db.Entry(vendor).State = System.Data.EntityState.Modified;
            if (ModelState.IsValid)
            {
                var vendorsInDataBase = _db.Vendors.Where(p => p.Vendor_FederalTaxID.Equals(vendor.Vendor_FederalTaxID));
                try
                {
                    Vendor p = vendorsInDataBase.First();
                    if (!p.Vendor_FederalTaxID.Equals(vendor.Vendor_FederalTaxID))
                    {
                        //ModelState.AddModelError("PerformerEmail", "Modification is not saved: The Email already exists in the database.");
                        TempData["STATUS"] = "Modification is not saved: the TIN already exists in the database.";
                        return RedirectToAction("Index", "FormsStatus");//, new { TIN = vendor.Vendor_FederalTaxID });
                    }
                    _db.SaveChanges();

                    UpdateFormStatus(vendor);
                    
                    //TempData["OBJECT"] = vendor;
                    TempData["STATUS"] = "Modification is saved successfully.";
                    return RedirectToAction("Index", "FormsStatus");//, new { TIN = vendor.Vendor_FederalTaxID });
                }
                catch (InvalidOperationException e)   // it means modifying no record with the TIN (TIN of an existing record has been modified): we should let the user update to new email
                {
                    string err = e.ToString();

                    _db.SaveChanges();

                    //TempData["OBJECT"] = performer;
                    TempData["STATUS"] = "Modification is saved successfully.";
                    return RedirectToAction("Index", "FormsStatus");//, new { TIN = vendor.Vendor_FederalTaxID });
                }
            }
            else
            {
                return View(vendor);
            }
        }

        // this is get method
        public ActionResult Edit(string s)   //string Vendor_FederalTaxID)  //string Vendor_FederalTaxID = "AClibAdmin"
        {
            // Taking care of the Admin/Manager who has been given the backdoor
            if (s.Equals("AClibAdmin", StringComparison.OrdinalIgnoreCase))
            {
                return View("ListOfVendors", _db.Vendors.ToList());
            }

            if (s != null)         // search by Vendor_FederalTaxID
            {
                var vendors = _db.Vendors.Where(v => (v.Vendor_FederalTaxID.Equals(s)));
                int cnt = vendors.Count();
                if (cnt == 1)
                {
                    Vendor vendor = vendors.First();
                    TempData["EDITMODE"] = "modify";
                    return View("VendorForm", vendor);
                }
                else
                if (cnt == 0)
                {
                    //TempData["OBJECT"] = null;
                    TempData["STATUS"] = "No Vendor Found";
                    return RedirectToAction("Index", "FormsStatus");//, new { TIN = Vendor_FederalTaxID });
                }
                else
                {
                    //TempData["OBJECT"] = null;
                    TempData["STATUS"] = "Error: more than one Vendor. Please see the Admin.";
                    return RedirectToAction("Index", "FormsStatus");//, new { TIN = Vendor_FederalTaxID} );
                }
            }

            //TempData["OBJECT"] = null;
            TempData["STATUS"] = "No Vendor Found";
            return RedirectToAction("Index", "FormsStatus");//, new { TIN = Vendor_FederalTaxID } );
        }

        private void UpdateFormStatus(Vendor vendor)
        {
            //FormsStatus formStatus = new FormsStatus();
            //formStatus.TIN = vendor.Vendor_FederalTaxID;
            // //formStatus.SubW9_TIN = vendor.Vendor_FederalTaxID;
            //formStatus.PerformerName = vendor.Vendor_FullName;
            //formStatus.PerformerEmail = vendor.Contact_Email;
            //_db.AllFormsStatus.Add(formStatus);
            //_db.SaveChanges();

            var formsStatus = _db.AllFormsStatus.Where(v => v.TIN.Equals(vendor.Vendor_FederalTaxID));  // any record that had been create by Vendor (check by using TIN)
            try
            {
                FormsStatus f = formsStatus.First();     // any record in the DB? throw exception if 

                if (f.TIN.Equals(vendor.Vendor_FederalTaxID)) // already there's a record with the same TIN
                {
                    // FormsStatus formStatus = new FormsStatus();
                    f.TIN = vendor.Vendor_FederalTaxID;
                    f.PerformerName = vendor.Vendor_FullName;
                    f.PerformerEmail = vendor.Contact_Email;
                    _db.Entry(f).State = System.Data.EntityState.Modified;
                    _db.SaveChanges();
                }
            }
            catch (InvalidOperationException)   // it means no TIN exists so we can modify
            {
                FormsStatus f = new FormsStatus();
                f.TIN = vendor.Vendor_FederalTaxID;
                f.PerformerName = vendor.Vendor_FullName;
                f.PerformerEmail = vendor.Contact_Email;
                //f.CreatedByUser = WebSecurity.CurrentUserName; 

                _db.AllFormsStatus.Add(f);
                _db.SaveChanges();
            }
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}
