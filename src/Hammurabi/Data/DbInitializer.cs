using Hammurabi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hammurabi.Models
{
    public static class DbInitializer
    {
        public static void Initialize(RestaurantContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Meals.Any())
            {
                return;   // DB has been seeded
            }

            var meals = new Meal[]
            {
            new Meal{Name="Pizza Margarita",Price=5,PreparationTime=10},
            new Meal{Name="Pizza Pepperoni",Price=6,PreparationTime=150},
            new Meal{Name="Pizza Vegetarian",Price=7,PreparationTime=20},
            new Meal{Name="Pizza Pomodoro",Price=8,PreparationTime=30},
            new Meal{Name="Pizza Paprica",Price=9,PreparationTime=140},
            };
            foreach (Meal s in meals)
            {
                context.Meals.Add(s);
            }
            context.SaveChanges();

            var ingredients = new Ingredient[]
            {
            new Ingredient{IngredientID=101,Name="Cheese",AddPrice=3,},
            new Ingredient{IngredientID=102,Name="Oregano",AddPrice=3,},
            new Ingredient{IngredientID=201,Name="Tomato",AddPrice=3,},
            new Ingredient{IngredientID=202,Name="Onion",AddPrice=2,},
            new Ingredient{IngredientID=203,Name="Pepper",AddPrice=2,},
            new Ingredient{IngredientID=204,Name="Garlic",AddPrice=2,},
            new Ingredient{IngredientID=301,Name="Chicken meat",AddPrice=4,}
            };
            foreach (Ingredient c in ingredients)
            {
                context.Ingredients.Add(c);
            }
            context.SaveChanges();

            var mealingredients = new MealIngredient[]
            {
            new MealIngredient{MealID=1,IngredientID=101},
            new MealIngredient{MealID=1,IngredientID=102},
            new MealIngredient{MealID=1,IngredientID=201},
            new MealIngredient{MealID=2,IngredientID=101},
            new MealIngredient{MealID=2,IngredientID=102},
            new MealIngredient{MealID=2,IngredientID=201},
            new MealIngredient{MealID=2,IngredientID=203},
            new MealIngredient{MealID=3,IngredientID=101},
            new MealIngredient{MealID=3,IngredientID=102},
            new MealIngredient{MealID=3,IngredientID=203},
            new MealIngredient{MealID=3,IngredientID=204},
            new MealIngredient{MealID=3,IngredientID=202},
            new MealIngredient{MealID=4,IngredientID=101},
            new MealIngredient{MealID=4,IngredientID=102},
            new MealIngredient{MealID=4,IngredientID=201},
            new MealIngredient{MealID=4,IngredientID=301},
            new MealIngredient{MealID=5,IngredientID=101},
            new MealIngredient{MealID=5,IngredientID=102},
            new MealIngredient{MealID=5,IngredientID=201},
            new MealIngredient{MealID=5,IngredientID=301},
            };
            foreach (MealIngredient e in mealingredients)
            {
                context.MealIngredients.Add(e);
            }
            context.SaveChanges();
        }
    }
}
