using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hammurabi.Models
{
    public class Meal
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int PreparationTime { get; set; }

        public ICollection<MealIngredient> MealIngredients { get; set; }
    }
}
