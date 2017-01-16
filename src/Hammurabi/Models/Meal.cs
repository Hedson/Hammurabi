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
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        public int PreparationTime { get; set; }

        public ICollection<MealIngredient> MealIngredients { get; set; }
    }
}
