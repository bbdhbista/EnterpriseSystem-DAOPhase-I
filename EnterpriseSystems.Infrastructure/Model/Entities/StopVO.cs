using System;
using System.Collections.Generic;

namespace EnterpriseSystems.Infrastructure.Model.Entities
{
    public class StopVO
    {

        public StopVO()
        {
            this.CreatedDate = DateTime.UtcNow;
            this.LastUpdatedDate = DateTime.UtcNow;
            this.Appointments = new List<AppointmentVO>();
            this.Comments = new List<CommentVO>();

        }
        public int Identity { get; set; }
        public string EntityName { get; set; }
        public int EntityIdentity { get; set; }
        public string RoleType { get; set; }
        public string StopNumber { get; set; }
        public string CustomerAlias { get; set; }   
        public string OrganizationName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressCityName { get; set; }
        public string AddressStateCode { get; set; }
        public string AddressCountryCode { get; set; }
        public string AddressPostalCode { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string CreatedUserId { get; set; }
        public string CreatedProgramCode { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public string LastUpdatedUserId { get; set; }
        public string LastUpdatedProgramCode { get; set; }

        public ICollection<AppointmentVO> Appointments { get; set; }
        public ICollection<CommentVO> Comments { get; set; }
    }
}
