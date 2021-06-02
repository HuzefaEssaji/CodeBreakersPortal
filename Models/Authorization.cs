using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Source.Models
{
    public class Authorization 
    {
        public static bool isAdmin { get; set; } = false;

        public static bool isSuper { get; set; } = false;

        public static bool isStudent { get; set; } = false;   
    }
}
