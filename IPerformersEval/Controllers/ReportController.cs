using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
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
using System.Threading;
using System.Data.Objects;
using IPerformersEval.Models;
using IPerformersEval.DAL;

namespace IPerformersEval.Controllers
{
    public class ReportController : Controller
    {
        //private static Logger logger = LogManager.GetCurrentClassLogger();
        PerformersDB _db = new PerformersDB();

        //
        // GET: /Report/
        public ActionResult Index(string username)
        {
            string user = username;
            if (user == null)
                user = (string)Session["username"];
            else
            {
                Session["username"] = username;
            }
            if (user == null)
                user = "";

            bool isFUAdmin = IsFinAdmin(user);
            if (isFUAdmin)
            {
                return View(_db.AllFormsStatus.ToList());
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
