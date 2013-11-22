using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using PerformersEval.DAL;
using PerformersEval.Models;
using System.IO;
using System.Net.Mail;
//using System.Net.Mail;
//using System.Web.Mail;
using Microsoft.Exchange.WebServices;
using Microsoft.Exchange.WebServices.Data;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
//using WebMatrix.WebData;
using System.Web.Security;
using System.Configuration;
using PerformersEval.Models.Grid;
using System.Threading;
using System.Data.Objects;
using PerformersEval.Models.Helpers;

namespace PerformersEval.Controllers
{
    public class ReportController : Controller
    {
        //private static Logger logger = LogManager.GetCurrentClassLogger();
        PerformersDB _db = new PerformersDB();

        /// <summary>
        /// The flag used to notify that the view was created from Index method.
        /// </summary>
        private static bool _fromIndex = false;

        //
        // GET: /Report/
        public ActionResult Index(string username)
        {
            bool isFUAdmin = IsFinAdmin(username);
            if (isFUAdmin)
            {
                _fromIndex = true;
                return View();  //_db.AllFormsStatus.ToList());
            }
            else
            {
                //var formsStatus = _db.AllFormsStatus.Where(p => (p.CreatedByUser.Equals(WebSecurity.CurrentUserName, StringComparison.OrdinalIgnoreCase)));
                //int cnt = formsStatus.Count();
                //return View(formsStatus.ToList());//_db.AllFormsStatus.ToList());
                TempData["STATUS"] = "You're not authorized";
                return RedirectToAction("Index", "Home");
            }
        }


        /// <summary>
        ///  Get Visitor log sorted by date.
        /// </summary>
        /// <returns>VisitorLog view</returns>
        //[Authorize]
        //public ActionResult Index()
        //{
        //    _fromIndex = true;
        //    //
        //    return View();
        //}

        /// <summary>
        /// Activate Delete view for the current item from the grid.
        /// </summary>
        /// <param name="id">The item ID.</param>
        /// <param name="gridSettings">The grid settings.</param>
        /// <returns>The action result.</returns>
        //[Authorize]
        //public ActionResult Delete(int id, string gridSettings)
        //{
        //    if (gridSettings != null)
        //        Session["GridSettings"] = gridSettings; // Cache the Grid Settings!
        //    //
        //    VisitorLog visitorLog = _db.VisitorLogs.Single(u => u.ID == id);
        //    //
        //    return View(visitorLog);
        //}

        /// <summary>
        /// Delete the current item.
        /// </summary>
        /// <param name="id">The item ID.</param>
        /// <returns>The action result.</returns>
        //[HttpPost, ActionName("Delete")]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    VisitorLog visitorLog = _db.VisitorLogs.Single(u => u.ID == id);
        //    _db.VisitorLogs.DeleteObject(visitorLog);
        //    _db.SaveChanges();
        //    //
        //    return RedirectToAction("Index");
        //}

        /// <summary>
        /// Search by using start date and from date from request.
        /// </summary>
        /// <returns></returns>
        //[Authorize]
        public PartialViewResult Search()
        {
            string startDate = Request["from"];
            string endDate = Request["to"];
            //
            // Cache the start and end dates into the session to be used by later one in the view.
            //
            Session["StartDate"] = (startDate.Length < 1 ? null : (object)DateTime.Parse(startDate, Thread.CurrentThread.CurrentCulture));
            Session["EndDate"] = (endDate.Length < 1 ? null : (object)DateTime.Parse(endDate, Thread.CurrentThread.CurrentCulture));
            //
            return PartialView("_ReportGrid");
        }

        /// <summary>
        /// Load the data that will be used by the jqGrid by using the given grid settings.
        /// </summary>
        /// <param name="grid">The grid sesstings.</param>
        /// <returns>The data in JSON format.</returns>
        public JsonResult GetData(GridSettings grid)
        {
            if (_fromIndex && Session["GridSettings"] != null)
            {
                //
                // Get grid settings from cache!
                //
                grid = new GridSettings((string)Session["GridSettings"]);
            }
            //
            _fromIndex = false; // Must be set on false here!
            //
            // Load the data from the database for the current grid settings.
            //
            DateTime searchStartDate = (Session["StartDate"] == null ? DateTime.MinValue : (DateTime)Session["StartDate"]);
            DateTime searchEndDate = (Session["EndDate"] == null ? DateTime.MinValue : (DateTime)Session["EndDate"]);
            int count;
            var query = grid.LoadGridData<FormsStatus>(SearchByDates(_db, searchStartDate, searchEndDate), out count);
            var data = query.ToArray();
            //
            // Convert the results in grid format.
            //
            string gridSettingsString = grid.ToString(); // Used to preserve grid settings!
            Session["GridSettings"] = gridSettingsString;
            gridSettingsString = null;
            var result = new
            {
                total = (int)Math.Ceiling((double)count / grid.PageSize),
                page = grid.PageIndex,
                records = count,
                rows = (from formStatus in data
                        select new
                        {
                            ID = formStatus.FormsStatusID,
                            Name = formStatus.PerformerName,
                            SubmitDate = formStatus.FormsSent.ToString(),
                            Vendor = formStatus.TIN != null ? true : false,
                            Action = string.Format("{0}",
                                                    RenderHelpers.ImageButton(
                                                            this,
                                                            "delete tip here",
                                                            "~/Images/Delete.gif",
                                                            "Delete",
                                                            new { id = formStatus.FormsStatusID },
                                                            new { style = "border:0px;" }))
                        }).ToArray()
            };
            //
            // Convert to JSON before to return.
            //
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Clear all order's history grid data.
        /// </summary>
        /// <returns>Returns the result view.</returns>
        [Authorize]
        public ActionResult ClearGridData()
        {
            DateTime searchStartDate = (Session["StartDate"] == null ? DateTime.MinValue : (DateTime)Session["StartDate"]);
            DateTime searchEndDate = (Session["EndDate"] == null ? DateTime.MinValue : (DateTime)Session["EndDate"]);
            //DeleteLogByDates(_db, searchStartDate, searchEndDate);
            //
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Quick search visitor logs .
        /// </summary>
        /// <param name="dataContext">The data context.</param>
        /// <param name="searchStartDate">The search start date.</param>
        /// <param name="searchEndDate">The search end date.</param>
        /// <returns></returns>
        public static IQueryable<FormsStatus> SearchByDates(PerformersDB dataContext, DateTime searchStartDate, DateTime searchEndDate)
        {
            if (searchStartDate.Date == DateTime.MinValue && searchEndDate.Date == DateTime.MinValue)
            {
                return dataContext.AllFormsStatus.AsQueryable();
            }
            else if (searchStartDate.Date == DateTime.MinValue)
            {
                var searchResults = dataContext.AllFormsStatus.Where(c =>
                    EntityFunctions.TruncateTime(c.FormsSent) <= EntityFunctions.TruncateTime(searchEndDate));
                //
                return searchResults.AsQueryable();
            }
            else if (searchEndDate.Date == DateTime.MinValue)
            {
                var searchResults = dataContext.AllFormsStatus.Where(c =>
                    EntityFunctions.TruncateTime(c.FormsSent) >= EntityFunctions.TruncateTime(searchStartDate));
                //
                return searchResults.AsQueryable();
            }
            else
            {
                var searchResults = dataContext.AllFormsStatus.Where(c =>
                    EntityFunctions.TruncateTime(c.FormsSent) >= EntityFunctions.TruncateTime(searchStartDate) &&
                    EntityFunctions.TruncateTime(c.FormsSent) <= EntityFunctions.TruncateTime(searchEndDate));
                //
                return searchResults.AsQueryable();
            }
        }

        //public static void DeleteLogByDates(PerformersDB dataContext, DateTime searchStartDate, DateTime searchEndDate)
        //{
        //    if (searchStartDate.Date == DateTime.MinValue && searchEndDate.Date == DateTime.MinValue)
        //    {
        //        foreach (var formsStatus  in dataContext.AllFormsStatus)
        //        {
        //            dataContext.DeleteObject(visitorLog);
        //        }
        //    }
        //    else if (searchStartDate.Date == DateTime.MinValue)
        //    {
        //        var searchResults = dataContext.AllFormsStatus.Where(c =>
        //            EntityFunctions.TruncateTime(c.StartDate) <= EntityFunctions.TruncateTime(searchEndDate));
        //        //
        //        DeleteSelectedLogs(dataContext, searchResults);

        //    }
        //    else if (searchEndDate.Date == DateTime.MinValue)
        //    {
        //        var searchResults = dataContext.AllFormsStatus.Where(c =>
        //            EntityFunctions.TruncateTime(c.FormsSent) >= EntityFunctions.TruncateTime(searchStartDate));
        //        //
        //        DeleteSelectedLogs(dataContext, searchResults);
        //    }
        //    else
        //    {
        //        var searchResults = dataContext.AllFormsStatus.Where(c =>
        //            EntityFunctions.TruncateTime(c.FormsSent) >= EntityFunctions.TruncateTime(searchStartDate) &&
        //            EntityFunctions.TruncateTime(c.FormsSent) <= EntityFunctions.TruncateTime(searchEndDate));
        //        //
        //        DeleteSelectedLogs(dataContext, searchResults);
        //    }
        //    //
        //    dataContext.SaveChanges();
        //}

        private bool IsFinAdmin(string username)
        {
            string[] names = ConfigurationManager.AppSettings.AllKeys
                              .Where(key => key.StartsWith("fu_admin"))
                              .Select(key => ConfigurationManager.AppSettings[key])
                              .ToArray();
            foreach (string name in names)
            {
                if (username.Equals(name, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        private bool IsBranchAdmin(string username)
        {
            string[] names = ConfigurationManager.AppSettings.AllKeys
                              .Where(key => key.StartsWith("br_admin"))
                              .Select(key => ConfigurationManager.AppSettings[key])
                              .ToArray();
            foreach (string name in names)
            {
                if (username.Equals(name, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        //private void SendEmail()
        //{
        //    ExchangeService service = CreateConnection("https://autodiscover.test.com/EWS/Exchange.asmx");

        //    EmailMessage msg = new EmailMessage(service);
        //    msg.ToRecipients.Add(new EmailAddress("tdang@aclibrary.org"));
        //    msg.Subject = "Test email";
        //    msg.Body = new MessageBody(BodyType.HTML, "<p>Hello Email!</p>");
        //    try
        //    {
        //        msg.Send();
        //    }
        //    catch (Exception e)
        //    {

        //        TempData["OBJECT"] = null;
        //        TempData["STATUS"] = e.Message;
        //        //return RedirectToAction("Index");
        //        //return PartialView("_FormsStatusView", formsStatus);
        //        //return RedirectToAction("Index", "FormsStatus", new { TIN = f.TIN });
        //    }
        //}

        //public static ExchangeService CreateConnection(String url)
        //{
        //    // Hook up the cert callback to prevent error if Microsoft.NET doesn't trust the server
        //    ServicePointManager.ServerCertificateValidationCallback =
        //        delegate(
        //            Object obj,
        //            X509Certificate certificate,
        //            X509Chain chain,
        //            SslPolicyErrors errors)
        //        {
        //            return true;
        //        };

        //    ExchangeService service = new ExchangeService();
        //    service.Url = new Uri(url);

        //    service.Credentials = new NetworkCredential("username", "password", "domain");

        //    return service;
        //}

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}
