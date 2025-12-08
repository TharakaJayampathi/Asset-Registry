using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssetRegistry.Models.User
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "First Name")]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [StringLength(100)]
        public string LastName { get; set; }

        [Display(Name = "Related Division")]
        public int RelatedDivision { get; set; }

        [Display(Name = "Role")]
        public string Role { get; set; }

        [Display(Name = "Profile Picture")]
        public string ProfilePicture { get; set; }

        [NotMapped]
        public List<string> Roles { get; set; }

        public int EnquiryType { get; set; }
        [StringLength(32)]
        public string Code { get; set; }

        public bool IsActive { get; set; }
        [StringLength(150)]
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        [StringLength(150)]
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        [StringLength(150)]
        public string DeletedBy { get; set; }
        public DateTime DeletedOn { get; set; }
    }

    public class LoginModel
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
