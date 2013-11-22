using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace IPerformersEval.Models
{
    [Table("FIN_PE_Performer")]
    public class Performer
    {
        //[Key]
        //[DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int PerformerID { get; set; }

        [Display(Name="TIN")]
        [StringLength(12)]
        public string Performer_TIN { get; set; }      // internal use

        [Display(Name="Performer Name")]
        [Required(ErrorMessage = "Performer Name is required")]
        [StringLength(50)]
        public string FullName { get; set; }

        [Display(Name="Name")]
        [StringLength(50)]
        public string FullName2 { get; set; }

        [Display(Name = "Address")]
        [Required]
        [StringLength(50)]
        public string Addr1 { get; set; }

        [StringLength(50)]
        public string Addr2 { get; set; }

        [Required]
        [StringLength(50)]
        public string City { get; set; }

        [Required]
        [StringLength(2)]
        public string State { get; set; }

        [Required]
        [StringLength(10)]
        public string Zip { get; set; }

        [Required]
        [Remote("IsUserNameAvailable", "Performer")]//, AdditionalFields = "PerformerID")]
        [Display(Name = "Email")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Email is invalid")]
        public string PerformerEmail { get; set; }

        [Display(Name = "Program Name")]
        [StringLength(1024)]
        public string ProgramName { get; set; }

        [Display(Name = "Contract Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? ContractDate { get; set; }

        [Range(0.0, 1000000.0, ErrorMessage = "Payment is from 0 to 1,000,000")]
        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$", ErrorMessage = "Payment is invalid")]
        public double Payment { get; set; }

        //[Required]
        public bool PersonalInfoAckSig { get; set; }

        //[Required]
        public bool OfficialAckSig { get; set; }

        //[Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? SignedDate { get; set; }

        [Required]
        [Display(Name = "Owner, Officer, Director, Partnership or other Principal Name")]
        public string PrintName { get; set; }

        public string Title { get; set; }
    }
}