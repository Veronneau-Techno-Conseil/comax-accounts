using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OIDCClient.ViewModels.Auhtenticate
{
    public class AuthorizeViewModel
    {
        [Display( Name = "Client Id")]
        public string ClientId { get; set; }
    }
}
