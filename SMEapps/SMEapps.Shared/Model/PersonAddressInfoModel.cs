using System;

namespace SMEapps.Shared.Model
{
    
    public class PersonAddressInfoModel
    {
        public int AddressInfoId { get; set; }

      
        public string AddressType { get; set; } = string.Empty;

       
        public int? LocationCode { get; set; }

        public string AddressLine1 { get; set; } = string.Empty;

      
        public string? AddressLine2 { get; set; }

        
        public string? Remarks { get; set; }

       
        public int? ReferenceId { get; set; }

        
        public string? ReferenceType { get; set; }
    }
}
