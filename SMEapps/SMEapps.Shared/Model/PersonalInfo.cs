using SMEapps.Shared.Enums;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SMEapps.Shared.Model
{
    public class PersonalInfo
    {
        public string? Title { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        public string? ShortName { get; set; }
        public string? Name { get; set; }

        [Required]
        public string? MobileNo { get; set; }

        public string? Email { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public DateOnly? DateOfBirth { get; set; }
        public string? DistrictOfBirth { get; set; }
        public string? Age { get; set; }
        public int? NationalityId { get; set; }
        public int? ReligionId { get; set; }
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
        public string? IdType { get; set; }
        public string? TelephoneResident { get; set; }
        public string? Profession { get; set; }
        public string? Designation { get; set; }
        public string? BusinessName { get; set; }
        public string? OfficeAddress { get; set; }

        public DateTime? DateOfBirthDateTime
        {
            get => DateOfBirth?.ToDateTime(TimeOnly.MinValue);
            set => DateOfBirth = value.HasValue ? DateOnly.FromDateTime(value.Value) : null;
        }

    }
}
