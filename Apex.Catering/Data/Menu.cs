using System.ComponentModel.DataAnnotations;

namespace Apex.Catering.Data
{
    public class Menu
    {
        [Key]
        public int MenuId { get; set; }

        [Required]
        [MaxLength(50)]
        public String MenuName { get; set; }
        public IList<FoodBooking> FoodBookings { get; set; } = new List<FoodBooking>();
        public IList<MenuFoodItem> MenuFoodItems { get; set; } = new List<MenuFoodItem>();
    }
}
