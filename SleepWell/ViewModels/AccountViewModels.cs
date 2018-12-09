using SleepWell.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SleepWell.ViewModels
{
    public class LoginRegisterViewModel
    {
        public LoginViewModel LoginViewModel { get; set; }
        public RegisterViewModel RegisterViewModel { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "Wprowadź poprawny adres email")]
        [Display(Name = "Adres email")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Musisz wprowadzić hasło")]
        [Display(Name = "Hasło")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Należy podać imię")]
        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Należy podać nazwisko")]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Należy podać adres")]
        [Display(Name = "Ulica i nr")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Należy podać miasto")]
        [Display(Name = "Miasto")]
        public string City { get; set; }

        [Required(ErrorMessage = "Należy podać kod pocztowy")]
        [Display(Name = "Kod pocztowy")]
        [DataType(DataType.PostalCode)]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Należy podać nr telefonu")]
        [Display(Name = "Numer telefonu")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Zakładam konto firmowe")]
        public bool IsCompany { get; set; }

        [Display(Name = "Numer NIP")]
        public string NIP { get; set; }

        [Display(Name = "Nazwa firmy")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Wprowadź poprawny adres email")]
        [Display(Name = "Adres email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Compare("Email", ErrorMessage = "Adresy email różnią się")]
        [Display(Name = "Powtórz adres email")]
        [EmailAddress]
        public string ConfirmEmail { get; set; }

        [Required(ErrorMessage = "Musisz wprowadzić hasło")]
        [StringLength(100, ErrorMessage = "Hasło nie może być krótsze niż {2} znaków", MinimumLength = 6)]
        [Display(Name = "Hasło")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Musisz wprowadzić hasło")]
        [Display(Name = "Powtórz hasło")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Hasła różnią się")]
        public string ConfirmPassword { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "Musisz zaakceptować postanowienia regulaminu")]
        public bool RegAccept { get; set; }
    }

    public class UserListViewModel
    {
        public ICollection<User> Users { get; set; }
    }
}