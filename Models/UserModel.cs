using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;

namespace Source.Models
{
    public class UserModel
    {
        [Required]
        [Hidden]
        public string Email { get; set; }

        [Required]
        [Hidden]
        public string Password { get; set; }

        public string Role { get; set; }
    }
}
