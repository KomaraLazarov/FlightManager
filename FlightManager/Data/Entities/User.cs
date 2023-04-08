using FlightManager.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FlightManager.Data
{
    public class User: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [StringLength(10, ErrorMessage = "The Egn must be 10 digits", MinimumLength = 10)]
        public string Egn { get; set; }
        public string Address { get; set; }
        public string Role { get; set; }
    }
}
