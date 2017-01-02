using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hammurabi.Models
{
    public class Ingredient
    {
        public int IngredientID { get; set; }
        public string Name { get; set; }
        public int AddPrice { get; set; }

        public ICollection<MealIngredient> MealIngeredient { get; set; }
    }
}
