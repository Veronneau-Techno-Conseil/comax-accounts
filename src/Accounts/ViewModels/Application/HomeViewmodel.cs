using System.Collections.Generic;

namespace CommunAxiom.Accounts.ViewModels.Application
{
    public class HomeViewmodel
    {
        public string FullName { get; set; }
        public bool ManagedAppCreated { get; set; }
        public ManagedAppInfo CommonsManagedAppInfo { get; set; }
    }
}
