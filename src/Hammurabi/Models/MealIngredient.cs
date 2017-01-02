using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hammurabi.Models
{
    public class MealIngredient
    {
        public int MealIngredientID { get; set; }
        public int IngredientID { get; set; }
        public int MealID { get; set; }

        public Meal Meal { get; set; }
        public Ingredient Ingredient { get; set; }
    }
}
