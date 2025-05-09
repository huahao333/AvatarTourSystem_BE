﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public partial class DailyTicketType
    {
        public DailyTicketType()
        {
            Tickets = new HashSet<Ticket>();
        }
        public string? DailyTicketId { get; set; }
        public string? TicketTypeId { get; set; }
        public string? DailyTourId { get; set; }
        public int? Capacity { get; set; }
        public float? DailyTicketPrice { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? Status { get; set; }

        public virtual TicketType? TicketTypes { get; set; }
        public virtual DailyTour? DailyTours { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
