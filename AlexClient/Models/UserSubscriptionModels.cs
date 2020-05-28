using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlexClient.Models
{
    public class UserSubscriptionModels
    {
        [Required]
        [MaxLength(50, ErrorMessage = "trop long")]
        public string Nom { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "trop long")]
        public string Prenom { get; set; }

        [Required]
        [MaxLength(255, ErrorMessage = "trop long")]
        [DataType(DataType.EmailAddress)]
        public string Mail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [MaxLength(25, ErrorMessage = "trop long")]
        [DataType(DataType.PhoneNumber)]
        public string Mobile { get; set; }

        [Required]
        public string Sexe { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateDeNaissance { get; set; }

        public bool Professionnel { get; set; }
    }
}
