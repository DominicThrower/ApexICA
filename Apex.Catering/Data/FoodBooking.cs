using System.ComponentModel.DataAnnotations;

namespace Apex.Catering.Data
{
    public class FoodBooking
    {
        [Key]
        public int FoodBookingId { get; set; }
        [Required]
        public int ClientReferenceId { get; set; }
        [Required]
        public int NumberOfGuests { get; set; }
        [Required]
        public int MenuId { get; set; }
        public Menu Menu { get; set; } = null!;
    }
}
