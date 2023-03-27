using DatabaseFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunAxiom.Accounts.BusinessLayer.Viewmodels
{
    public interface IUsers
    {
        Task<User> GetUser(string username);
    }
}
