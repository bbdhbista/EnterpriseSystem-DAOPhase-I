using System;
using System.Collections.Generic;

namespace EnterpriseSystems.Infrastructure.Model.Entities
{
    public class CustomerRequestVO
    {
        public CustomerRequestVO()
        {
            this.Appointments = new List<AppointmentVO>();
            this.Comments = new List<CommentVO>();
            this.ReferenceNumbers = new List<ReferenceNumberVO>();
            this.Stops = new List<StopVO>();
            this.CreatedDate = DateTime.UtcNow;
            this.LastUpdatedDate = DateTime.UtcNow;
            this.TypeCode = "ORDER";
        }

        public int Identity { get; set; }
        public string Status { get; set; }
        public string BusinessEntityKey { get; set; }
        public string TypeCode { get; set; }
        public string ConsumerClassificationType { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedUserId { get; set; }
        public string CreatedProgramCode { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public string LastUpdatedUserId { get; set; }
        public string LastUpdatedProgramCode { get; set; }

        public ICollection<AppointmentVO> Appointments { get; set; }
        public ICollection<CommentVO> Comments { get; set; }
        public ICollection<ReferenceNumberVO> ReferenceNumbers { get; set; }
        public ICollection<StopVO> Stops { get; set; }
    }
}
