using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseFramework.Models.Configurations
{
    public static class DbConfiguration
    {
        static DbConfiguration()
        {
        }
        public static void Setup(ModelBuilder builder)
        {
            var arr = new IModelConfig[]{
                new InitialConfig(),
                new ApplicationClaimsConfig(),
                new StdUserClaimsConfig(),
                new EcosystemsConfig(),
            };

            foreach (var item in arr)
                item.SetupTables(builder);

            foreach (var item in arr)
                item.SetupFields(builder);

            foreach (var item in arr)
                item.SetupRelationships(builder);
        }
    }
}
