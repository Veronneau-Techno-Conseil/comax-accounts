﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseFramework.Models
{
    public class UserApplicationMap
    {
        public User User { get; set; }
        public string UserId { get; set; }
        public string ApplicationId { get; set; }
        public Application Application { get; set; }

        public HostingTypes HostingType { get; set; }

        public enum HostingTypes
        {
            None = 0,
            NetworkAppliance = 1,
            WindowsDesktop = 2,
            LinuxDesktop = 3,
            MacDesktop = 4,
            Managed = 5
        }
    }
}
