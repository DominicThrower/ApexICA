using System.ComponentModel.DataAnnotations;

namespace Apex.Catering.Data
{
    public class FoodItem
    {
        [Key]
        public int FoodItemId { get; set; }
        [MaxLength(50)]
        public String Description { get; set; }
        public float UnitPrice { get; set; }

        public IList<MenuFoodItem> MenuFoodItems { get; set; } = new List<MenuFoodItem>();
    }
}
