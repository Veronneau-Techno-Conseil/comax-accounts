﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseFramework.Models
{
    public class Application : OpenIddict.EntityFrameworkCore.Models.OpenIddictEntityFrameworkCoreApplication<string, Models.Authorization, Models.Token>
    {
        //The Id Property has been added since it was not identified without being added here
        //to make sure if any actions are required
        public override string Id { get => base.Id; set => base.Id = value; }
        public bool Deleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public List<Models.UserApplicationMap> UserApplicationMaps { get; set; }
        public List<Models.ApplicationTypeMap> ApplicationTypeMaps { get; set; }
        public List<Models.AppConfiguration> Configurations { get; set; }
        public virtual IList<Models.AppSecret> Secrets { get; set; }
        //TODO configure foreign key
        public int? AppVersionTagId { get; set; }
        public AppVersionTag AppVersionTag { get; set; }

    }
}