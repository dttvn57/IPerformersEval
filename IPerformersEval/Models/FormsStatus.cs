using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace IPerformersEval.Models
{
    [Table("FIN_PE_FormStatus")]
    public class FormsStatus
    {
        public int FormsStatusID { get; set; }

       // [Display(Name = "Performer Name")]
        [StringLength(50)]
        //[Required]
        public string PerformerName { get; set; }

       // [Remote("IsUserNameAvailable", "Performer")]//, AdditionalFields = "PerformerID")]
        //[Display(Name = "Performer Email")]
        //[RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Email is invalid")]
        public string PerformerEmail { get; set; }

        // this is the Vendor's Vendor_FederalTaxID
        [StringLength(12)]
        public string TIN { get; set; }

        [StringLength(12)]
        public string SubW9_TIN { get; set; }

        [StringLength(12)]
        public string Performer_TIN { get; set; }

        public string AutoInsuranceFileName { get; set; }

        [StringLength(12)]
        public string Invoice_TIN { get; set; }

        public DateTime? FormsSent { get; set; }

        public string CreatedByUser { get; set; }
    }
}