﻿using BusinessObjects.Enums;
using BusinessObjects.ViewModels.Destination;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels.PackageTourFlow
{
    public class FPackageTourCreateModel
    {
        [Required(ErrorMessage = "PackageTourName is required")]
        [StringLength(100, ErrorMessage = "PackageTourName can't be longer than 100 characters")]
        public string? PackageTourName { get; set; }

        public string? PackageTourImgURL { get; set; }
        public int Status { get; set; }       
        [MinLength(1, ErrorMessage = "At least one ticket type must be provided")]
        public List<FTicketTypeCreate> TicketTypesCreate { get; set; } = new List<FTicketTypeCreate>();


    }
}
