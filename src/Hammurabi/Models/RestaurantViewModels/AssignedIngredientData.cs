using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hammurabi.Models.RestaurantViewModels
{
    public class AssignedIngredientData
    {
        public int IngredientID { get; set; }
        public string Name { get; set; }
        public bool Assigned { get; set; }       
    }
}
