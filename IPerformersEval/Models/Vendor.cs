using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IPerformersEval.Models
{
    [Table("FIN_PE_Vendor")]
    public class Vendor
    {
        //[Key]
        //[DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int VendorID { get; set; }

        [Display(Name="Name")]
        [StringLength(50)]
        public string SentBy_Name { get; set; }

        [Display(Name="Department")]
        [StringLength(50)]
        public string SentBy_Department { get; set; }

        [Display(Name="QIC")]
        [StringLength(10)]
        public string SentBy_QIC { get; set; }

        [Display(Name = "Telephone")]
        [StringLength(25)]
        //[RegularExpression(@"^([0-9]( |-)?)?(\(?[0-9]{3}\)?|[0-9]{3})( |-)?([0-9]{3}( |-)?[0-9]{4}|[a-zA-Z0-9]{7})$", ErrorMessage = "Phone is invalid")]
        public string SentBy_Phone { get; set; }

        [Display(Name = "FAX")]
        [StringLength(25)]
        //[RegularExpression(@"^([0-9]( |-)?)?(\(?[0-9]{3}\)?|[0-9]{3})( |-)?([0-9]{3}( |-)?[0-9]{4}|[a-zA-Z0-9]{7})$", ErrorMessage = "Phone is invalid")]
        public string SentBy_FAX { get; set; }

        public bool Request_AddNewVendor { get; set; }
        public bool Request_AddNewDBAForExistingVendor { get; set; }
        public bool Request_AddNewAddressForExistingVendor { get; set; }
        public bool Request_ChangeNameForExistingVendor { get; set; }
        public bool Request_ChangeDBAForExistingVendor { get; set; }
        public bool Request_ChangeAddressForExistingVendor { get; set; }

        public bool Affiliate_Yes { get; set; }
        public bool Affiliate_No { get; set; }

        [Display(Name = "ALCOLINK Vendor Number")]
        public string Vendor_ALCOLINKNumber { get; set; }

        [Display(Name = "Full Legal Name")]
        [StringLength(50)]
        [Required]
        public string Vendor_FullName { get; set; }

        [Display(Name = "DBA Name")]
        [StringLength(50)]
        public string Vendor_DBA { get; set; }

        public bool Vendor_Entity_Individual { get; set; }
        public bool Vendor_Entity_SoleProprietor { get; set; }
        public bool Vendor_Entity_Partnership { get; set; }
        public bool Vendor_Entity_Corporation { get; set; }
        public bool Vendor_Entity_TaxExempted { get; set; }
        public bool Vendor_Entity_GovOrTrust { get; set; }

        public bool Vendor_Payment_GoodsOnly { get; set; }
        public bool Vendor_Payment_GoodsAndServices { get; set; }
        public bool Vendor_Payment_RentsLeases { get; set; }
        public bool Vendor_Payment_RentsLeasesAsAgent { get; set; }
        public bool Vendor_Payment_MedicalService { get; set; }
        public bool Vendor_Payment_LegalService { get; set; }
        public bool Vendor_Payment_OtherServices { get; set; }

        [StringLength(250)]
        public string Vendor_Payment_OtherServices_Desc { get; set; }

        public bool Vendor_Payment_Settlement { get; set; }
        public bool Vendor_Payment_CourtAppointedServices { get; set; }

        [Display(Name = "Federal Tax ID")]
        [StringLength(12)]
        [Required]
        public string Vendor_FederalTaxID { get; set; }

        // true: it's FederalTaxID
        // false: it's a SSN
        public bool Vendor_IsFederalTaxID { get; set; }

        [Display(Name = "Street Address")]
        [StringLength(50)]
        [Required]
        public string Vendor_Addr1 { get; set; }

        [StringLength(50)]
        public string Vendor_Addr2 { get; set; }

        [Display(Name = "Vendor City")]
        [StringLength(50)]
        [Required]
        public string Vendor_City { get; set; }

        [Display(Name = "Vendor State")]
        [StringLength(2)]
        [Required]
        public string Vendor_State { get; set; }

        [Display(Name = "Vendor Zip")]
        [StringLength(10)]
        [Required]
        public string Vendor_Zip { get; set; }

        [Display(Name = "Local Business?")]
        [Required]
        public bool Vendor_Local { get; set; }

        [StringLength(2)]
        public string Vendor_Howlong_Years { get; set; }
        [StringLength(2)]
        public string Vendor_Howlong_Months { get; set; }

        [Display(Name = "Contact Name")]
        [StringLength(50)]
        [Required]
        public string Contact_Name { get; set; }

        [Display(Name = "Contact Telephone")]
        [StringLength(25)]
        [Required]
        public string Contact_Phone { get; set; }

        [Display(Name = "Contact FAX")]
        [StringLength(25)]
        public string Contact_FAX { get; set; }

        [Display(Name = "Contact Email")]
        [StringLength(25)]
        [Required]
        public string Contact_Email { get; set; }

        public bool Composition_Public_Entity_Yes { get; set; }
        public bool Composition_Public_Entity_No { get; set; }
        public bool Composition_NonProfit_Public_Yes { get; set; }
        public bool Composition_NonProfit_Public_No { get; set; }

        public bool Ethnicity_Black { get; set; }
        public bool Ethnicity_Hispanic { get; set; }
        public bool Ethnicity_Indian { get; set; }
        public bool Ethnicity_Hawaiian { get; set; }
        public bool Ethnicity_Asian { get; set; }
        public bool Ethnicity_MultiEthnic_Minority_Ownership { get; set; }
        public bool Ethnicity_Caucasian { get; set; }
        public bool Ethnicity_MultiEthnic_Ownership { get; set; }
        public bool Ethnicity_Filipino { get; set; }

        public bool Gender_Female { get; set; }
        public bool Gender_Male { get; set; }

        [Display(Name = "List of Product and/or Services")]
        public string ProductsServices { get; set; }

        [Display(Name = "Auditor's Acknowledgement By")]
        [StringLength(50)]
        public string Auditor_Name { get; set; }

        [Display(Name = "Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? Auditor_SignedDate { get; set; }

        public bool Auditor_Add { get; set; }

        [Display(Name = "Assigned ALCOLINK vendor number")]
        public string Assigned_ALCOLINK_Number { get; set; }

        public bool Auditor_CannotAdd { get; set; }

        [Display(Name = "Cannot Add Reason(s)")]
        public string Auditor_CannotAdd_Reasons { get; set; }

        public bool Auditor_Resubmit { get; set; }
    }
}