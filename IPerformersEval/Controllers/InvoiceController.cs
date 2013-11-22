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
    public class InvoiceController : Controller
    {
        PerformersDB _db = new PerformersDB();

        //
        // GET: /Invoice/

        public ActionResult Index(string spec = "", string Name = "")
        {
            IQueryable<Invoice> invoices = _db.Invoices.Where(i => i.SSN == spec);
            List<Invoice> invList = invoices.ToList();
            if (invoices.Count() == 0)
            {
                Invoice inv = new Invoice();
                inv.SSN = spec;
                invList.Add(inv);
            }
            return View(invList);
        }

        public ActionResult Print(int id = 0)
        {
            Invoice inv = _db.Invoices.Find(id);
            if (inv == null)
            {
                TempData["STATUS"] = "Invoice not Found.";
                return RedirectToAction("Index", "FormsStatus", new { TIN = 0 });
            }
            TempData["PRINTSTATUS"] = "The invoice has been saved to " + ". Please print and get the manager's approval";
            return View("Print", inv);
        }

        public ActionResult PrintInvoice(int invoiceId, bool ForPrinting = false)
        {
            return new ActionAsPdf(
                           "Print",
                           new { id = invoiceId }) { FileName = invoiceId.ToString() + "_Invoice.pdf" };

        }

        //
        // GET: /Invoice/Create
        public ActionResult Create(string spec = null, string Name = null)
        {
            Invoice inv = new Invoice();
            if (spec != null)
            {
                var vendorsInDataBase = _db.Vendors.Where(v => v.Vendor_FederalTaxID.Equals(spec, StringComparison.OrdinalIgnoreCase));
                try
                {
                    Vendor v = vendorsInDataBase.First();     // any record in the DB? throw exception if 

                    if (v.Vendor_FederalTaxID.Equals(spec)) // already there's a record with the same TIN
                    {
                        inv.SSN = spec;
                    }
                }
                catch (InvalidOperationException)   // it means no TIN exists so we can modify
                {
                    TempData["OBJECT"] = inv;
                    TempData["STATUS"] = "Success!";
                    return RedirectToAction("Index", "FormsStatus");
                }
            }
            if (Name != null)
            {
                inv.Name = Name;
            }

            return View(inv);
        }

        [HttpPost]
        public ActionResult Create(Invoice inv)
        {
            if (ModelState.IsValid)
            {
                //var invInDataBase = _db.Invoices.Where(i => i.SSN.Equals(inv.SSN));
                //try
                //{
                //    Invoice i = invInDataBase.First();     // any record in the DB? throw exception if 

                //    if (i.SSN.Equals(inv.SSN)) // already there's a record with the same TIN
                //    {
                //        ModelState.AddModelError("Invoice", "The Invoice already exists in the database.");
                //        return View(inv);
                //    }
                //}
                //catch (InvalidOperationException)   // it means no TIN exists so we can modify
                {
                    _db.Invoices.Add(inv);
                    _db.SaveChanges();

                    // update table FormsStatus
                    UpdateFormStatus(inv);

                    //TempData["OBJECT"] = inv;
                    //TempData["STATUS"] = "Success!";

                    //return RedirectToAction("Details", new { id = inv.InvoiceID, ForPrinting = false });
                    //-- this is for testing -- return RedirectToAction("Print", new { id = inv.InvoiceID });
                }
            }

            return View(inv);
        }

        //public ActionResult Edit(string SSN)  //string SSN = "AClibAdmin"
        //{
        //    // Taking care of the Admin/Manager who has been given the backdoor
        //    if (SSN.Equals("AClibAdmin", StringComparison.OrdinalIgnoreCase))
        //    {
        //        return View("ListOfInvoices", _db.Invoices.ToList());
        //    }

        //    if (SSN != null)         // search by SSN
        //    {
        //        var invoices = _db.Invoices.Where(w => (w.SSN.Equals(SSN)));
        //        int cnt = invoices.Count();
        //        if (cnt == 1)
        //        {
        //            Invoice w = invoices.First();
        //            return View(w);
        //        }
        //        else
        //            if (cnt == 0)
        //            {
        //                TempData["OBJECT"] = null;
        //                TempData["STATUS"] = "No Invoice Found";
        //                return RedirectToAction("Index");
        //            }
        //            else
        //            {
        //                TempData["OBJECT"] = null;
        //                TempData["STATUS"] = "Error: more than one Invoice. Please see the Admin.";
        //                return RedirectToAction("Index");
        //            }
        //    }

        //    TempData["OBJECT"] = null;
        //    TempData["STATUS"] = "No Invoice Found";
        //    return RedirectToAction("Index");
        //}

        public ActionResult Edit(int id)
        {
            Invoice inv = _db.Invoices.Find(id);
            if (inv == null)
            {
                //TempData["STATUS"] = "Invoice not Found.";
                return RedirectToAction("ListOfInvoices");
            }
            return View("Create", inv);
        }

        [HttpPost]
        public ActionResult Edit(Invoice inv)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(inv).State = System.Data.EntityState.Modified;
                _db.SaveChanges();

                //TempData["OBJECT"] = inv;
                //TempData["STATUS"] = "Modification is saved successfully.";
                //return RedirectToAction("Index");
                return View("Create", inv);
            }
            else
            {
                return View("Create", inv);
            }
        }

        public ActionResult Details(int id = 0, bool ForPrinting = false)
        {
            Invoice inv = _db.Invoices.Find(id);
            if (inv == null)
            {
                TempData["STATUS"] = "Invoice not Found.";
                return RedirectToAction("Index", "FormsStatus", new { TIN = 0 });
            }

            //if (ForPrinting)
            //    return RedirectToAction("Index", "FormsStatus", new { TIN = inv.SSN });
            //else
                return View("Details", inv);
        }

        private void UpdateFormStatus(Invoice inv)
        {
            var formsStatus = _db.AllFormsStatus.Where(w => w.TIN.Equals(inv.SSN));  // any record that had been create by Vendor (check by using TIN)
            try
            {
                FormsStatus f = formsStatus.First();     // any record in the DB? throw exception if 

                if (f.TIN.Equals(inv.SSN)) // already there's a record with the same TIN
                {
                    // FormsStatus formStatus = new FormsStatus();
                    f.Invoice_TIN = inv.SSN;
                    _db.Entry(f).State = System.Data.EntityState.Modified;
                    _db.SaveChanges();
                }
            }
            catch (InvalidOperationException)   // it means no TIN exists so we can modify
            {
                FormsStatus f = new FormsStatus();
                f.Invoice_TIN = inv.SSN;
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
