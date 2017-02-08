using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hammurabi.Models
{
    public class Meal
    {
        public int ID { get; set; }

        [Display(Name = "Meal name:")]
        [Required(ErrorMessage = "You have to write meal name")]
        [StringLength(50)]
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "You have to write price")]
        public decimal Price { get; set; }

        [Display(Name = "Preparation Time:")]
        [Required(ErrorMessage = "You have to write preparation time")]
        public int PreparationTime { get; set; }

        public ICollection<MealIngredient> MealIngredients { get; set; }
    }
}
