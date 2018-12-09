using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace SleepWell.Models
{
    public class User : IdentityUser
    {
        public virtual ICollection<Reservation> Reservations { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Element authenticationType musi pasować do elementu zdefiniowanego w elemencie CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Dodaj tutaj niestandardowe oświadczenia użytkownika
            return userIdentity;
        }

        [Display(Name = "Imię")]
        public string FirstName { get; set; }
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }
        [Display(Name = "Ulica i nr")]
        public string Address { get; set; }
        [Display(Name = "Miasto")]
        public string City { get; set; }
        [Display(Name = "Kod pocztowy")]
        [DataType(DataType.PostalCode)]
        public string PostalCode { get; set; }
        [Display(Name = "Numer NIP")]
        public string NIP { get; set; }
        [Display(Name = "Zakładam konto firmowe")]
        public bool IsCompany { get; set; }
        [Display(Name = "Nazwa firmy")]
        public string CompanyName { get; set; }

    }
}