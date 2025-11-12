using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMEapps.Shared.Model
{
    public class PersonalInfo
    {
        public string? Title { get; set; }
        [Required(ErrorMessage ="First Name is Required")]
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public string? ShortName { get; set; }
        public string? Name { get; set; }
        public string? Gender { get; set; }

        [Required]
        public DateOnly? DateOfBirth { get; set; }
        public string? DistrictOfBirth { get; set; }
        public string? Age { get; set; }
        public string? Nationality { get; set; }
        public string? Religion { get; set; }
        public string? MaritalStatus { get; set; }
        public string? FatherTitle { get; set; }
        public string? FatherName { get; set; }
        public string? MotherTitle { get; set; }
        public string? MotherName { get; set; }
        public string? SpouseTitle { get; set; }
        public string? SpouseName { get; set; }
        public string? NationalIdNo { get; set; }
        public string? SmartNid { get; set; }
        public string? BirthCertificateNo { get; set; }
        public string? TinNo { get; set; }
        public string? IdType { get; set; }
        public string? Email { get; set; }
        public string? MobileNo { get; set; }
        public string? TelephoneResident { get; set; }
        public string? TelephoneOffice { get; set; }
        public string? FaxNo { get; set; }
        public string? PresentAddress { get; set; }
        public string? ThanaCode { get; set; }
        public string? AreaCode { get; set; }
        public string? PostalCode { get; set; }
        public string? PermanentAddress { get; set; }
        public string? PermanentAreaCode { get; set; }
        public string? PermanentThanaCode { get; set; }
        public string? Profession { get; set; }
        public string? Designation { get; set; }
        public string? BusinessName { get; set; }
        public string? OfficeAddress { get; set; }
        public string? PassNo { get; set; }
        public DateTime? PassIssueDt { get; set; }
        public string? PassIssuePlace { get; set; }
        public string? TradeLicense { get; set; }
        public DateTime? IssueDate { get; set; }
        public string? IssueAuthority { get; set; }
        public string? AnsarRegNo { get; set; }
        public string? AnsarBatNo { get; set; }
    }
}
