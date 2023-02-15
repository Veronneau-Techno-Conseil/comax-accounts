using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseFramework.Models.Configurations
{
    public class UserManagedAppsConfig : IModelConfig
    {
        public void SetupFields(ModelBuilder builder)
        {
            builder.Entity<UserApplicationMap>()
                .Property(x => x.HostingType)
                .HasConversion<string>();
        }

        public void SetupRelationships(ModelBuilder builder)
        {
        }

        public void SetupTables(ModelBuilder builder)
        {
            
        }
    }
}
