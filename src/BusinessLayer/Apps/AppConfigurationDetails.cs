using DatabaseFramework.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommunAxiom.Accounts.BusinessLayer.Apps
{
    public class AppConfigurationDetails: AppConfiguration
    {
        public string AppConfigurationKey { get; set; }
        public bool FromAppDefault { get; set; }
    }
}
