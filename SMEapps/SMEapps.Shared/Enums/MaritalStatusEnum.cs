using System.ComponentModel.DataAnnotations;
namespace SMEapps.Shared.Enums;

public enum MaritalStatusEnum
{
    [Display(Name = "Single")]
    S,

    [Display(Name = "Married")]
    M,

    [Display(Name = "Divorced")]
    D,

    [Display(Name = "Widowed")]
    W
}

