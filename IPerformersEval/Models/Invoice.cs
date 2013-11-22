using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace IPerformersEval.Models
{
    [Table("FIN_PE_Invoice")]
    public class Invoice
    {
        public int InvoiceID { get; set; }

        [Display(Name = "Name")]
        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Address")]
         [StringLength(50)]
        public string Addr1 { get; set; }

        //[Required]
        [StringLength(50)]
        public string City { get; set; }

        //[Required]
        [StringLength(2)]
        public string State { get; set; }

        //[Required]
        [StringLength(10)]
        public string Zip { get; set; }

        [Display(Name = "TIN (SSN or EIN)")]
        [Required]
        [StringLength(12)]
        public string SSN { get; set; }

        [Range(0.0, 1000000.0, ErrorMessage = "Payment is from 0 to 1,000,000")]
        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$", ErrorMessage = "Payment is invalid")]
        public double Payment { get; set; }

        [StringLength(1024)]
        public string PaymentFor1 { get; set; }

        [StringLength(1024)]
        public string PaymentFor2 { get; set; }

        public bool SignedAck { get; set; }

        //[Required]
        [Display(Name = "Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? SignedDate { get; set; }

        [StringLength(50)]
        public string PayFromAcct { get; set; }

        [StringLength(50)]
        public string Manager { get; set; }

        [Display(Name = "Manager Signed Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? ManagerSignedDate { get; set; }

        [StringLength(50)]
        public string Librarian { get; set; }

        [Display(Name = "Librarian Signed Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? LibrarianSignedDate { get; set; }
    }
}