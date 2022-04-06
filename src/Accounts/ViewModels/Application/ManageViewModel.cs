using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunAxiom.Accounts.ViewModels.Application
{
    public class ManageViewModel
    {
      public IAsyncEnumerable<Models.Application> Applications { get; set; }
    }
}
