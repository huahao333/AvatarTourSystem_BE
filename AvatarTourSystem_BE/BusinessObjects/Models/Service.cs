using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public partial class Service
    {
        public Service()
        {
            ServiceByTourSegments = new HashSet<ServiceByTourSegment>();
            ServiceUsedByTickets = new HashSet<ServiceUsedByTicket>();
        }
        public string? ServiceId { get; set; }
        public string? ServiceName { get; set; }
        public float? ServicePrice { get; set; }
        public string? ServiceImgUrl { get; set; }
        public string? ServiceTypeId { get; set; }
        public string? LocationId { get; set; }
        public string? SupplierId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? Status { get; set; }

        public virtual ServiceType? ServiceTypes { get; set; }
        public virtual Location? Locations { get; set; }
        public virtual Supplier? Suppliers { get; set; }
        public virtual ICollection<ServiceByTourSegment> ServiceByTourSegments { get; set; }
        public virtual ICollection<ServiceUsedByTicket> ServiceUsedByTickets { get; set; }
    }
}
