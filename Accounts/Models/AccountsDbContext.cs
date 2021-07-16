using CommunAxiom.Accounts.Models.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunAxiom.Accounts.Models
{
    public class AccountsDbContext : IdentityDbContext<User>
    {
        public AccountsDbContext(): base()
        {

        }

        public AccountsDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
     
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            DbConfiguration.Setup(builder);
        }
    }
}
