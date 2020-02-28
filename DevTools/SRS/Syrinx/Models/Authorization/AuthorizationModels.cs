using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VXDesign.Store.DevTools.Core.Entities.Storage.User;

namespace VXDesign.Store.DevTools.SRS.Syrinx.Models.Authorization
{
    public class SignInModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string Password { get; set; }
    }

    public class SignUpModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string Password { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(32)]
        public string Color { get; set; }
    }

    public class UserAuthorizationModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Color { get; set; }

        public int UserRoleId { get; set; }
        public IEnumerable<UserRolePermissionEntity> Permissions { get; set; }
    }
}