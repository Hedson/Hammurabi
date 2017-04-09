using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hammurabi.Data;
using Hammurabi.Models;
using Microsoft.AspNetCore.Authorization;
using Hammurabi.Models.RestaurantViewModels;

namespace Hammurabi.Controllers
{
    public class MealsController : Controller
    {
        private readonly RestaurantContext _context;

        public MealsController(RestaurantContext context)
        {
            _context = context;    
        }

        // GET: Meals. Add sorting Functionality: Preparation Time or Price.
        public async Task<IActionResult> Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            // Added paging functionality for Index action(for standard user - Admin have one list on product, without paging).
            ViewData["CurrentSort"] = sortOrder;
            ViewData["TimeSortParm"] = sortOrder == "Time" ? "Time_desc" : "Time";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "Price_desc" : "Price";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;


            //return View(await _context.Meals.ToListAsync());

            var meals = from m in _context.Meals
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                meals = meals.Where(s => s.Name.Contains(searchString));
            }
            //.OrderBy(s => s.Price);
            switch (sortOrder)
            {
                case "Time_desc":
                    meals = meals
                        .Include(s => s.MealIngredients)
                         .ThenInclude(e => e.Ingredient)
                         .OrderByDescending(s => s.PreparationTime);
                    break;
                case "Time":
                    meals = meals
                        .Include(s => s.MealIngredients)
                         .ThenInclude(e => e.Ingredient)
                         .OrderBy(s => s.PreparationTime);
                    break;
                case "Price_desc":
                    meals = meals
                        .Include(s => s.MealIngredients)
                         .ThenInclude(e => e.Ingredient)
                         .OrderByDescending(s => s.Price);
                    break;

                default:
                    meals = meals
                        .Include(s => s.MealIngredients)
                         .ThenInclude(e => e.Ingredient)
                         .OrderBy(s => s.Price);
                    break;
            }

            int pageSize = 5;

            return View(await PaginatedList<Meal>.CreateAsync(meals.AsNoTracking(), page ?? 1, pageSize)); //with paging functionality

            //return View(await meals
            //            .AsNoTracking()
            //            .ToListAsync());
        }

        [Authorize(Roles="Admin")]
        public async Task<IActionResult> EditMenu(string sortOrder, string searchString)
        {
            ViewData["TimeSortParm"] = sortOrder == "Time" ? "Time_desc" : "Time";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "Price_desc" : "Price";
            ViewData["CurrentFilter"] = searchString;

            //return View(await _context.Meals.ToListAsync());

            var meals = from m in _context.Meals
                        select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                meals = meals.Where(s => s.Name.Contains(searchString));
            }
            //.OrderBy(s => s.Price);
            switch (sortOrder)
            {
                case "Time_desc":
                    meals = meals
                        .Include(s => s.MealIngredients)
                         .ThenInclude(e => e.Ingredient)
                         .OrderByDescending(s => s.PreparationTime);
                    break;
                case "Time":
                    meals = meals
                        .Include(s => s.MealIngredients)
                         .ThenInclude(e => e.Ingredient)
                         .OrderBy(s => s.PreparationTime);
                    break;
                case "Price_desc":
                    meals = meals
                        .Include(s => s.MealIngredients)
                         .ThenInclude(e => e.Ingredient)
                         .OrderByDescending(s => s.Price);
                    break;

                default:
                    meals = meals
                        .Include(s => s.MealIngredients)
                         .ThenInclude(e => e.Ingredient)
                         .OrderBy(s => s.Price);
                    break;
            }


            return View(await meals
                        .AsNoTracking()
                        .ToListAsync());
        }

        // GET: Meals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meal = await _context.Meals
                .Include(s => s.MealIngredients)
                    .ThenInclude(e => e.Ingredient)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);

            if (meal == null)
            {
                return NotFound();
            }

            return View(meal);
        }


        // GET: Meals/Create
        public IActionResult Create()
        {
            var meal = new Meal();
            meal.MealIngredients = new List<MealIngredient>();
            PopulateAssignedIngredientData(meal);
            return View();
        }

        // POST: Meals/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,PreparationTime,Price")] Meal meal, string[] selectedIngredients)
        {

            if (selectedIngredients != null)
            {
                meal.MealIngredients = new List<MealIngredient>();
                foreach (var ingredient in selectedIngredients)
                {
                    var ingredientToAdd = new MealIngredient { MealID = meal.ID, IngredientID = int.Parse(ingredient) };
                    meal.MealIngredients.Add(ingredientToAdd);
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(meal);
                await _context.SaveChangesAsync();
                return RedirectToAction("EditMenu");
            }
            return View(meal);
        }



        // GET: Meals/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meal = await _context.Meals
                .Include(i => i.MealIngredients)//
                    .ThenInclude(i => i.Ingredient)//
                .AsNoTracking()//
                .SingleOrDefaultAsync(m => m.ID == id);
            if (meal == null)
            {
                return NotFound();
            }
            PopulateAssignedIngredientData(meal);//
            return View(meal);
        }

        //Method for httpget action:
        private void PopulateAssignedIngredientData(Meal meal)
        {
            var allIngredients = _context.Ingredients;
            var mealIngredients = new HashSet<int>(meal.MealIngredients.Select(c => c.Ingredient.IngredientID));
            var viewModel = new List<AssignedIngredientData>();
            foreach(var ingredient in allIngredients)
            {
                viewModel.Add(new AssignedIngredientData
                {
                    IngredientID = ingredient.IngredientID,
                    Name = ingredient.Name,
                    Assigned = mealIngredients.Contains(ingredient.IngredientID)
                });
            }
            ViewData["Ingredients"] = viewModel;
        }





        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedIngredients)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mealToUpdate = await _context.Meals
                .Include(i => i.MealIngredients)
                    .ThenInclude(i => i.Ingredient)
                .SingleOrDefaultAsync(m => m.ID == id);

            if (await TryUpdateModelAsync<Meal>(
                mealToUpdate,
                "",
                i => i.Name, i => i.Price, i => i.PreparationTime))
            {

                UpdateMealIngredients(selectedIngredients, mealToUpdate);

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
                return RedirectToAction("EditMenu");
            }
            return View(mealToUpdate);
        }


        private void UpdateMealIngredients(string[] selectedIngredients, Meal mealToUpdate)
        {
            if (selectedIngredients == null)
            {
                mealToUpdate.MealIngredients = new List<MealIngredient>();
                return;
            }

            var selectedIngredientsHS = new HashSet<string>(selectedIngredients);
            var mealsIngredients = new HashSet<int>
                (mealToUpdate.MealIngredients.Select(c => c.Ingredient.IngredientID));
            foreach (var ingredient in _context.Ingredients)    // tutaj by³ bl¹d - zamiast tablicy Ingredients 
                //by³o _context.MealIngredients przez co dodawa³o po kilka elementó(tyle ile akurat bylo w tablicy MealIngredient danego sk³adnika). Teraz dzia³a cacy :)
            {
                if (selectedIngredientsHS.Contains(ingredient.IngredientID.ToString()))
                {
                    if (!mealsIngredients.Contains(ingredient.IngredientID))
                    {
                        mealToUpdate.MealIngredients.Add(new MealIngredient { MealID = mealToUpdate.ID, IngredientID = ingredient.IngredientID });
                    }
                }
                else
                {
                    if (mealsIngredients.Contains(ingredient.IngredientID))
                    {
                        MealIngredient mealToRemove = mealToUpdate.MealIngredients.SingleOrDefault(i => i.IngredientID == ingredient.IngredientID);
                        _context.Remove(mealToRemove);
                    }
                }
            }

        }


        // GET: Meals/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meal = await _context.Meals.SingleOrDefaultAsync(m => m.ID == id);
            if (meal == null)
            {
                return NotFound();
            }

            return View(meal);
        }


        // POST: Meals/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var meal = await _context.Meals.SingleOrDefaultAsync(m => m.ID == id);
            _context.Meals.Remove(meal);
            await _context.SaveChangesAsync();
            return RedirectToAction("EditMenu");
        }

        private bool MealExists(int id)
        {
            return _context.Meals.Any(e => e.ID == id);
        }

    }
}
