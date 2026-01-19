using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinguaMeet.Domain.Entities
{
    public class ApplicationUser: IdentityUser
    {

        public string FullName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
