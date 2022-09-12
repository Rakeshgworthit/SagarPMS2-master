using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace PMS.Models
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(HasPassword)}={HasPassword.ToString()}, {nameof(Logins)}={Logins}, {nameof(PhoneNumber)}={PhoneNumber}, {nameof(TwoFactor)}={TwoFactor.ToString()}, {nameof(BrowserRemembered)}={BrowserRemembered.ToString()}}}";
        }
    }

    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(CurrentLogins)}={CurrentLogins}, {nameof(OtherLogins)}={OtherLogins}}}";
        }
    }

    public class FactorViewModel
    {
        public string Purpose { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(Purpose)}={Purpose}}}";
        }
    }

    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(NewPassword)}={NewPassword}, {nameof(ConfirmPassword)}={ConfirmPassword}}}";
        }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Current Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(OldPassword)}={OldPassword}, {nameof(NewPassword)}={NewPassword}, {nameof(ConfirmPassword)}={ConfirmPassword}}}";
        }
    }

    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(Number)}={Number}}}";
        }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(Code)}={Code}, {nameof(PhoneNumber)}={PhoneNumber}}}";
        }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(SelectedProvider)}={SelectedProvider}, {nameof(Providers)}={Providers}}}";
        }
    }
}