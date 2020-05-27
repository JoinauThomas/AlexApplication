using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlexApi.Models
{
    public class UserSubscriptionModel
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public string Mobile { get; set; }
        public string Sexe { get; set; }
        public DateTime DateDeNaissance { get; set; }
        public bool Professionnel { get; set; }
    }
}
