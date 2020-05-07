using System.ComponentModel.DataAnnotations;

namespace stake_place_web.Entities.Login
{
    public class MoLoginRequest
    {
        [Required]
        public string ConnectionId { get; set; }
        public string MoLogin { get; set; }
        public string Password { get; set; }
    }
}