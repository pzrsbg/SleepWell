using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SleepWell.ViewModels
{
    public class ManageViewModel
    {
        public EditPasswordViewModel EditPasswordViewModel { get; set; }
        public SleepWell.Controllers.ManageController.ManageMessageId? Message { get; set; }
        public EditProfileViewModel EditProfileViewModel { get; set; }
    }

    public class EditProfileViewModel
    {
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Numer telefonu")]
        public string PhoneNumber { get; set; }
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Adres email")]
        public string Email { get; set; }
        [Display(Name = "Imię")]
        public string FirstName { get; set; }
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }
        [Display(Name = "Ulica i nr")]
        public string Address { get; set; }
        [Display(Name = "Miasto")]
        public string City { get; set; }
        [DataType(DataType.PostalCode)]
        [Display(Name = "Kod pocztowy")]
        public string PostalCode { get; set; }
        [Display(Name = "Numer NIP")]
        public string NIP { get; set; }
        [Display(Name = "Zakładam konto firmowe")]
        public bool IsCompany { get; set; }
        [Display(Name = "Nazwa firmy")]
        public string CompanyName { get; set; }
    }

    public class EditPasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Obecne hasło")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Hasło nie może być krótsze niż {2} znaków", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nowe hasło")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Hasła różnią się")]
        [Display(Name = "Potwierdź nowe hasło")]
        public string ConfirmPassword { get; set; }
    }
}