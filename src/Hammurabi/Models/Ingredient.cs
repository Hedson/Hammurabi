using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Hammurabi.Models
{
    public class Ingredient
    {
        /*// I added this previously, when i want to create ingredients manually in database.
         *  When i create option for user to create new ingredients i have to delete this. Now ingredents have auto-increment ID.*/
        //[DatabaseGenerated(DatabaseGeneratedOption.None)] 
        public int IngredientID { get; set; }

        [Display(Name = "Ingredient name:")]
        [Required(ErrorMessage = "You have to write ingredient name")]
        [StringLength(50)]
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "You have to write price")]
        public int AddPrice { get; set; }

        public ICollection<MealIngredient> MealIngeredient { get; set; }
    }
}
