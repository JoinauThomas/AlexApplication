using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlexApi.Models
{
    public class MyUser : IdentityUser<int>
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Mobile { get; set; }
        public string Sexe { get; set; }
        public DateTime DateDeNaissance { get; set; }
        public bool Professionnel { get; set; }
    }
}
