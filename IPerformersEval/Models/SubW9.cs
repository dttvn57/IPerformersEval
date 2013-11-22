using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace IPerformersEval.Models
{
    [Table("FIN_PE_SubW9")]
    public class SubW9
    {
        public int SubW9ID { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50)]
        public string Name { get; set; }

        [Display(Name = "All DBA(s) or Invoice Name(s)")]
        public string OtherNames { get; set; }

        [Display(Name = "Address for Correspondence")]
        public string Addr { get; set; }

        [StringLength(1)]
        [RegularExpression(@"[0-9]+", ErrorMessage = "Number only for SSN")]
        public string SSN1 { get; set; }

        [StringLength(1)]
        [RegularExpression(@"[0-9]+", ErrorMessage = "Number only for SSN")]
        public string SSN2 { get; set; }

        [StringLength(1)]
        [RegularExpression(@"[0-9]+", ErrorMessage = "Number only for SSN")]
        public string SSN3 { get; set; }

        [StringLength(1)]
        [RegularExpression(@"[0-9]+", ErrorMessage = "Number only for SSN")]
        public string SSN4 { get; set; }

        [StringLength(1)]
        [RegularExpression(@"[0-9]+", ErrorMessage = "Number only for SSN")]
        public string SSN5 { get; set; }

        [StringLength(1)]
        [RegularExpression(@"[0-9]+", ErrorMessage = "Number only for SSN")]
        public string SSN6 { get; set; }

        [StringLength(1)]
        [RegularExpression(@"[0-9]+", ErrorMessage = "Number only for SSN")]
        public string SSN7 { get; set; }

        [StringLength(1)]
        [RegularExpression(@"[0-9]+", ErrorMessage = "Number only for SSN")]
        public string SSN8 { get; set; }

        [StringLength(1)]
        [RegularExpression(@"[0-9]+", ErrorMessage = "Number only for SSN")]
        public string SSN9 { get; set; }

        [StringLength(1)]
        [RegularExpression(@"[0-9]+", ErrorMessage = "Number only for EIN")]
        public string EIN1 { get; set; }

        [StringLength(1)]
        [RegularExpression(@"[0-9]+", ErrorMessage = "Number only for EIN")]
        public string EIN2 { get; set; }

        [StringLength(1)]
        [RegularExpression(@"[0-9]+", ErrorMessage = "Number only for EIN")]
        public string EIN3 { get; set; }

        [StringLength(1)]
        [RegularExpression(@"[0-9]+", ErrorMessage = "Number only for EIN")]
        public string EIN4 { get; set; }

        [StringLength(1)]
        [RegularExpression(@"[0-9]+", ErrorMessage = "Number only for EIN")]
        public string EIN5 { get; set; }

        [StringLength(1)]
        [RegularExpression(@"[0-9]+", ErrorMessage = "Number only for EIN")]
        public string EIN6 { get; set; }

        [StringLength(1)]
        [RegularExpression(@"[0-9]+", ErrorMessage = "Number only for EIN")]
        public string EIN7 { get; set; }

        [StringLength(1)]
        [RegularExpression(@"[0-9]+", ErrorMessage = "Number only for EIN")]
        public string EIN8 { get; set; }

        [StringLength(1)]
        [RegularExpression(@"[0-9]+", ErrorMessage = "Number only for EIN")]
        public string EIN9 { get; set; }

        [StringLength(12)]
        public string TIN { get; set; }

        public bool Individual { get; set; }
        public bool SoleProprietor { get; set; }
        public bool Partnership { get; set; }
        public bool Corporation { get; set; }
        public bool TaxExempted { get; set; }
        public bool GovOrTrust { get; set; }

        public bool GoodsOnly { get; set; }
        public bool GoodsAndServices { get; set; }
        public bool RentsLeases { get; set; }
        public bool RentsLeasesAsAgent { get; set; }
        public bool MedicalService { get; set; }
        public bool LegalService { get; set; }
        public bool Settlement { get; set; }
        public bool OtherServices { get; set; }

        [StringLength(50)]
        public string OtherServices_Desc { get; set; }

        public bool ExemptFromBackupWithHolding { get; set; }

        public bool SigAppliesToCertification { get; set; }

        public bool SignedAck { get; set; }

        [Required]
        [Display(Name = "Signature")]
        public string SignedSignature { get; set; }

        [Required]
        [Display(Name = "Print Name")]
        public string SignedPrintName { get; set; }

        //[Required]
        [Display(Name = "Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? SignedDate { get; set; }

        public string SignedTitle { get; set; }

        [Display(Name = "Phone Number")]
        [StringLength(25)]
        public string SignedPhone { get; set; }

        [Display(Name = "Fax Number")]
        [StringLength(25)]
        public string SignedFAX { get; set; }

        [Display(Name = "e-mail address")]
        [StringLength(25)]
        public string SignedEmail { get; set; }
    }
}