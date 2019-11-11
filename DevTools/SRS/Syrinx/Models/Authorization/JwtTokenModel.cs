using System.ComponentModel.DataAnnotations;

namespace VXDesign.Store.DevTools.SRS.Syrinx.Models.Authorization
{
    public class JwtTokenModel
    {
        [Required]
        public string AccessToken { get; set; }

        [Required]
        [StringLength(64)]
        public string RefreshToken { get; set; }
    }
}